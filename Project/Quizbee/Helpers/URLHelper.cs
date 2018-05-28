using Quizbee.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Quizbee.Helpers
{
    public static class URLHelper
    {
        public static string Home(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("Home");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string Login(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("Login", new
            {
                controller = "User",
                action = "Login"
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string Register(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("Register", new
            {
                controller = "User",
                action = "Register"
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string Logout(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("Logout", new
            {
                controller = "User",
                action = "LogOff"
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string Me(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("Me", new
            {
                controller = "User",
                action = "Index"
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string UploadPhoto(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("UploadPhoto", new
            {
                controller = "User",
                action = "UploadPhoto"
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string MyPhoto(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("MyPhoto", new
            {
                controller = "User",
                action = "MyPhoto"
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string UpdateInfo(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("UpdateInfo", new
            {
                controller = "User",
                action = "UpdateInfo"
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string UpdatePassword(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("UpdatePassword", new
            {
                controller = "User",
                action = "UpdatePassword"
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string QuizzesList(this UrlHelper helper, string searchTerm = "", int? pageNo = 1, int? pageSize = 10)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("Quizzes", new
            {
                controller = "Quiz",
                action = "Index",
                search = searchTerm,
                page = pageNo.Value,
                items = pageSize.Value
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string CreateQuiz(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("QuizOperation", new
            {
                controller = "Quiz",
                action = "QuizOperation",
                Operation = Operations.Create
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string ModifyQuiz(this UrlHelper helper, int quizID)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("QuizOperation", new
            {
                controller = "Quiz",
                action = "QuizOperation",
                Operation = Operations.Modify,
                ID = quizID
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string UpdateQuiz(this UrlHelper helper, int quizID)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("QuizOperation", new
            {
                controller = "Quiz",
                action = "QuizOperation",
                Operation = Operations.Update,
                ID = quizID
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string DeleteQuiz(this UrlHelper helper, int quizID)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("QuizOperation", new
            {
                controller = "Quiz",
                action = "QuizOperation",
                Operation = Operations.Delete,
                ID = quizID
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string CreateSurvey(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("SurveyOperation", new
            {
                controller = "Survey",
                action = "SurveyOperation",
                Operation = Operations.Create
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string ModifySurvey(this UrlHelper helper, int surveyID)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("SurveyOperation", new
            {
                controller = "Survey",
                action = "SurveyOperation",
                Operation = Operations.Modify,
                ID = surveyID
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string UpdateSurvey(this UrlHelper helper, int surveyID)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("SurveyOperation", new
            {
                controller = "Survey",
                action = "SurveyOperation",
                Operation = Operations.Update,
                ID = surveyID
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string DeleteSurvey(this UrlHelper helper, int surveyID)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("SurveyOperation", new
            {
                controller = "Survey",
                action = "SurveyOperation",
                Operation = Operations.Delete,
                ID = surveyID
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string QuestionsList(this UrlHelper helper, int quizID, string searchTerm = "", int? pageNo = 1, int? pageSize = 10)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("QuestionsList", new
            {
                controller = "Question",
                action = "Index",
                quizID = quizID,
                search = searchTerm,
                page = pageNo.Value,
                items = pageSize.Value
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string AddQuestion(this UrlHelper helper, int quizID)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("QuestionOperation", new
            {
                controller = "Question",
                action = "QuestionOperation",
                Operation = Operations.Create,
                QuizID = quizID
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string ModifyQuestion(this UrlHelper helper, int quizID, int questionID)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("QuestionOperation", new
            {
                controller = "Question",
                action = "QuestionOperation",
                Operation = Operations.Modify,
                QuizID = quizID,
                ID = questionID
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string UpdateQuestion(this UrlHelper helper, int quizID, int questionID)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("QuestionOperation", new
            {
                controller = "Question",
                action = "QuestionOperation",
                Operation = Operations.Update,
                QuizID = quizID,
                ID = questionID
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string DeleteQuestion(this UrlHelper helper, int quizID, int questionID)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("QuestionOperation", new
            {
                controller = "Question",
                action = "QuestionOperation",
                Operation = Operations.Delete,
                QuizID = quizID,
                ID = questionID
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string AttemptQuiz(this UrlHelper helper, int quizID)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("AttemptQuiz", new
            {
                controller = "AttemptQuiz",
                action = "Attempt",
                QuizID = quizID
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string AnswerQuestion(this UrlHelper helper, int quizID)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("AnswerQuestion", new
            {
                controller = "AttemptQuiz",
                action = "AnswerQuestion"
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string AttemptDetails(this UrlHelper helper, int attemptID)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("AttemptDetails", new
            {
                controller = "AttemptQuiz",
                action = "AttemptDetails",
                attemptID = attemptID
            });

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string MyResults(this UrlHelper helper, bool? isPartial = false, string searchTerm = "", int? pageNo = 1, int? pageSize = 10)
        {
            string routeURL = string.Empty;

            if (!isPartial.HasValue || !isPartial.Value)
            {
                routeURL = helper.RouteUrl("MyResults", new
                {
                    controller = "AttemptQuiz",
                    action = "MyResults",
                    search = searchTerm,
                    page = pageNo.Value,
                    items = pageSize.Value
                });
            }
            else
            {
                routeURL = helper.RouteUrl("MyResults", new
                {
                    controller = "AttemptQuiz",
                    action = "MyResults",
                    isPartial = isPartial,
                    search = searchTerm,
                    page = pageNo.Value,
                    items = pageSize.Value
                });
            }

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

        public static string Survey(this UrlHelper helper)
        {
            string routeURL = string.Empty;

            routeURL = helper.RouteUrl("Survey");

            routeURL = HttpUtility.UrlDecode(routeURL, System.Text.Encoding.UTF8);
            return routeURL.ToLower();
        }

    }
}