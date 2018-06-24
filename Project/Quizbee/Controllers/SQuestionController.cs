using Quizbee.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quizbee.ViewModels;
using System.Threading.Tasks;
using Quizbee.Commons;
using Quizbee.Models;

namespace Quizbee.Controllers
{
    [Authorize]
    public class SQuestionController : Controller
    {
        // GET: SQuestion
        ApplicationDbContext db;

        public SQuestionController()
        {
            db = new ApplicationDbContext();
        }

        public ActionResult Index(int SurveyID, string search, int? page, int? items)
        {
            SQuestionListingViewModel model = new SQuestionListingViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Questions",
                PageDescription = "List of Questions for Selected Survey."
            };

            model.searchTerm = search;
            model.pageNo = page ?? 1;
            model.pageSize = items ?? 10;

            var quiz = db.Surveys.Where(q => q.IsActive).FirstOrDefault(x => x.ID == SurveyID);

            if (quiz != null)
            {
                model.SurveyID = SurveyID;

                if (string.IsNullOrEmpty(model.searchTerm))
                {
                    model.SQuestions = quiz.SQuestions
                                        .Where(q => q.IsActive && q.IsQuiz == false)
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((model.pageNo - 1) * model.pageSize)
                                        .Take(model.pageSize)
                                        .ToList();

                    model.TotalCount = quiz.SQuestions
                                        .Where(q => q.IsActive && q.IsQuiz == false).Count();
                }
                else
                {
                    model.SQuestions = quiz.SQuestions
                                        .Where(q => q.IsActive && q.IsQuiz == false)
                                        .Where(x => x.Title.ToLower().StartsWith(model.searchTerm))
                                        .OrderByDescending(x => x.ModifiedOn)
                                        .Skip((model.pageNo - 1) * model.pageSize)
                                        .Take(model.pageSize)
                                        .ToList();

                    model.TotalCount = quiz.SQuestions
                                        .Where(q => q.IsActive && q.IsQuiz == false)
                                        .Where(x => x.Title.ToLower().StartsWith(model.searchTerm))
                                        .Count();
                }

                model.Pager = new Pager(model.TotalCount, model.pageNo, model.pageSize);
            }
            else return HttpNotFound();

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> SQuestionOperation(string Operation, int SurveyID, int? ID)
        {
            SQuestionViewModel model = new SQuestionViewModel();

            if (Operation == Operations.Create)
            {
                var quiz = await db.Surveys.FindAsync(SurveyID);

                if (quiz == null || !quiz.IsActive) return HttpNotFound();

                model.PageInfo = new PageInfo()
                {
                    PageTitle = "Add New Question",
                    PageDescription = "Add questions to selected quiz."
                };

                model.SurveyID = quiz.ID;

                return View(model);
            }
            else if (Operation == Operations.Modify)
            {
                if (!ID.HasValue) return HttpNotFound();

                var quiz = await db.Surveys.FindAsync(SurveyID);

                if (quiz == null || !quiz.IsActive) return HttpNotFound();

                var question = quiz.SQuestions.FirstOrDefault(q => q.ID == ID.Value && q.IsQuiz == false);

                if (question == null || !question.IsActive) return HttpNotFound();

                model.PageInfo = new PageInfo()
                {
                    PageTitle = "Modify Question",
                    PageDescription = "Modify selected question."
                };

                model.SurveyID = SurveyID;
                model.ID = question.ID;
                model.Title = question.Title;
                model.TypeOfQuestion = Operations.GetQuestionType(question.TypeOfQuestion);//question.TypeOfQuestion.ToString();
                model.Options = question.Options.Where(o => o.IsActive).ToList();
                model.CorrectOption = model.Options.Find(q => q.IsCorrect);

                model.Options.Remove(model.CorrectOption);

                return View(model);
            }
            else return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> SQuestionOperation(string Operation, FormCollection collection)
        {
            QuestionViewModel model = GetQuestionViewModelFromFormCollection(collection);

            if (Operation == Operations.Create)
            {
                var quiz = await db.Surveys.FindAsync(model.QuizID);

                if (quiz == null || !quiz.IsActive) return HttpNotFound();

                model.PageInfo = new PageInfo()
                {
                    PageTitle = "Add New Question",
                    PageDescription = "Add questions to selected quiz."
                };

                if (model.TypeOfQuestion == "mc")
                {
                    if (string.IsNullOrEmpty(model.Title) || model.CorrectOption == null || model.Options.Count == 0 || model.TypeOfQuestion == null)
                    {
                        if (string.IsNullOrEmpty(model.TypeOfQuestion))
                        {
                            ModelState.AddModelError("TypeOfQuestion", "Please select Answer type.");
                        }
                        if (string.IsNullOrEmpty(model.Title))
                        {
                            ModelState.AddModelError("Title", "Please enter question title.");
                        }

                        if (model.CorrectOption == null)
                        {
                            ModelState.AddModelError("CorrectOption", "Please enter correct option.");
                        }

                        if (model.Options.Count == 0)
                        {
                            ModelState.AddModelError("Options", "Please enter some other options.");
                        }

                        return View(model);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(model.Title) || model.TypeOfQuestion == null)
                    {


                        if (string.IsNullOrEmpty(model.TypeOfQuestion))
                        {
                            ModelState.AddModelError("TypeOfQuestion", "Please select Answer type.");
                        }
                        if (string.IsNullOrEmpty(model.Title))
                        {
                            ModelState.AddModelError("Title", "Please enter question title.");
                        }
                        return View(model);
                    }
                }

                await CreateQuestionAsync(quiz, model);

                return RedirectToAction("SQuestionOperation", new { Operation = Operations.Create, QuizID = model.QuizID });
            }
            else if (Operation == Operations.Update && model.ID > 0)
            {
                var quiz = await db.Surveys.FindAsync(model.QuizID);

                if (quiz == null || !quiz.IsActive) return HttpNotFound();

                var question = await db.SQuestions.FindAsync(model.ID);

                if (question == null || !question.IsActive || question.IsQuiz) return HttpNotFound();

                model.PageInfo = new PageInfo()
                {
                    PageTitle = "Modify Question",
                    PageDescription = "Modify selected question."
                };

                if (model.TypeOfQuestion == "mc")
                {
                    if (string.IsNullOrEmpty(model.Title) || model.CorrectOption == null || model.Options.Count == 0 || model.TypeOfQuestion == null)
                    {
                        if (string.IsNullOrEmpty(model.TypeOfQuestion))
                        {
                            ModelState.AddModelError("TypeOfQuestion", "Please select Answer type.");
                        }
                        if (string.IsNullOrEmpty(model.Title))
                        {
                            ModelState.AddModelError("Title", "Please enter question title.");
                        }

                        if (model.CorrectOption == null)
                        {
                            ModelState.AddModelError("CorrectOption", "Please enter correct option.");
                        }

                        if (model.Options.Count == 0)
                        {
                            ModelState.AddModelError("Options", "Please enter some other options.");
                        }

                        return View(model);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(model.Title) || model.TypeOfQuestion == null)
                    {


                        if (string.IsNullOrEmpty(model.TypeOfQuestion))
                        {
                            ModelState.AddModelError("TypeOfQuestion", "Please select Answer type.");
                        }
                        if (string.IsNullOrEmpty(model.Title))
                        {
                            ModelState.AddModelError("Title", "Please enter question title.");
                        }
                        return View(model);
                    }
                }

                await UpdateQuestionAsync(quiz, model, question);

                return RedirectToAction("Index", new { quizID = question.SurveyID });
            }
            else if (Operation == Operations.Delete && model.ID > 0)
            {
                var quiz = await db.Surveys.FindAsync(model.QuizID);

                if (quiz == null || !quiz.IsActive) return HttpNotFound();

                var question = await db.SQuestions.FindAsync(model.ID);

                if (question == null || !question.IsActive) return HttpNotFound();

                await DeleteQuestionAsync(quiz, model, question);

                return RedirectToAction("Index", new { quizID = question.SurveyID });
            }
            else return HttpNotFound();
        }

        private async Task<SQuestion> CreateQuestionAsync(Survey quiz, QuestionViewModel model)
        {
            SQuestion question = new SQuestion();
            question.SurveyID = quiz.ID;
            question.IsQuiz = false;
            question.Title = model.Title;
            question.TypeOfQuestion = Operations.GetQuestionType(model.TypeOfQuestion);

            if (model.CorrectOption != null)
            {
                question.Options = new List<Option>();
                question.Options.Add(model.CorrectOption);
                question.Options.AddRange(model.Options);
            }

            question.IsActive = true;
            question.ModifiedOn = DateTime.Now;

            db.SQuestions.Add(question);

            await db.SaveChangesAsync();

            return question;
        }

        private async Task<SQuestion> UpdateQuestionAsync(Survey quiz, QuestionViewModel model, SQuestion question)
        {
            foreach (var option in question.Options.ToList())
            {
                option.IsActive = false;
                option.ModifiedOn = DateTime.Now;

                db.Entry(question).State = System.Data.Entity.EntityState.Modified;
            }

            question.SurveyID = quiz.ID;
            question.Title = model.Title;
            question.TypeOfQuestion = Operations.GetQuestionType(model.TypeOfQuestion);
            if(model.CorrectOption != null)
            {
                question.Options = new List<Option>();
                question.Options.Add(model.CorrectOption);
                question.Options.AddRange(model.Options);
            }
            

            question.ModifiedOn = DateTime.Now;

            db.Entry(question).State = System.Data.Entity.EntityState.Modified;

            await db.SaveChangesAsync();

            return question;
        }

        private async Task<SQuestion> DeleteQuestionAsync(Survey quiz, QuestionViewModel model, SQuestion question)
        {
            foreach (var attemptedQuestion in db.AttemptedQuestions.Where(at => at.Question.ID == question.ID).ToList())
            {
                attemptedQuestion.IsActive = false;
                attemptedQuestion.ModifiedOn = DateTime.Now;

                db.Entry(attemptedQuestion).State = System.Data.Entity.EntityState.Modified;
            }

            question.IsActive = false;
            question.ModifiedOn = DateTime.Now;

            db.Entry(question).State = System.Data.Entity.EntityState.Modified;

            await db.SaveChangesAsync();

            return question;
        }

        private QuestionViewModel GetQuestionViewModelFromFormCollection(FormCollection collection)
        {
            QuestionViewModel model = new QuestionViewModel();

            model.Options = new List<Option>();

            if (collection.AllKeys.Count() > 0)
            {
                foreach (string key in collection)
                {
                    if (key == "QuizID")
                    {
                        model.QuizID = int.Parse(collection[key]);
                    }
                    else if (key == "ID")
                    {
                        model.ID = int.Parse(collection[key]);
                    }
                    else if (key == "Title")
                    {
                        model.Title = collection[key];
                    }

                    else if (key.Contains("optionNo")) //this must be Option
                    {
                        if (!string.IsNullOrEmpty(collection[key]))
                        {
                            string OptionIndex = key.Substring("optionNo".Length, key.Length - "optionNo".Length);

                            model.Options.Add(
                                new Option()
                                {
                                    Answer = collection[key],
                                    IsActive = true
                                }
                            );
                        }
                    }
                    else if (key == "qtyperadio")
                    {
                        model.TypeOfQuestion = collection[key];

                        if (model.TypeOfQuestion == "mc" && !string.IsNullOrEmpty(collection["CorrectOption"]))
                        {
                            model.CorrectOption = new Option()
                            {
                                Answer = collection["CorrectOption"],
                                IsCorrect = true,
                                IsActive = true
                            };
                        }
                        if (model.TypeOfQuestion == "sc" && !string.IsNullOrEmpty(collection["smileyrating"]))
                        {
                            model.CorrectOption = new Option()
                            {
                                Answer = collection["smileyrating"],
                                IsCorrect = true,
                                IsActive = true
                            };
                        }
                        if (model.TypeOfQuestion == "lc" && !string.IsNullOrEmpty(collection["starrating"]))
                        {
                            model.CorrectOption = new Option()
                            {
                                Answer = collection["starrating"],
                                IsCorrect = true,
                                IsActive = true
                            };
                        }
                        if (model.TypeOfQuestion == "oc" && !string.IsNullOrEmpty(collection["Description"]))
                        {
                            model.CorrectOption = new Option()
                            {
                                Answer = collection["Description"],
                                IsCorrect = true,
                                IsActive = true
                            };
                        }

                    }

                    else
                    {
                        var dfgdf = collection[key];
                    }
                }
            }

            return model;
        }
    }
}