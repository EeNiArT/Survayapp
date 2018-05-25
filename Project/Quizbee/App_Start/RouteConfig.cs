using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Quizbee
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Home",
				url: "",
				defaults: new { controller = "Home", action = "Index" }
			);

			routes.MapRoute(
				name: "Register",
				url: "register/",
				defaults: new { controller = "User", action = "Register" }
			);

			routes.MapRoute(
				name: "Login",
				url: "login/",
				defaults: new { controller = "User", action = "Login" }
			);
			
			routes.MapRoute(
				name: "Logout",
				url: "logout/",
				defaults: new { controller = "User", action = "LogOff" }
			);
			
			routes.MapRoute(
				name: "Me",
				url: "me/",
				defaults: new { controller = "User", action = "Index" }
			);

			routes.MapRoute(
				name: "UploadPhoto",
				url: "me/upload-photo/",
				defaults: new { controller = "User", action = "UploadPhoto" }
			);

			routes.MapRoute(
				name: "MyPhoto",
				url: "me/photo/",
				defaults: new { controller = "User", action = "MyPhoto" }
			);

			routes.MapRoute(
				name: "UpdateInfo",
				url: "me/update-info/",
				defaults: new { controller = "User", action = "UpdateInfo" }
			);

			routes.MapRoute(
				name: "UpdatePassword",
				url: "me/update-password/",
				defaults: new { controller = "User", action = "UpdatePassword" }
			);

			routes.MapRoute(
				name: "Quizzes",
				url: "quizzes/",
				defaults: new { controller = "Quiz", action = "Index"}
			);

			routes.MapRoute(
				name: "QuizOperation",
				url: "quizzes/{Operation}/",
				defaults: new { controller = "Quiz", action = "QuizOperation" }
			);

			routes.MapRoute(
				name: "QuestionsList",
				url: "questions/{quizID}/",
				defaults: new { controller = "Question", action = "Index" }
			);

			routes.MapRoute(
				name: "QuestionOperation",
				url: "questions/{QuizID}/{Operation}/",
				defaults: new { controller = "Question", action = "QuestionOperation" }
			);

			routes.MapRoute(
				name: "AttemptQuiz",
				url: "attempt-quiz/{QuizID}",
				defaults: new { controller = "AttemptQuiz", action = "Attempt" }
			);

			routes.MapRoute(
				name: "AnswerQuestion",
				url: "answer-question",
				defaults: new { controller = "AttemptQuiz", action = "AnswerQuestion" }
			);
			
			routes.MapRoute(
				name: "AttemptDetails",
				url: "quiz-result/{attemptID}",
				defaults: new { controller = "AttemptQuiz", action = "AttemptDetails" }
			);
			
			routes.MapRoute(
				name: "MyResults",
				url: "me/results",
				defaults: new { controller = "AttemptQuiz", action = "MyResults" }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}/",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

            routes.MapRoute(
                name: "Survey",
                url: "survey/",
                defaults: new { controller = "Survey", action = "Index" }
            );
        }
	}
}
