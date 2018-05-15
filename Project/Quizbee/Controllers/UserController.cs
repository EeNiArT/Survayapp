using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Quizbee.Models;
using Quizbee.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Quizbee.Controllers
{
	[Authorize]
	public class UserController : Controller
    {
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;
		
		public ApplicationSignInManager SignInManager
		{
			get
			{
				return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			}
			private set
			{
				_signInManager = value;
			}
		}

		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}

		public UserController()
		{

		}

		public async Task<ActionResult> Index()
		{
			ProfileViewModel pModel = new ProfileViewModel();

			pModel.PageInfo = new PageInfo()
			{
				PageTitle = "Profile",
				PageDescription = "My Profile."
			};

			pModel.User = await UserManager.FindByIdAsync(User.Identity.GetUserId());
			pModel.UserName = pModel.User.UserName;
			pModel.Email = pModel.User.Email;

			return View(pModel);
		}

		[AllowAnonymous]
		public ActionResult Register()
		{
			return View("Register", "_LayoutEmpty", new RegisterViewModel());
		}
		
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
				var result = await UserManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

					return RedirectToAction("Index", "Home");
				}
				AddErrors(result);
			}
			
			return View("Register", "_LayoutEmpty", model);
		}

		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;

			return View("Login", "_LayoutEmpty", new LoginViewModel());
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View("Login", "_LayoutEmpty", model);
			}

			var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
			switch (result)
			{
				case SignInStatus.Success:
					return RedirectToLocal(returnUrl);
				default:
					ModelState.AddModelError("", "Invalid login attempt.");
					return View("Login", "_LayoutEmpty", model);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			return RedirectToAction("Index", "Home");
		}

		public JsonResult UploadPhoto()
		{
			JsonResult result = new JsonResult();

			try
			{
				var file = Request.Files[0];

				var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
				var path = Path.Combine(Server.MapPath("~/Content/images/users/"), fileName);

				file.SaveAs(path);

				var ImageURL = string.Format("{0}", fileName);

				result.Data = new { Success = true, File = path, ImageURL = ImageURL };
			}
			catch (Exception ex)
			{
				result.Data = new { success = false, Message = ex.Message };
			}

			return result;
		}

		public string UserPhoto()
		{
			if (User.Identity.IsAuthenticated)
			{
				var user = UserManager.FindById(User.Identity.GetUserId());

				return user.Photo;
			}
			else return null;
		}

		[OutputCache(Duration = 3600, VaryByCustom = "User", Location = OutputCacheLocation.Client, NoStore = true)]
		public ActionResult MyPhoto()
		{
			if (User.Identity.IsAuthenticated)
			{
				var user = UserManager.FindById(User.Identity.GetUserId());

				var imagePath = string.Empty;

				if(string.IsNullOrEmpty(user.Photo))
				{
					imagePath = Path.Combine(Server.MapPath("~/Content/images/users/"), "user-default-avatar.png");
				}
				else imagePath = Path.Combine(Server.MapPath("~/Content/images/users/"), user.Photo);
				
				return new FileStreamResult(new FileStream(imagePath, FileMode.Open), "image/jpeg");
			}
			else return null;
		}

		[HttpPost]
		public async Task<JsonResult> UpdateInfo(ProfileViewModel pModel)
		{
			JsonResult result = new JsonResult();
			result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

			if (!ModelState.IsValid)
			{
				var Errors = new List<string>();

				foreach (ModelState modelState in ViewData.ModelState.Values)
				{
					foreach (ModelError error in modelState.Errors)
					{
						Errors.Add(error.ErrorMessage.ToString());
					}
				}

				result.Data = new { Success = false, Errors = Errors };

				return result;
			}

			var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

			if (!string.IsNullOrEmpty(pModel.Photo))
			{
				user.Photo = pModel.Photo;
			}

			user.UserName = pModel.UserName;
			user.Email = pModel.Email;

			var updateResult = await UserManager.UpdateAsync(user);

			if (updateResult.Succeeded)
			{
				result.Data = new { Success = true };
			}
			else
			{
				result.Data = new { Success = false, Errors = updateResult.Errors };
			}

			return result;
		}

		[HttpPost]
		public async Task<JsonResult> UpdatePassword(ChangePasswordViewModel model)
		{
			JsonResult result = new JsonResult();
			result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

			if (!ModelState.IsValid)
			{
				var Errors = new List<string>();

				foreach (ModelState modelState in ViewData.ModelState.Values)
				{
					foreach (ModelError error in modelState.Errors)
					{
						Errors.Add(error.ErrorMessage.ToString());
					}
				}

				result.Data = new { Success = false, Errors = Errors };

				return result;
			}

			var updationResult = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
			if (updationResult.Succeeded)
			{
				var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
				if (user != null)
				{
					await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
				}

				result.Data = new { Success = true };
			}
			else
			{
				result.Data = new { Success = false, Errors = updationResult.Errors };
			}

			return result;
		}

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			return RedirectToAction("Index", "Home");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && _userManager != null)
			{
				_userManager.Dispose();
				_userManager = null;
			}

			base.Dispose(disposing);
		}
	}
}