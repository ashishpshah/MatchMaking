using MatchMaking.Controllers;
using MatchMaking.Infra;
using MatchMaking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatchMaking.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class HomeController : BaseController<dynamic>
	{
		public HomeController(IRepositoryWrapper repository) : base(repository) { }

		public IActionResult Index()
		{
			var list = _context.Using<User>().GetAll().ToList();

			return View();
		}
	}
}
