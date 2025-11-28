using MatchMaking.Areas.Admin.Models;
using MatchMaking.Infra;
using MatchMaking.Infra.Services;
using MatchMaking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Globalization;

namespace MatchMaking.Controllers
{
	public class HomeController : BaseController<ResponseModel<LoginViewModel>>
	{
		public HomeController(IRepositoryWrapper repository) : base(repository) { }

		public IActionResult Index()
		{
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
			if (AppHttpContextAccessor.LoggedUserId <= 0)
				return RedirectToAction("Index", "Home", new { Area = "" });

			var profile = _context.Using<Profile>().GetByCondition(x => x.UserId == AppHttpContextAccessor.LoggedUserId).FirstOrDefault();

			var jainGroups = _context.Using<JainGroup>().GetAll().ToList();

			List<Lov> SelectListItems = _context.Using<Lov>().GetAll().ToList();

			if (profile != null)
			{
				profile.GroupName = jainGroups.Where(x => x.Id == profile.GroupId).Select(x => x.Name).FirstOrDefault();
				profile.Gender = SelectListItems.Where(x => x.LovColumn == "Gender" && x.LovCode == profile.Gender).Select(x => x.LovDesc).FirstOrDefault();
				profile.LookingForGender = SelectListItems.Where(x => x.LovColumn == "Gender" && x.LovCode == profile.LookingForGender).Select(x => x.LovDesc).FirstOrDefault();
				profile.MaritalStatus = SelectListItems.Where(x => x.LovColumn == "MaritalStatus" && x.LovCode == profile.MaritalStatus).Select(x => x.LovDesc).FirstOrDefault();
				profile.Education = SelectListItems.Where(x => x.LovColumn == "Education" && x.LovCode == profile.Education).Select(x => x.LovDesc).FirstOrDefault();
				profile.Occupation = SelectListItems.Where(x => x.LovColumn == "Occupation" && x.LovCode == profile.Occupation).Select(x => x.LovDesc).FirstOrDefault();
				profile.Smoking = SelectListItems.Where(x => x.LovColumn == "Smoking" && x.LovCode == profile.Smoking).Select(x => x.LovDesc).FirstOrDefault();
				profile.Diet = SelectListItems.Where(x => x.LovColumn == "Diet" && x.LovCode == profile.Diet).Select(x => x.LovDesc).FirstOrDefault();
				profile.Interests = string.Join(", ", SelectListItems.Where(x => x.LovColumn == "Interest" && profile.Interests.Split(",").Contains(x.LovCode)).Select(x => x.LovDesc).ToArray());
				profile.Language = string.Join(", ", SelectListItems.Where(x => x.LovColumn == "Language" && profile.Language.Split(",").Contains(x.LovCode)).Select(x => x.LovDesc).ToArray());
			}

			return View(profile ?? new Profile());
		}

		public IActionResult ProfileUpdate()
		{
			var profile = _context.Using<Profile>().GetByCondition(x => x.UserId == AppHttpContextAccessor.LoggedUserId).FirstOrDefault();

			List<SelectListItem_Custom> SelectListItems = _context.Using<Lov>().GetAll().OrderBy(x => x.LovColumn).ThenBy(x => x.DisplayOrder).ToList()
				.Select(x => new SelectListItem_Custom(x.LovCode, x.LovDesc, x.LovColumn)).ToList();

			var jainGroups = _context.Using<JainGroup>().GetByCondition(x => x.IsActive == true).ToList()
				.Select(x => new SelectListItem_Custom(x.Id.ToString(), x.Name, "JainGroup")).ToList();

			if (SelectListItems != null) SelectListItems.AddRange(jainGroups);

			return View((profile ?? new Profile(), SelectListItems));
		}

