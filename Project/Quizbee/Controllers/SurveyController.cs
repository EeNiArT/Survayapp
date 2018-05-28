using Quizbee.Commons;
using Quizbee.Database;
using Quizbee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Quizbee.Controllers
{
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

                model.TotalCount = db.Quizzes.Where(q => q.IsActive).Count();
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
            QuizViewModel model = new QuizViewModel();

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

                var quiz = await db.Quizzes.FindAsync(ID.Value);

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

                model.Questions = quiz.Questions.Where(q => q.IsActive).ToList();

                return View(model);
            }
            else return HttpNotFound();
        }
    }
}