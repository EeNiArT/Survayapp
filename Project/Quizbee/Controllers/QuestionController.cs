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
	public class QuestionController : Controller
    {
		ApplicationDbContext db;

		public QuestionController()
		{
			db = new ApplicationDbContext();
		}

        public ActionResult Index(int quizID, string search, int? page, int? items)
        {
			QuestionListingViewModel model = new QuestionListingViewModel();

			model.PageInfo = new PageInfo()
			{
				PageTitle = "Questions",
				PageDescription = "List of Questions for Selected Quiz."
			};

			model.searchTerm = search;
			model.pageNo = page ?? 1;
			model.pageSize = items ?? 10;

			var quiz = db.Quizzes.Where(q => q.IsActive).FirstOrDefault(x => x.ID == quizID);

			if (quiz != null)
			{
				model.QuizID = quizID;

				if (string.IsNullOrEmpty(model.searchTerm))
				{
					model.Questions = quiz.Questions
										.Where(q=>q.IsActive)
										.OrderByDescending(x => x.ModifiedOn)
										.Skip((model.pageNo - 1) * model.pageSize)
										.Take(model.pageSize)
										.ToList();

					model.TotalCount = quiz.Questions
										.Where(q => q.IsActive).Count();
				}
				else
				{
					model.Questions = quiz.Questions
										.Where(q => q.IsActive)
										.Where(x => x.Title.ToLower().StartsWith(model.searchTerm))
										.OrderByDescending(x => x.ModifiedOn)
										.Skip((model.pageNo - 1) * model.pageSize)
										.Take(model.pageSize)
										.ToList();

					model.TotalCount = quiz.Questions
										.Where(q => q.IsActive)
										.Where(x => x.Title.ToLower().StartsWith(model.searchTerm))
										.Count();
				}

				model.Pager = new Pager(model.TotalCount, model.pageNo, model.pageSize);
			}
			else return HttpNotFound();

			return View(model);
        }

		[HttpGet]
		public async Task<ActionResult> QuestionOperation(string Operation, int QuizID, int? ID)
		{
			QuestionViewModel model = new QuestionViewModel();

			if (Operation == Operations.Create)
			{
				var quiz = await db.Quizzes.FindAsync(QuizID);

				if (quiz == null || !quiz.IsActive) return HttpNotFound();

				model.PageInfo = new PageInfo()
				{
					PageTitle = "Add New Question",
					PageDescription = "Add questions to selected quiz."
				};

				model.QuizID = quiz.ID;

				return View(model);
			}
			else if (Operation == Operations.Modify)
			{
				if (!ID.HasValue) return HttpNotFound();

				var quiz = await db.Quizzes.FindAsync(QuizID);

				if (quiz == null || !quiz.IsActive) return HttpNotFound();

				var question = quiz.Questions.FirstOrDefault(q=>q.ID == ID.Value);
				
				if (question == null || !question.IsActive) return HttpNotFound();

				model.PageInfo = new PageInfo()
				{
					PageTitle = "Modify Question",
					PageDescription = "Modify selected question."
				};

				model.QuizID = QuizID;
				model.ID = question.ID;
				model.Title = question.Title;
				model.Options = question.Options.Where(o => o.IsActive).ToList();
				model.CorrectOption = model.Options.Find(q => q.IsCorrect);

				model.Options.Remove(model.CorrectOption);

				return View(model);
			}
			else return HttpNotFound();
		}

		[HttpPost]
		public async Task<ActionResult> QuestionOperation(string Operation, FormCollection collection)
		{
			QuestionViewModel model = GetQuestionViewModelFromFormCollection(collection);
			
			if (Operation == Operations.Create)
			{
				var quiz = await db.Quizzes.FindAsync(model.QuizID);

				if (quiz == null || !quiz.IsActive) return HttpNotFound();

				model.PageInfo = new PageInfo()
				{
					PageTitle = "Add New Question",
					PageDescription = "Add questions to selected quiz."
				};

				if (string.IsNullOrEmpty(model.Title) || model.CorrectOption == null || model.Options.Count == 0)
				{
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

				await CreateQuestionAsync(quiz, model);

				return RedirectToAction("QuestionOperation", new { Operation = Operations.Create, QuizID = model.QuizID });
			}
			else if (Operation == Operations.Update && model.ID > 0)
			{
				var quiz = await db.Quizzes.FindAsync(model.QuizID);

				if (quiz == null || !quiz.IsActive) return HttpNotFound();

				var question = await db.Questions.FindAsync(model.ID);

				if (question == null || !question.IsActive) return HttpNotFound();

				model.PageInfo = new PageInfo()
				{
					PageTitle = "Modify Question",
					PageDescription = "Modify selected question."
				};

				if (string.IsNullOrEmpty(model.Title) || model.CorrectOption == null || model.Options.Count == 0)
				{
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

				await UpdateQuestionAsync(quiz, model, question);

				return RedirectToAction("Index", new { quizID = question.QuizID });
			}
			else if (Operation == Operations.Delete && model.ID > 0)
			{
				var quiz = await db.Quizzes.FindAsync(model.QuizID);

				if (quiz == null || !quiz.IsActive) return HttpNotFound();

				var question = await db.Questions.FindAsync(model.ID);

				if (question == null || !question.IsActive) return HttpNotFound();

				await DeleteQuestionAsync(quiz, model, question);

				return RedirectToAction("Index", new { quizID = question.QuizID });
			}
			else return HttpNotFound();			
		}
		
		private async Task<Question> CreateQuestionAsync(Quiz quiz, QuestionViewModel model)
		{
			Question question = new Question();
			question.QuizID = quiz.ID;
			question.Title = model.Title;

			question.Options = new List<Option>();
			question.Options.Add(model.CorrectOption);
			question.Options.AddRange(model.Options);

			question.IsActive = true;
			question.ModifiedOn = DateTime.Now;

			db.Questions.Add(question);
			
			await db.SaveChangesAsync();

			return question;
		}

		private async Task<Question> UpdateQuestionAsync(Quiz quiz, QuestionViewModel model, Question question)
		{
			foreach (var option in question.Options.ToList())
			{
				option.IsActive = false;
				option.ModifiedOn = DateTime.Now;

				db.Entry(question).State = System.Data.Entity.EntityState.Modified;
			}

			question.QuizID = quiz.ID;
			question.Title = model.Title;

			question.Options = new List<Option>();
			question.Options.Add(model.CorrectOption);
			question.Options.AddRange(model.Options);
			
			question.ModifiedOn = DateTime.Now;

			db.Entry(question).State = System.Data.Entity.EntityState.Modified;

			await db.SaveChangesAsync();

			return question;
		}

		private async Task<Question> DeleteQuestionAsync(Quiz quiz, QuestionViewModel model, Question question)
		{
			foreach (var attemptedQuestion in db.AttemptedQuestions.Where(at=>at.Question.ID == question.ID).ToList())
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
					else if (key == "CorrectOption")
					{
						if (!string.IsNullOrEmpty(collection[key]))
						{
							model.CorrectOption = new Option()
							{
								Answer = collection[key],
								IsCorrect = true,
								IsActive = true
							};
						}
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
				}
			}

			return model;
		}
	}
}
