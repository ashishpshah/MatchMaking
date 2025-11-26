using MatchMaking.Infra;
using MatchMaking.Infra.Services;
using MatchMaking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Globalization;

namespace MatchMaking.Controllers
{
	public class HomeController : BaseController<ResponseModel<LoginViewModel>>
	{
		public HomeController(IRepositoryWrapper repository) : base(repository) { }

		public IActionResult Index()
		{
			//if (Common.LoggedUser_Id() <= 0)
			//	return RedirectToAction("Account", "Home", new { Area = "Admin" });

			var list = _context.Using<User>().GetAll().ToList();

			var (IsSuccess, Message, Id) = (false, "", (long)0);

			if (list == null || list.Count == 0)
			{
				Common.Set_Session_Int(SessionKey.KEY_USER_ID, 1);

				var user = new User() { UserName = "Adnin", Password = Common.Encrypt("admin"), CreatedBy = 1 };
				user = _context.Using<User>().Add(user);

				var role = new Role() { Name = "Super Admin", IsAdmin = true, CreatedBy = 1 };
				role = _context.Using<Role>().Add(role);

				var userRole = new UserRoleMapping() { UserId = user.Id, RoleId = role.Id, CreatedBy = 1 };
				_context.Using<UserRoleMapping>().Add(userRole);

				user = new User() { UserName = "Admin", Password = Common.Encrypt("admin"), CreatedBy = 1 };
				user = _context.Using<User>().Add(user);

				role = new Role() { Name = "Admin", IsAdmin = true, CreatedBy = 1 };
				role = _context.Using<Role>().Add(role);

				userRole = new UserRoleMapping() { UserId = user.Id, RoleId = role.Id, CreatedBy = 1 };
				_context.Using<UserRoleMapping>().Add(userRole);

				var menu = new Menu() { ParentId = 0, Area = "", Controller = "", Name = "Configuration", IsSuperAdmin = false, IsAdmin = true, DisplayOrder = 1, CreatedBy = 1 };
				menu = _context.Using<Menu>().Add(menu);

				var userMenuAccess = new UserMenuAccess() { UserId = user.Id, RoleId = role.Id, MenuId = menu.Id, IsCreate = true, IsUpdate = true, IsRead = true, IsDelete = true, CreatedBy = 1 };
				_context.Using<UserMenuAccess>().Add(userMenuAccess);

				List<Menu> listMenu_Child = new List<Menu>();

				listMenu_Child.Add(new Menu() { ParentId = menu.Id, Area = "Admin", Controller = "User", Name = "User", IsSuperAdmin = false, IsAdmin = true, DisplayOrder = 2, CreatedBy = 1 });
				listMenu_Child.Add(new Menu() { ParentId = menu.Id, Area = "Admin", Controller = "Role", Name = "Role", IsSuperAdmin = false, IsAdmin = true, DisplayOrder = 3, CreatedBy = 1 });
				listMenu_Child.Add(new Menu() { ParentId = menu.Id, Area = "Admin", Controller = "Access", Name = "User Access", IsSuperAdmin = false, IsAdmin = true, DisplayOrder = 4, CreatedBy = 1 });
				listMenu_Child.Add(new Menu() { ParentId = menu.Id, Area = "Admin", Controller = "Menu", Name = "Menu", IsSuperAdmin = true, IsAdmin = false, DisplayOrder = 5, CreatedBy = 1 });

				for (int i = 0; i < listMenu_Child.Count; i++) listMenu_Child[i] = _context.Using<Menu>().Add(listMenu_Child[i]);

				foreach (var item in listMenu_Child.OrderBy(x => x.ParentId).ThenBy(x => x.Id).ToList())
				{
					var roleMenuAccess = new RoleMenuAccess() { RoleId = role.Id, MenuId = item.Id, IsCreate = true, IsUpdate = true, IsRead = true, IsDelete = true, CreatedBy = 1 };
					_context.Using<RoleMenuAccess>().Add(roleMenuAccess);
				}

				foreach (var item in listMenu_Child.OrderBy(x => x.ParentId).ThenBy(x => x.Id).ToList())
				{
					userMenuAccess = new UserMenuAccess() { UserId = user.Id, RoleId = role.Id, MenuId = item.Id, IsCreate = true, IsUpdate = true, IsRead = true, IsDelete = true, CreatedBy = 1 };
					_context.Using<UserMenuAccess>().Add(userMenuAccess);
				}
			}

			return View();
		}

		public IActionResult Member()
		{
			return View();
		}

		public IActionResult Community()
		{
			return View();
		}

		public IActionResult Contact()
		{
			return View();
		}

		public IActionResult Profile()
		{
			var profile = _context.Using<Profile>().GetByCondition(x => x.UserId == AppHttpContextAccessor.LoggedUserId).FirstOrDefault();

			return View(profile ?? new Profile());
		}

		public IActionResult ProfileUpdate()
		{
			var profile = _context.Using<Profile>().GetByCondition(x => x.UserId == AppHttpContextAccessor.LoggedUserId).FirstOrDefault();

			return View(profile ?? new Profile());
		}

