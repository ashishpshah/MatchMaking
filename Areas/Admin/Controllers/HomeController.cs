using MatchMaking.Controllers;
using MatchMaking.Infra;
using MatchMaking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MatchMaking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : BaseController<ResponseModel<LoginViewModel>>
    {
        public HomeController(IRepositoryWrapper repository) : base(repository) { }

        public ActionResult Index()
        {
            if (Common.LoggedUser_Id() <= 0)
                return RedirectToAction("Account", "Home", new { Area = "Admin" });

            return View();
        }

        public ActionResult Account()
        {
            Common.Clear_Session();

            return View(new ResponseModel<LoginViewModel>());
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Login(LoginViewModel viewModel)
        {
            try
            {
                if (!string.IsNullOrEmpty(viewModel.Username) && viewModel.Username.Length > 0 && _context.Using<User>().GetAll().ToList().Any(x => x.UserName == viewModel.Username))
                {
                    viewModel.Password = Common.Encrypt(viewModel.Password);

                    var obj = _context.Using<User>().GetByCondition(x => x.UserName == viewModel.Username && x.Password == viewModel.Password).FirstOrDefault();

                    if (obj != null && obj.IsActive == true && obj.IsDeleted == false)
                    {
                        var userRole = _context.Using<UserRoleMapping>().GetByCondition(x => x.UserId == obj.Id).FirstOrDefault();

                        obj.RoleId = userRole != null ? userRole.RoleId : 0;

                        List<UserMenuAccess> listMenuAccess = new List<UserMenuAccess>();
                        List<UserMenuAccess> listMenuPermission = new List<UserMenuAccess>();

                        Role role = _context.Using<Role>().GetByCondition(x => x.Id == obj.RoleId).FirstOrDefault();

                        if (role == null)
                        {
                            CommonViewModel.IsSuccess = false;
                            CommonViewModel.StatusCode = ResponseStatusCode.Error;
                            CommonViewModel.Message = ResponseStatusMessage.Error;

                            return Json(CommonViewModel);
                        }
                        else if (role != null && role.Id == 1)
                        {
                            listMenuAccess = (from y in _context.Using<Menu>().GetAll().ToList()
                                              where y.IsActive == true && y.IsDeleted == false
                                              select new UserMenuAccess() { Id = y.Id, ParentMenuId = y.ParentId, Area = y.Area, Controller = y.Controller, Url = y.Url, MenuName = y.Name, IsCreate = true, IsUpdate = true, IsRead = true, IsDelete = true, DisplayOrder = y.DisplayOrder, IsActive = y.IsActive, IsDeleted = y.IsDeleted }).ToList();
                        }
                        else if (role != null && role.IsAdmin)
                        {
                            listMenuAccess = (from x in _context.Using<UserMenuAccess>().GetAll().ToList()
                                              join y in _context.Using<Menu>().GetAll().ToList() on x.MenuId equals y.Id
                                              where x.UserId == obj.Id && x.RoleId == obj.RoleId
                                              && y.IsActive == true && y.IsDeleted == false && x.IsActive == true && x.IsDeleted == false && y.Name != "Menu"
                                              && x.IsRead == true
                                              select new UserMenuAccess() { Id = y.Id, ParentMenuId = y.ParentId, Area = y.Area, Controller = y.Controller, Url = y.Url, MenuName = y.Name, DisplayOrder = y.DisplayOrder, IsActive = x.IsActive, IsDeleted = x.IsDeleted }).ToList();
                        }
                        else if (role != null && !role.IsAdmin && role.IsActive && !role.IsDeleted)
                        {
                            listMenuAccess = (from x in _context.Using<UserMenuAccess>().GetAll().ToList()
                                              join y in _context.Using<Menu>().GetAll().ToList() on x.MenuId equals y.Id
                                              where x.UserId == obj.Id && x.RoleId == obj.RoleId
                                              && y.IsActive == true && y.IsDeleted == false && x.IsActive == true && x.IsDeleted == false && y.Id != 1 && y.ParentId != 1 && y.Name != "Menu"
                                              && x.IsRead == true
                                              select new UserMenuAccess() { Id = y.Id, ParentMenuId = y.ParentId, Area = y.Area, Controller = y.Controller, Url = y.Url, MenuName = y.Name, DisplayOrder = y.DisplayOrder, IsActive = x.IsActive, IsDeleted = x.IsDeleted }).ToList();
                        }

                        if (role != null && role.Id == 1)
                            listMenuPermission = listMenuAccess;
                        else
                            listMenuPermission = (from x in _context.Using<UserMenuAccess>().GetAll().ToList()
                                                  join y in _context.Using<Menu>().GetAll().ToList() on x.MenuId equals y.Id
                                                  where x.UserId == obj.Id && y.IsActive == true && y.IsDeleted == false && x.IsActive == true && x.IsDeleted == false
                                                  && listMenuAccess.Any(z => z.Id == y.Id)
                                                  select new UserMenuAccess() { MenuId = y.Id, ParentMenuId = y.ParentId, Area = y.Area, Controller = y.Controller, Url = y.Url, MenuName = y.Name, IsCreate = x.IsCreate, IsUpdate = x.IsUpdate, IsRead = x.IsRead, IsDelete = x.IsDelete, IsActive = x.IsActive, IsDeleted = x.IsDeleted }).ToList();

                        Common.Configure_UserMenuAccess(listMenuAccess.Where(x => x.IsActive == true && x.IsDeleted == false).ToList(), listMenuPermission.Where(x => x.IsActive == true && x.IsDeleted == false).ToList());

                        Common.Set_Session_Int(SessionKey.KEY_USER_ID, (Int32)obj.Id);
                        Common.Set_Session_Int(SessionKey.KEY_USER_ROLE_ID, (Int32)obj.RoleId);

                        Common.Set_Session(SessionKey.KEY_USER_NAME, obj.UserName);
                        Common.Set_Session(SessionKey.KEY_USER_ROLE, role.Name);
                        Common.Set_Session_Int(SessionKey.KEY_IS_ADMIN, (role.IsAdmin || obj.RoleId == 1 ? 1 : 0));
                        Common.Set_Session_Int(SessionKey.KEY_IS_SUPER_USER, (obj.RoleId == 1 ? 1 : 0));



                        CommonViewModel.IsSuccess = true;
                        CommonViewModel.StatusCode = ResponseStatusCode.Success;
                        CommonViewModel.Message = ResponseStatusMessage.Success;

                        CommonViewModel.RedirectURL = Url.Content("~/") + "Admin/" + this.ControllerContext.RouteData.Values["Controller"].ToString() + "/Index";

                        return Json(CommonViewModel);
                    }

                }

                CommonViewModel.IsSuccess = false;
                CommonViewModel.StatusCode = ResponseStatusCode.Error;
                CommonViewModel.Message = "User Id and Password does not Match";

            }
            catch (Exception ex)
            {
                CommonViewModel.IsSuccess = false;
                CommonViewModel.StatusCode = ResponseStatusCode.Error;
                CommonViewModel.Message = ResponseStatusMessage.Error + " | " + ex.Message;
            }

            return Json(CommonViewModel);
        }

        public ActionResult Logout()
        {
            Common.Clear_Session();

            return RedirectToAction("Account", "Home", new { Area = "Admin" });
        }


        [HttpGet]
        public PartialViewResult ForgotPassword()
        {
            return PartialView("_Partial_ForgotPassword", new ResponseModel<ForgotPassword>());
        }

        // Step 1: Submit email
        [HttpPost]
        public JsonResult ForgotPassword_SendOTP(string email)
        {
            var userExists = _context.Using<User>().Any(u => u.Email == email);

            if (!userExists)
            {
                return Json(new { IsSuccess = false, Message = "Email does not exist." });
            }

            // Generate OTP
            var otp = new Random().Next(100000, 999999).ToString();
            var otpEntry = new ForgotPassword
            {
                Email = email,
                OTP = otp,
                CreatedAt = DateTime.Now,
                IsUsed = false
            };

            _context.Using<ForgotPassword>().Add(otpEntry);


            Common.SendEmail(
               "Your OTP Code",
               email,
               true,                       // HTML enabled
               "",                         // body will be replaced by template
               "otp_message",              // template name without _enquiry.html
               JsonConvert.SerializeObject(new { otp = otp })
           );
            

            // Send Email (use your Common.SendEmail logic)
            // await Common.SendEmail(...);

            return Json(new { IsSuccess = true, Message = "OTP sent to email." });
        }

        // Step 2: Verify OTP
        [HttpPost]
        public JsonResult ForgotPassword_VerifyOTP(string email, string otp)
        {
            var record = _context.Using<ForgotPassword>()
                .GetByCondition(f => f.Email == email && f.OTP == otp && !f.IsUsed)
                .OrderByDescending(f => f.CreatedAt)
                .FirstOrDefault();

            if (record == null)
                return Json(new { IsSuccess = false, Message = "Invalid OTP." });

            // Mark as used
            record.IsUsed = true;
            _context.Using<ForgotPassword>().Update(record);

            return Json(new { IsSuccess = true, Message = "OTP verified." });
        }

        // Step 3: Reset Password
        [HttpPost]
        public JsonResult ForgotPassword_ResetPassword(string email, string newPassword)
        {
            var user = _context.Using<User>().GetByCondition(u => u.Email == email).FirstOrDefault();
            if (user == null)
                return Json(new { IsSuccess = false, Message = "User not found." });

            user.Password = Common.Encrypt(newPassword);
            _context.Using<User>().Update(user);

            return Json(new { IsSuccess = true, Message = "Password reset successfully." });
        }


    }
}
