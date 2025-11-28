using MatchMaking.Infra;
using MatchMaking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MatchMaking.Controllers
{
	public class BaseController<T> : Controller where T : class
	{
		public T CommonViewModel { get; set; } = default(T);

		public bool IsLogActive = false;

		public readonly DateTime? nullDateTime = null;
		public string ControllerName = "";
		public string ActionName = "";
		public string AreaName = "";

		public long Logged_In_UserId { get; set; }

		public IRepositoryWrapper _context;

		public BaseController(IRepositoryWrapper repository)
		{
			try { Logged_In_UserId = Common.Get_Session_Int(SessionKey.KEY_USER_ID); } catch { }

			_context = repository;
			CommonViewModel = (dynamic)Activator.CreateInstance(typeof(T));
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			try
			{
				ControllerName = Convert.ToString(context.RouteData.Values["controller"]);
				ActionName = Convert.ToString(context.RouteData.Values["action"]);

				if (context.RouteData.DataTokens != null) AreaName = Convert.ToString(context.RouteData.DataTokens["area"]);

				if (string.IsNullOrEmpty(AreaName)) AreaName = Convert.ToString(context.RouteData.Values["area"]);

				if (!string.IsNullOrEmpty(AreaName))
				{
					List<UserMenuAccess> listMenuAccess = Common.GetUserMenuPermission();

					if (listMenuAccess != null && listMenuAccess.Count > 0)
					{
						if (listMenuAccess.FindIndex(x => x.Controller == ControllerName) > -1)
						{
							//CommonViewModel.IsCreate = listMenuAccess[listMenuAccess.FindIndex(x => x.Controller == ControllerName)].IsCreate;
							//CommonViewModel.IsRead = listMenuAccess[listMenuAccess.FindIndex(x => x.Controller == ControllerName)].IsRead;
							//CommonViewModel.IsUpdate = listMenuAccess[listMenuAccess.FindIndex(x => x.Controller == ControllerName)].IsUpdate;
							//CommonViewModel.IsDelete = listMenuAccess[listMenuAccess.FindIndex(x => x.Controller == ControllerName)].IsDelete;

							try { Common.Set_Session_Int(SessionKey.CURRENT_MENU_ID, Convert.ToInt32(listMenuAccess[listMenuAccess.FindIndex(x => x.Controller == ControllerName)].MenuId)); }
							catch { Common.Set_Session_Int(SessionKey.CURRENT_MENU_ID, 0); }
						}
					}

					if (!Common.IsUserLogged() && !string.IsNullOrEmpty(AreaName) && !(Convert.ToString(ControllerName).ToLower() == "home" && (new string[] { "account", "login", "forgotpassword", "forgotpassword_sendotp", "forgotpassword_verifyotp", "forgotpassword_resetpassword" }).Any(x => x == Convert.ToString(ActionName).ToLower())))
					{
						context.Result = new RedirectResult(Url.Content("~/") + (string.IsNullOrEmpty(AreaName) ? "" : AreaName + "/") + "Home/Account");
						return;
					}
					else if (!Common.IsUserLogged() && !Common.IsAdmin() && !string.IsNullOrEmpty(AreaName) && !(Convert.ToString(ControllerName).ToLower() == "home" && (new string[] { "account", "login", "forgotpassword", "forgotpassword_sendotp", "forgotpassword_verifyotp", "forgotpassword_resetpassword" }).Any(x => x == Convert.ToString(ActionName).ToLower())))
					{
						context.Result = new RedirectResult(Url.Content("~/") + "Home/Login");
						return;
					}
				}
			}
			catch (Exception ex) { LogService.LogInsert(GetCurrentAction(), "", ex); }
		}


		public string GetCurrentAction() => (string.IsNullOrEmpty(AreaName) ? "" : AreaName + " - ") + ControllerName + " - " + ActionName;
		public string GetCurrentControllerUrl() => (string.IsNullOrEmpty(AreaName) ? "" : AreaName + "/") + ControllerName;
	}
}
