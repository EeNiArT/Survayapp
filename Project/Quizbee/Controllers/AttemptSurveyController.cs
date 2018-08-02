using Microsoft.AspNet.Identity;
using Quizbee.Commons;
using Quizbee.Database;
using Quizbee.Models;
using Quizbee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Quizbee.Controllers
{
    [Authorize]
    public class AttemptSurveyController : Controller
    {
        // GET: AttemptSurvey
        ApplicationDbContext db;

        public AttemptSurveyController()
        {
            db = new ApplicationDbContext();
        }

        [HttpGet]
        public async Task<ActionResult> Attempt(int SurveyID)
        {
            var quiz = await db.Surveys.FindAsync(SurveyID);

            if (quiz == null || !quiz.IsActive) return HttpNotFound();

            SurveyViewModel model = new SurveyViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = string.Format("Survey : {0}", quiz.Name),
                PageDescription = quiz.Description
            };

            model.ID = quiz.ID;
            model.Name = quiz.Name;
            model.Description = quiz.Description;
            model.TimeDuration = quiz.TimeDuration;
            model.Hours = quiz.TimeDuration.Hours;
            model.Minutes = quiz.TimeDuration.Minutes;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Attempt(AttemptSurveyViewModel model)
        {
            var quiz = await db.Surveys.FindAsync(model.SurveyID);

            if (quiz == null || !quiz.IsActive) return HttpNotFound();

            if (quiz.SQuestions.Count == 0) return HttpNotFound();

            StudentSurvey studentQuiz = new StudentSurvey();

            studentQuiz.StudentID = User.Identity.GetUserId();
            studentQuiz.Survey = quiz;
            studentQuiz.StartedAt = DateTime.Now;
            studentQuiz.IsActive = true;
            studentQuiz.ModifiedOn = DateTime.Now;

            db.StudentSurveys.Add(studentQuiz);
            await db.SaveChangesAsync();

            model.StudentSurveyID = studentQuiz.ID;
            model.TotalQuestions = quiz.SQuestions.Count;
            model.SQuestion = quiz.SQuestions.Where(q => q.IsActive).FirstOrDefault();
            model.QuestionIndex = 0;

            model.Options = new List<Option>();
            model.Options.AddRange(model.SQuestion.Options.Where(q => q.IsActive));
            model.Options.Shuffle();

            return PartialView("SurveySQuestion", model);
        }

        [HttpPost]
        public async Task<ActionResult> AnswerSQuestion(AttemptSurveyViewModel model)
        {
            if (model.TimerExpired)
            {
                var studentQuiz = await db.StudentSurveys.FindAsync(model.StudentSurveyID);

                studentQuiz.CompletedAt = DateTime.Now;
                studentQuiz.ModifiedOn = DateTime.Now;

                db.Entry(studentQuiz).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();

                StudentSurveyViewModel studentQuizModel = new StudentSurveyViewModel();

                studentQuizModel.StudentSurvey = studentQuiz;
                studentQuizModel.TimerExpired = model.TimerExpired;

                return PartialView("AttemptSDetailsPartial", studentQuizModel);
            }
            else
            {
                var quiz = await db.Surveys.FindAsync(model.SurveyID);

                if (quiz == null || !quiz.IsActive) return HttpNotFound();

                var question = quiz.SQuestions.Find(q => q.ID == model.QuestionID);

                if (question == null || !question.IsActive) return HttpNotFound();

                var selectedOption = question.Options.Find(o => o.ID == model.SelectedOptionID);

                if (selectedOption == null || !selectedOption.IsActive) return HttpNotFound();

                AttemptedSQuestion attemptedQuestion = new AttemptedSQuestion();

                attemptedQuestion.SQuestion = question;
                attemptedQuestion.SelectedOption = selectedOption;
                attemptedQuestion.AnswerValue = model.AnswerValue;
                attemptedQuestion.AnsweredAt = DateTime.Now;

                if (selectedOption.IsCorrect)
                    attemptedQuestion.IsCorrect = true;

                var studentQuiz = await db.StudentSurveys.FindAsync(model.StudentSurveyID);

                if (studentQuiz.AttemptedSQuestions == null)
                    studentQuiz.AttemptedSQuestions = new List<AttemptedSQuestion>();

                attemptedQuestion.IsActive = true;
                attemptedQuestion.ModifiedOn = DateTime.Now;

                studentQuiz.AttemptedSQuestions.Add(attemptedQuestion);

                if (model.QuestionIndex == quiz.SQuestions.Count - 1) //this was the Last question
                {
                    studentQuiz.CompletedAt = DateTime.Now;
                }

                db.Entry(studentQuiz).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();

                if (model.QuestionIndex != quiz.SQuestions.Where(q => q.IsActive).Count() - 1)
                {
                    model.TotalQuestions = quiz.SQuestions.Count;
                    model.SQuestion = quiz.SQuestions.Where(q => q.IsActive).ElementAtOrDefault(model.QuestionIndex + 1);
                    model.QuestionIndex = model.QuestionIndex + 1;

                    model.Options = new List<Option>();
                    model.Options.AddRange(model.SQuestion.Options.Where(o => o.IsActive));
                    model.Options.Shuffle();

                    return PartialView("SurveySQuestion", model);
                }
                else //this was the Last question so display the result
                {
                    StudentSurveyViewModel studentQuizModel = new StudentSurveyViewModel();

                    studentQuizModel.StudentSurvey = studentQuiz;
                    studentQuizModel.TimerExpired = model.TimerExpired;

                    //return PartialView("AttemptSDetailsPartial", studentQuizModel);
                    return RedirectToAction("ThankYou", "HOme");
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> AttemptSDetails(int attemptID)
        {
            var studentQuiz = await db.StudentSurveys.FindAsync(attemptID);

            if (studentQuiz == null || !studentQuiz.IsActive) return HttpNotFound();

            StudentSurveyViewModel model = new StudentSurveyViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Survey Attempt Details",
                PageDescription = "Details of Attempted Survey"
            };

            model.StudentSurvey = studentQuiz;

            return View(model);
        }

        [HttpGet]
        public ActionResult MyResults(bool? isPartial, string search, int? page, int? items)
        {
            var UserId = User.Identity.GetUserId();

            StudentSurveyListViewModel model = new StudentSurveyListViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "My Survey Results",
                PageDescription = "Results of My Survey Attempts"
            };

            model.searchTerm = search;
            model.pageNo = page ?? 1;
            model.pageSize = items ?? 10;

            if (string.IsNullOrEmpty(model.searchTerm))
            {
                model.StudentSurveys = db.StudentSurveys
                                    .Where(q => q.IsActive)
                                    .Where(x => x.StudentID == UserId)
                                    .OrderByDescending(x => x.ModifiedOn)
                                    .Skip((model.pageNo - 1) * model.pageSize)
                                    .Take(model.pageSize)
                                    .ToList();

                model.TotalCount = db.StudentSurveys
                                    .Where(q => q.IsActive)
                                    .Where(x => x.StudentID == UserId)
                                    .Count();
            }
            else
            {
                model.StudentSurveys = db.StudentSurveys
                                    .Where(q => q.IsActive)
                                    .Where(x => x.StudentID == UserId)
                                    .Where(x => x.Survey.Name.ToLower().StartsWith(model.searchTerm))
                                    .OrderByDescending(x => x.ModifiedOn)
                                    .Skip((model.pageNo - 1) * model.pageSize)
                                    .Take(model.pageSize)
                                    .ToList();

                model.TotalCount = db.StudentSurveys
                                    .Where(q => q.IsActive)
                                    .Where(x => x.StudentID == UserId)
                                    .Where(x => x.Survey.Name.ToLower().StartsWith(model.searchTerm))
                                    .Count();
            }

            model.Pager = new Pager(model.TotalCount, model.pageNo, model.pageSize);

            if (isPartial.HasValue && isPartial.Value)
            {
                model.isPartialView = true;
                return PartialView(model);
            }
            else return View(model);
        }
    }
}