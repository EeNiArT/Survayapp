using Quizbee.Commons;
using Quizbee.Database;
using Quizbee.Models;
using Quizbee.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Quizbee.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {

        ApplicationDbContext db;

        public SurveyController()
        {
            db = new ApplicationDbContext();
        }

        // GET: Survey
        public ActionResult Index(string search, int? page, int? items)
        {
            SurveyListViewModel model = new SurveyListViewModel();

            model.PageInfo = new PageInfo()
            {
                PageTitle = "Surveys",
                PageDescription = "List of Survays."
            };

            model.searchTerm = search;
            model.pageNo = page ?? 1;
            model.pageSize = items ?? 10;

            if (string.IsNullOrEmpty(model.searchTerm))
            {
                model.Surveys = db.Surveys
                                    .Where(q => q.IsActive)
                                    .OrderByDescending(x => x.ModifiedOn)
                                    .Skip((model.pageNo - 1) * model.pageSize)
                                    .Take(model.pageSize)
                                    .ToList();

                model.TotalCount = db.Surveys.Where(q => q.IsActive).Count();
            }
            else
            {
                model.Surveys = db.Surveys
                                    .Where(q => q.IsActive)
                                    .Where(x => x.Name.ToLower().StartsWith(model.searchTerm))
                                    .OrderByDescending(x => x.ModifiedOn)
                                    .Skip((model.pageNo - 1) * model.pageSize)
                                    .Take(model.pageSize)
                                    .ToList();

                model.TotalCount = db.Surveys
                                    .Where(q => q.IsActive)
                                    .Where(x => x.Name.ToLower().StartsWith(model.searchTerm))
                                    .Count();
            }

            model.Pager = new Pager(model.TotalCount, model.pageNo, model.pageSize);

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> SurveyOperation(string Operation, int? ID)
        {
            SurveyViewModel model = new SurveyViewModel();

            if (Operation == Operations.Create)
            {
                model.PageInfo = new PageInfo()
                {
                    PageTitle = "Create New Survey",
                    PageDescription = "Create new survey."
                };

                return View(model);
            }
            else if (Operation == Operations.Modify)
            {
                if (!ID.HasValue) return HttpNotFound();

                var quiz = await db.Surveys.FindAsync(ID.Value);

                if (quiz == null || !quiz.IsActive)
                    return HttpNotFound();

                model.PageInfo = new PageInfo()
                {
                    PageTitle = "Modify Survey",
                    PageDescription = "Modify this survey."
                };

                model.ID = quiz.ID;
                model.Name = quiz.Name;
                model.Description = quiz.Description;
                model.Hours = quiz.TimeDuration.Hours;
                model.Minutes = quiz.TimeDuration.Minutes;

                model.SQuestions = quiz.SQuestions.Where(q => q.IsActive).ToList();

                return View(model);
            }
            else return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> SurveyOperation(SurveyViewModel model)
        {
            if (model.Operation == Operations.Create)
            {
                model.PageInfo = new PageInfo()
                {
                    PageTitle = "Create New Survey",
                    PageDescription = "Create new survey."
                };

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                Survey quiz = await CreateQuizAsync(model);

                return RedirectToAction("SQuestionOperation", "SQuestion", new { Operation = Operations.Create, QuizID = quiz.ID, });
            }
            else if (model.Operation == Operations.Update && model.ID > 0)
            {
                model.PageInfo = new PageInfo()
                {
                    PageTitle = "Modify Survey",
                    PageDescription = "Modify this survey."
                };

                var quiz = await db.Surveys.FindAsync(model.ID);

                if (quiz == null) return HttpNotFound();

                if (!ModelState.IsValid)
                {
                    model.SQuestions = quiz.SQuestions.Where(q => q.IsActive).ToList();

                    return View(model);
                }

                await UpdateQuizAsync(model, quiz);

                return RedirectToAction("Index");
            }
            else if (model.Operation == Operations.Delete && model.ID > 0)
            {
                model.PageInfo = new PageInfo()
                {
                    PageTitle = "Modify Survey",
                    PageDescription = "Modify this survey."
                };

                var quiz = await db.Surveys.FindAsync(model.ID);

                if (quiz == null) return HttpNotFound();

                await DeleteQuizAsync(model, quiz);

                return RedirectToAction("Index");
            }
            else return HttpNotFound();
        }

        #region Supporting method

        private async Task<Survey> CreateQuizAsync(SurveyViewModel model)
        {
            Survey quiz = new Survey();

            quiz.Name = model.Name;
            quiz.Description = model.Description;
            quiz.TimeDuration = new TimeSpan(model.Hours, model.Minutes, 0);
            quiz.ModifiedOn = DateTime.Now;
            quiz.IsActive = true;

            db.Surveys.Add(quiz);
            await db.SaveChangesAsync();

            return quiz;
        }

        private async Task<Survey> UpdateQuizAsync(SurveyViewModel model, Survey quiz)
        {
            quiz.Name = model.Name;
            quiz.Description = model.Description;
            quiz.TimeDuration = new TimeSpan(model.Hours, model.Minutes, 0);
            quiz.ModifiedOn = DateTime.Now;

            db.Entry(quiz).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return quiz;
        }

        private async Task<Survey> DeleteQuizAsync(SurveyViewModel model, Survey quiz)
        {
            quiz.IsActive = false;
            quiz.ModifiedOn = DateTime.Now;

            db.Entry(quiz).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return quiz;
        }

        #endregion
    }
}