		[HttpPost]
		public IActionResult Login(RegisterViewModel viewModel)
		{
			try
			{
				if (viewModel != null)
				{
					if (string.IsNullOrEmpty(viewModel.Username))
					{
						CommonViewModel.Message = "Please enter Email.";
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;

						return Json(CommonViewModel);
					}

					if (string.IsNullOrEmpty(viewModel.Password))
					{
						CommonViewModel.Message = "Please enter Password.";
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;

						return Json(CommonViewModel);
					}

					if (!_context.Using<User>().Any(x => x.UserName.ToLower() == Convert.ToString((viewModel.Username)).ToLower()))
					{
						CommonViewModel.Message = "Username not already exist";
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;

						return Json(CommonViewModel);
					}

					if (!_context.Using<User>().Any(x => x.UserName.ToLower() == Convert.ToString(viewModel.Username)
						&& x.Password == Common.Encrypt(Convert.ToString((viewModel.Password)))))
					{
						CommonViewModel.Message = "Invalid Password";
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;

						return Json(CommonViewModel);
					}

					var user = _context.Using<User>().GetByCondition(x => x.UserName.ToLower() == Convert.ToString(viewModel.Username)
						&& x.Password == Common.Encrypt(Convert.ToString((viewModel.Password)))).FirstOrDefault();

					if (user != null)
					{
						//var userDtls = _context.Using<Profile>().GetByCondition(x => x.UserId == user.Id).FirstOrDefault();

						//if (userDtls != null)
						//{
						//	Common.Set_Session("UserFullName", string.Format("{0}", Convert.ToString(userDtls.FirstName), Convert.ToString(userDtls.LastName)));
						//}
						//else
						//{
						//	Common.Set_Session("UserFullName", Convert.ToString(user.UserName));
						//}

						Common.Set_Session_Int(SessionKey.KEY_USER_ID, Convert.ToInt32(user.Id));

						CommonViewModel.IsConfirm = false;
						CommonViewModel.IsSuccess = true;
						CommonViewModel.StatusCode = ResponseStatusCode.Success;
						CommonViewModel.Message = "Login successful ! ";
						CommonViewModel.RedirectURL = Url.Action("Profile", "Home", new { area = "" });
						return Json(CommonViewModel);
					}
					else
					{
						CommonViewModel.Message = "Invalid Username and Password";
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;

						return Json(CommonViewModel);
					}

				}
			}
			catch (Exception ex) { }

			CommonViewModel.Message = ResponseStatusMessage.Error;
			CommonViewModel.IsSuccess = false;
			CommonViewModel.StatusCode = ResponseStatusCode.Error;

			return Json(CommonViewModel);
		}

		[HttpPost]
		public IActionResult Register(RegisterViewModel viewModel)
		{
			try
			{
				if (viewModel != null)
				{
					#region Validation

					if (string.IsNullOrEmpty(viewModel.Username))
					{
						CommonViewModel.Message = "Please enter Email.";
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;

						return Json(CommonViewModel);
					}

					if (!string.IsNullOrEmpty(viewModel.Username) && !ValidateField.IsValidEmail(viewModel.Username))
					{
						CommonViewModel.Message = "Please enter valid Email.";
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;

						return Json(CommonViewModel);
					}

					if (string.IsNullOrEmpty(viewModel.Password))
					{
						CommonViewModel.Message = "Please enter Password.";
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;

						return Json(CommonViewModel);
					}

					if (string.IsNullOrEmpty(viewModel.ConfirmPassword))
					{
						CommonViewModel.Message = "Please enter Confirm Password.";
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;

						return Json(CommonViewModel);
					}

					if (viewModel.Password != viewModel.ConfirmPassword)
					{
						CommonViewModel.Message = "Password and Confirm Password do not match.";
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;

						return Json(CommonViewModel);
					}

					if (_context.Using<User>().Any(x => x.UserName.ToLower() == Convert.ToString((viewModel.Username)).ToLower()))
					{
						CommonViewModel.Message = "Username already exist. Please try another Username.";
						CommonViewModel.IsSuccess = false;
						CommonViewModel.StatusCode = ResponseStatusCode.Error;

						return Json(CommonViewModel);
					}

					#endregion


					#region Database-Transaction

					using (var transaction = _context.BeginTransaction())
					{
						try
						{
							var user = new User()
							{
								UserName = Convert.ToString((viewModel.Username)),
								Password = Common.Encrypt(Convert.ToString((viewModel.Password))),
								NoOfWrongPasswordAttempts = 5,
								NextChangePasswordDate = DateTime.Now,
								CreatedBy = 1
							};

							user = _context.Using<User>().Add(user);

							var role = _context.Using<Role>().GetByCondition(x => x.Name == "Candidate").FirstOrDefault();

							if (role == null)
							{
								role = new Role() { Name = "Candidate", IsAdmin = true, CreatedBy = 1 };
								role = _context.Using<Role>().Add(role);
							}

							if (role != null)
							{
								var userRole = new UserRoleMapping() { UserId = user.Id, RoleId = role.Id, CreatedBy = 1 };
								_context.Using<UserRoleMapping>().Add(userRole);
							}

							CommonViewModel.IsConfirm = true;
							CommonViewModel.IsSuccess = true;
							CommonViewModel.StatusCode = ResponseStatusCode.Success;
							CommonViewModel.Message = "Record saved successfully ! ";

							CommonViewModel.RedirectURL = Url.Action("Profile", "Home", new { area = "" });

							transaction.Commit();

							return Json(CommonViewModel);
						}
						catch (Exception ex) { transaction.Rollback(); }
					}

					#endregion
				}
			}
			catch (Exception ex) { }

			CommonViewModel.Message = ResponseStatusMessage.Error;
			CommonViewModel.IsSuccess = false;
			CommonViewModel.StatusCode = ResponseStatusCode.Error;

			return Json(CommonViewModel);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