		[HttpPost]
		public IActionResult Profile(Profile viewModel)
		{
			try
			{
				if (viewModel != null)
				{
					#region Validation

					if (string.IsNullOrEmpty(viewModel.Firstname))
					{
						CommonViewModel.Message = "Please enter your name.";
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
							var profile = _context.Using<Profile>().GetByCondition(x => x.UserId == AppHttpContextAccessor.LoggedUserId).FirstOrDefault();

							if (profile != null)
							{
								profile.Firstname = viewModel.Firstname;
								profile.Fathername = viewModel.Fathername;
								profile.Mothername = viewModel.Mothername;
								profile.PaternalSurname = viewModel.PaternalSurname;
								profile.MaternalSurname = viewModel.MaternalSurname;
								profile.Mosal = viewModel.Mosal;
								profile.Gender = viewModel.Gender;
								profile.LookingForGender = viewModel.LookingForGender;
								profile.MaritalStatus = viewModel.MaritalStatus;
								profile.DateOfBirth = viewModel.DateOfBirth;
								profile.Address = viewModel.Address;
								profile.City = viewModel.City;
								profile.State = viewModel.State;
								profile.Country = viewModel.Country;
								profile.Education = viewModel.Education;
								profile.Occupation = viewModel.Occupation;
								profile.Summary = viewModel.Summary;
								profile.GroupId = viewModel.GroupId;
								profile.Interests = viewModel.Interests;
								profile.Smoking = viewModel.Smoking;
								profile.Height = viewModel.Height;
								profile.Weight = viewModel.Weight;
								profile.HairColor = viewModel.HairColor;
								profile.EyeColor = viewModel.EyeColor;
								profile.BodyType = viewModel.BodyType;
								profile.Diet = viewModel.Diet;
								profile.Language = viewModel.Language;
								profile.LastModifiedBy = AppHttpContextAccessor.LoggedUserId;
								profile.LastModifiedDate = DateTime.Now;

								_context.Using<Profile>().Update(profile);
							}
							else
							{
								viewModel.UserId = AppHttpContextAccessor.LoggedUserId;
								viewModel.CreatedBy = AppHttpContextAccessor.LoggedUserId;
								viewModel.CreatedDate = DateTime.Now;

								viewModel = _context.Using<Profile>().Add(viewModel);
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

		[HttpPost]
		[RequestSizeLimit(500_000_000)]
		[RequestFormLimits(MultipartBodyLengthLimit = 500_000_000)]
		public IActionResult UploadPhotos(List<IFormFile> fileProfile = null, List<IFormFile> fileCover = null, List<IFormFile> fileOthers = null)
		{
			try
			{
				var profile = _context.Using<Profile>().GetByCondition(x => x.UserId == AppHttpContextAccessor.LoggedUserId).FirstOrDefault();

				if (profile == null)
				{
					profile = new Profile() { UserId = AppHttpContextAccessor.LoggedUserId, IsActive = true, IsDeleted = false, CreatedBy = 1, CreatedDate = DateTime.Now };
					profile = _context.Using<Profile>().Add(profile);
				}

				List<IFormFile> files = new List<IFormFile>();
				if (fileProfile != null && fileProfile.Count() > 0) files.AddRange(fileProfile);
				if (fileCover != null && fileCover.Count() > 0) files.AddRange(fileCover);
				if (fileOthers != null && fileOthers.Count() > 0) files.AddRange(fileOthers);

				if (profile != null && files != null && files.Count > 0 && files.Any(x => x.Length > 0))
				{
					foreach (var fileItem in files.Where(x => x.Length > 0).ToList())
					{
						try
						{
							if (!string.IsNullOrEmpty(fileItem.Name) && (fileItem.Name == "fileCover" || fileItem.Name == "fileProfile"))
							{
								string uploadFolder = System.IO.Path.Combine(AppHttpContextAccessor.WebRootPath, "UploadPhotos", fileItem.Name.Replace("file", ""));
								if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);
								string fullPath = Path.Combine(uploadFolder, profile.Id + ".jpg");
								if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
								using (var stream = new FileStream(fullPath, FileMode.Create)) { fileItem.CopyTo(stream); }
							}
							else
							{
								string uploadFolder = System.IO.Path.Combine(AppHttpContextAccessor.WebRootPath, "UploadPhotos", "Others");
								if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

								string fullPath = Path.Combine(uploadFolder, profile.Id + "_1.jpg");

								string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullPath);

								int counter = 1;
								while (System.IO.File.Exists(fullPath))
								{
									string newFileName = $"{fileNameWithoutExtension.Split('_')[0]}_{counter}.jpg";

									fullPath = Path.Combine(uploadFolder, newFileName);
									counter++;
								}

								using (var stream = new FileStream(fullPath, FileMode.Create)) { fileItem.CopyTo(stream); }
							}
						}
						catch (Exception ex) { continue; }

					}

					CommonViewModel.IsConfirm = true;
					CommonViewModel.IsSuccess = true;
					CommonViewModel.StatusCode = ResponseStatusCode.Success;
					CommonViewModel.Message = "Record saved successfully ! ";

					CommonViewModel.RedirectURL = Url.Action("ProfileUpdate", "Home", new { area = "" });

					return Json(CommonViewModel);
				}
			}
			catch (Exception ex) { }

			CommonViewModel.Message = ResponseStatusMessage.Error;
			CommonViewModel.IsSuccess = false;
			CommonViewModel.StatusCode = ResponseStatusCode.Error;

			return Json(CommonViewModel);
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
						CommonViewModel.Message = "Username not exist";
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
								NextChangePasswordDate = DateTime.Now
							};

							user = _context.Using<User>().Add(user);

							var role = _context.Using<Role>().GetByCondition(x => x.Name == "Candidate").FirstOrDefault();

							if (role == null)
							{
								role = new Role() { Name = "Candidate", IsAdmin = true };
								role = _context.Using<Role>().Add(role);
							}

							if (role != null)
							{
								var userRole = new UserRoleMapping() { UserId = user.Id, RoleId = role.Id };
								_context.Using<UserRoleMapping>().Add(userRole);
							}


							var profile = _context.Using<Profile>().GetByCondition(x => x.UserId == user.Id).FirstOrDefault();

							if (profile == null)
							{
								profile = new Profile() { UserId = user.Id };
								profile = _context.Using<Profile>().Add(profile);
							}

							Common.Set_Session_Int(SessionKey.KEY_USER_ID, Convert.ToInt32(user.Id));

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

		public IActionResult Logout()
		{
			Common.Clear_Session();

			return RedirectToAction("Index", "Home");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
