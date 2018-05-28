using Quizbee.Database;
using Quizbee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}