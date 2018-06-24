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
	public class QuizController : Controller
    {
		ApplicationDbContext db;

		public QuizController()
		{
			db = new ApplicationDbContext();
		}

        public ActionResult Index(string search, int? page, int? items)
        {
			QuizListViewModel model = new QuizListViewModel();

			model.PageInfo = new PageInfo()
			{
				PageTitle = "Quizzes",
				PageDescription = "List of Quizzes."
			};

			model.searchTerm = search;
			model.pageNo = page ?? 1;
			model.pageSize = items ?? 10;

			if (string.IsNullOrEmpty(model.searchTerm))
			{
				model.Quizzes = db.Quizzes
									.Where(q=>q.IsActive)
									.OrderByDescending(x => x.ModifiedOn)
									.Skip((model.pageNo - 1) * model.pageSize)
									.Take(model.pageSize)
									.ToList();

				model.TotalCount = db.Quizzes.Where(q => q.IsActive).Count();
			}
			else
			{
				model.Quizzes = db.Quizzes
									.Where(q => q.IsActive)
									.Where(x => x.Name.ToLower().StartsWith(model.searchTerm))
									.OrderByDescending(x => x.ModifiedOn)
									.Skip((model.pageNo - 1) * model.pageSize)
									.Take(model.pageSize)
									.ToList();

				model.TotalCount = db.Quizzes
									.Where(q => q.IsActive)
									.Where(x => x.Name.ToLower().StartsWith(model.searchTerm))
									.Count();				
			}

			model.Pager = new Pager(model.TotalCount, model.pageNo, model.pageSize);

			return View(model);
        }
		
		[HttpGet]
		public async Task<ActionResult> QuizOperation(string Operation, int? ID)
		{
			QuizViewModel model = new QuizViewModel();

			if (Operation == Operations.Create)
			{
				model.PageInfo = new PageInfo()
				{
					PageTitle = "Create New Quiz",
					PageDescription = "Create new quiz."
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
					PageTitle = "Modify Quiz",
					PageDescription = "Modify this quiz."
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

		[HttpPost]
		public async Task<ActionResult> QuizOperation(QuizViewModel model)
		{
			if (model.Operation == Operations.Create)
			{
				model.PageInfo = new PageInfo()
				{
					PageTitle = "Create New Quiz",
					PageDescription = "Create new quiz."
				};

				if (!ModelState.IsValid)
				{
					return View(model);
				}

				Quiz quiz = await CreateQuizAsync(model);
				
				return RedirectToAction("QuestionOperation", "Question", new { Operation = Operations.Create, QuizID = quiz.ID,  });
			}
			else if (model.Operation == Operations.Update && model.ID > 0)
			{
				model.PageInfo = new PageInfo()
				{
					PageTitle = "Modify Quiz",
					PageDescription = "Modify this quiz."
				};

				var quiz = await db.Quizzes.FindAsync(model.ID);

				if (quiz == null) return HttpNotFound();

				if (!ModelState.IsValid)
				{
					model.Questions = quiz.Questions.Where(q=>q.IsActive).ToList();

					return View(model);
				}

				await UpdateQuizAsync(model, quiz);

				return RedirectToAction("Index");
			}
			else if (model.Operation == Operations.Delete && model.ID > 0)
			{
				model.PageInfo = new PageInfo()
				{
					PageTitle = "Modify Quiz",
					PageDescription = "Modify this quiz."
				};

				var quiz = await db.Quizzes.FindAsync(model.ID);

				if (quiz == null) return HttpNotFound();
				
				await DeleteQuizAsync(model, quiz);

				return RedirectToAction("Index");
			}
			else return HttpNotFound();			
		}

        #region Supporting method

        private async Task<Quiz> CreateQuizAsync(QuizViewModel model)
		{
			Quiz quiz = new Quiz();

			quiz.Name = model.Name;
			quiz.Description = model.Description;
			quiz.TimeDuration = new TimeSpan(model.Hours, model.Minutes, 0);
			quiz.ModifiedOn = DateTime.Now;
			quiz.IsActive = true;

			db.Quizzes.Add(quiz);
			await db.SaveChangesAsync();

			return quiz;
		}
		
		private async Task<Quiz> UpdateQuizAsync(QuizViewModel model, Quiz quiz)
		{
			quiz.Name = model.Name;
			quiz.Description = model.Description;
			quiz.TimeDuration = new TimeSpan(model.Hours, model.Minutes, 0);
			quiz.ModifiedOn = DateTime.Now;

			db.Entry(quiz).State = EntityState.Modified;
			await db.SaveChangesAsync();

			return quiz;
		}

		private async Task<Quiz> DeleteQuizAsync(QuizViewModel model, Quiz quiz)
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