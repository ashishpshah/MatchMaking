using MatchMaking.Areas.Admin.Models;
using MatchMaking.Controllers;
using MatchMaking.Infra;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace MatchMaking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class JainGroupController : BaseController<ResponseModel<JainGroup>>
    {
        public JainGroupController(IRepositoryWrapper repository) : base(repository)
        {
        }
        public IActionResult Index()
        {
            CommonViewModel.ObjList = new List<JainGroup>();
            CommonViewModel.ObjList = _context.Using<JainGroup>().GetAll().ToList();
            return View(CommonViewModel);
        }
        public ActionResult Partial_AddEditForm(long Id = 0)
        {
            CommonViewModel.Obj = new JainGroup();

            // CommonViewModel.Obj = new User();

            if (Id > 0)
            {
                CommonViewModel.Obj = (from x in _context.Using<JainGroup>().GetAll().ToList()
                                       where x.Id == Id
                                       select new JainGroup() { Id = x.Id, Name = x.Name }).FirstOrDefault();
            }
            if (CommonViewModel.Obj != null)
            {
               CommonViewModel.Obj.Id = CommonViewModel.Obj.Id;
               // CommonViewModel.Obj.User_Id_Str = CommonViewModel.Obj != null && CommonViewModel.Obj.Id > 0 ? Common.Encrypt(CommonViewModel.Obj.Id.ToString()) : null;
            }

            return PartialView("_Partial_AddEditForm", CommonViewModel);

        }

        

        [HttpPost]
        public ActionResult Save(ResponseModel<JainGroup> viewModel)
        {
            try
            {
                if (viewModel?.Obj == null)
                {
                    CommonViewModel.Message = "Invalid data.";
                    return Json(CommonViewModel);
                }

               // long Decrypt_Id = !string.IsNullOrEmpty(viewModel.Obj.User_Id_Str) ? Convert.ToInt64(Common.Decrypt(viewModel.Obj.User_Id_Str)) : 0;
                long Decrypt_Id = viewModel.Obj.Id;


                // VALIDATION
                if (string.IsNullOrWhiteSpace(viewModel.Obj.Name))
                {
                    CommonViewModel.Message = "Please enter Name.";
                    return Json(CommonViewModel);
                }

                // Check duplicate
                if (_context.Using<JainGroup>()
                        .Any(x => x.Name.ToLower() == viewModel.Obj.Name.ToLower()
                               && x.Id != Decrypt_Id))
                {
                    CommonViewModel.Message = "Name already exists.";
                    return Json(CommonViewModel);
                }

                using (var tran = _context.BeginTransaction())
                {
                    try
                    {
                        JainGroup obj = _context.Using<JainGroup>().GetByCondition(x => x.Id == Decrypt_Id).FirstOrDefault();

                        if (obj != null && (Common.IsAdmin()))
                        {
                            // UPDATE
                            obj.Name = viewModel.Obj.Name;
                            obj.IsActive = viewModel.Obj.IsActive;

                            _context.Using<JainGroup>().Update(obj);
                        }
                        else if (Common.IsAdmin())
                        {
                            _context.Using<JainGroup>().Add(viewModel.Obj);
                        }

                        CommonViewModel.IsConfirm = true;
                        CommonViewModel.IsSuccess = true;
                        CommonViewModel.StatusCode = ResponseStatusCode.Success;
                        CommonViewModel.Message = "Record saved successfully ! ";
                        CommonViewModel.RedirectURL = Url.Action("Index", "JainGroup", new { area = "Admin" });

                        tran.Commit();

                        return Json(CommonViewModel);
                    }
                    catch (Exception ex)
                    { tran.Rollback(); }
                }


            }

            catch (Exception ex) { }


            CommonViewModel.Message = ResponseStatusMessage.Error;
            CommonViewModel.IsSuccess = false;
            CommonViewModel.StatusCode = ResponseStatusCode.Error;

            return Json(CommonViewModel);
        }
        [HttpPost]
        //[CustomAuthorizeAttribute(AccessType_Enum.Delete)]
        public ActionResult DeleteConfirmed(long Id)
        {
            try
            {
                //if (_context.Using<User>().GetAll().ToList().Any(x => x.Id == Id))
                if (_context.Using<JainGroup>().GetAll().ToList().Any(x => x.Id == Id))
                {
                   

                    var data = _context.Using<JainGroup>().GetByCondition(x => x.Id == Id).FirstOrDefault();

                    if (data != null)
                    {
                        _context.Using<JainGroup>().Delete(data);
                    }



                    CommonViewModel.IsConfirm = true;
                    CommonViewModel.IsSuccess = true;
                    CommonViewModel.StatusCode = ResponseStatusCode.Success;
                    CommonViewModel.Message = "Data deleted successfully ! ";
                    CommonViewModel.RedirectURL = Url.Action("Index", "JainGroup", new { area = "Admin" });

                    return Json(CommonViewModel);
                }

            }
            catch (Exception ex)
            { }


            CommonViewModel.Message = "Unable to delete User.";
            CommonViewModel.IsSuccess = false;
            CommonViewModel.StatusCode = ResponseStatusCode.Error;

            return Json(CommonViewModel);
        }
    }
}











