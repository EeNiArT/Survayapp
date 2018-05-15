using Quizbee.Database;
using Quizbee.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quizbee.Controllers
{
	public class HomeController : Controller
	{
		ApplicationDbContext db;

		public HomeController()
		{
			db = new ApplicationDbContext();
		}

		public ActionResult Index()
		{
			HomeViewModel model = new HomeViewModel();

			model.PageInfo = new PageInfo()
			{
				PageTitle = "Quizbee",
				PageDescription = "Quizbee helps you to create scalable and dynamic quizzes with any number of questions and related options. Creating and attempting Quizzes have never been this easy. Try it now!"
			};

			model.Quizzes = db.Quizzes
									.Where(q=>q.IsActive)
									.Where(q=>q.Questions.Count > 0)
									.OrderByDescending(x => x.ModifiedOn)
									.Take(9)
									.ToList();

			return View(model);
		}		
	}
}