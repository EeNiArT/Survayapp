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
    public class AttemptQuizController : Controller
	{
		ApplicationDbContext db;

		public AttemptQuizController()
		{
			db = new ApplicationDbContext();
		}
		
		[HttpGet]
        public async Task<ActionResult> Attempt(int QuizID)
        {
			var quiz = await db.Quizzes.FindAsync(QuizID);

			if (quiz == null || !quiz.IsActive) return HttpNotFound();

			QuizViewModel model = new QuizViewModel();

			model.PageInfo = new PageInfo()
			{
				PageTitle = string.Format("Quiz : {0}", quiz.Name),
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
		public async Task<ActionResult> Attempt(AttemptQuizViewModel model)
		{
			var quiz = await db.Quizzes.FindAsync(model.QuizID);

			if (quiz == null || !quiz.IsActive) return HttpNotFound();

			if (quiz.Questions.Count == 0) return HttpNotFound();

			StudentQuiz studentQuiz = new StudentQuiz();
			
			studentQuiz.StudentID = User.Identity.GetUserId();
			studentQuiz.Quiz = quiz;
			studentQuiz.StartedAt = DateTime.Now;
			studentQuiz.IsActive = true;
			studentQuiz.ModifiedOn = DateTime.Now;

			db.StudentQuizzes.Add(studentQuiz);
			await db.SaveChangesAsync();
			
			model.StudentQuizID = studentQuiz.ID;
			model.TotalQuestions = quiz.Questions.Count;
			model.Question = quiz.Questions.Where(q=>q.IsActive).FirstOrDefault();
			model.QuestionIndex = 0;

			model.Options = new List<Option>();
			model.Options.AddRange(model.Question.Options.Where(q=>q.IsActive));
			model.Options.Shuffle();
			
			return PartialView("QuizQuestion", model);
		}

		[HttpPost]
		public async Task<ActionResult> AnswerQuestion(AttemptQuizViewModel model)
		{
			if (model.TimerExpired)
			{
				var studentQuiz = await db.StudentQuizzes.FindAsync(model.StudentQuizID);

				studentQuiz.CompletedAt = DateTime.Now;
				studentQuiz.ModifiedOn = DateTime.Now;

				db.Entry(studentQuiz).State = System.Data.Entity.EntityState.Modified;
				await db.SaveChangesAsync();

				StudentQuizViewModel studentQuizModel = new StudentQuizViewModel();

				studentQuizModel.StudentQuiz = studentQuiz;
				studentQuizModel.TimerExpired = model.TimerExpired;

				return PartialView("AttemptDetailsPartial", studentQuizModel);
			}
			else
			{
				var quiz = await db.Quizzes.FindAsync(model.QuizID);

				if (quiz == null || !quiz.IsActive) return HttpNotFound();

				var question = quiz.Questions.Find(q => q.ID == model.QuestionID);

				if (question == null || !question.IsActive) return HttpNotFound();
				
				var selectedOption = question.Options.Find(o => o.ID == model.SelectedOptionID);

				if (selectedOption == null || !selectedOption.IsActive) return HttpNotFound();

				AttemptedQuestion attemptedQuestion = new AttemptedQuestion();

				attemptedQuestion.Question = question;
				attemptedQuestion.SelectedOption = selectedOption;
				attemptedQuestion.AnsweredAt = DateTime.Now;

				if (selectedOption.IsCorrect)
					attemptedQuestion.IsCorrect = true;

				var studentQuiz = await db.StudentQuizzes.FindAsync(model.StudentQuizID);

				if (studentQuiz.AttemptedQuestions == null)
					studentQuiz.AttemptedQuestions = new List<AttemptedQuestion>();

				attemptedQuestion.IsActive = true;
				attemptedQuestion.ModifiedOn = DateTime.Now;

				studentQuiz.AttemptedQuestions.Add(attemptedQuestion);

				if (model.QuestionIndex == quiz.Questions.Count - 1) //this was the Last question
				{
					studentQuiz.CompletedAt = DateTime.Now;
				}
				
				db.Entry(studentQuiz).State = System.Data.Entity.EntityState.Modified;
				await db.SaveChangesAsync();

				if (model.QuestionIndex != quiz.Questions.Where(q => q.IsActive).Count() - 1)
				{
					model.TotalQuestions = quiz.Questions.Count;
					model.Question = quiz.Questions.Where(q=>q.IsActive).ElementAtOrDefault(model.QuestionIndex + 1);
					model.QuestionIndex = model.QuestionIndex + 1;

					model.Options = new List<Option>();
					model.Options.AddRange(model.Question.Options.Where(o=>o.IsActive));
					model.Options.Shuffle();

					return PartialView("QuizQuestion", model);
				}
				else //this was the Last question so display the result
				{
					StudentQuizViewModel studentQuizModel = new StudentQuizViewModel();

					studentQuizModel.StudentQuiz = studentQuiz;
					studentQuizModel.TimerExpired = model.TimerExpired;

					return PartialView("AttemptDetailsPartial", studentQuizModel);
				}
			}
		}

		[HttpGet]
		public async Task<ActionResult> AttemptDetails(int attemptID)
		{
			var studentQuiz = await db.StudentQuizzes.FindAsync(attemptID);

			if (studentQuiz == null || !studentQuiz.IsActive) return HttpNotFound();

			StudentQuizViewModel model = new StudentQuizViewModel();

			model.PageInfo = new PageInfo()
			{
				PageTitle = "Quiz Attempt Details",
				PageDescription = "Details of Attempted Quiz"
			};

			model.StudentQuiz = studentQuiz;

			return View(model);
		}

		[HttpGet]
		public ActionResult MyResults(bool? isPartial, string search, int? page, int? items)
		{
			var UserId = User.Identity.GetUserId();
			
			StudentQuizListViewModel model = new StudentQuizListViewModel();

			model.PageInfo = new PageInfo()
			{
				PageTitle = "My Quiz Results",
				PageDescription = "Results of My Quiz Attempts"
			};

			model.searchTerm = search;
			model.pageNo = page ?? 1;
			model.pageSize = items ?? 10;
			
			if (string.IsNullOrEmpty(model.searchTerm))
			{
				model.StudentQuizzes = db.StudentQuizzes
									.Where(q=>q.IsActive)
									.Where(x => x.StudentID == UserId)
									.OrderByDescending(x => x.ModifiedOn)
									.Skip((model.pageNo - 1) * model.pageSize)
									.Take(model.pageSize)
									.ToList();

				model.TotalCount = db.StudentQuizzes
									.Where(q => q.IsActive)
									.Where(x => x.StudentID == UserId)
									.Count();
			}
			else
			{
				model.StudentQuizzes = db.StudentQuizzes
									.Where(q => q.IsActive)
									.Where(x=>x.StudentID == UserId)
									.Where(x => x.Quiz.Name.ToLower().StartsWith(model.searchTerm))
									.OrderByDescending(x => x.ModifiedOn)
									.Skip((model.pageNo - 1) * model.pageSize)
									.Take(model.pageSize)
									.ToList();

				model.TotalCount = db.StudentQuizzes
									.Where(q => q.IsActive)
									.Where(x => x.StudentID == UserId)
									.Where(x => x.Quiz.Name.ToLower().StartsWith(model.searchTerm))
									.Count();
			}

			model.Pager = new Pager(model.TotalCount, model.pageNo, model.pageSize);

			if(isPartial.HasValue && isPartial.Value)
			{
				model.isPartialView = true;
				return PartialView(model);
			}
			else return View(model);
		}
	}
}