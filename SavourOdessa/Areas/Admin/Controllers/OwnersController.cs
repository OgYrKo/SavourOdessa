using DataLayer.EfClasses;
using DataLayer.EFClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavourOdessa.Areas.Admin.Models.Owners;

namespace SavourOdessa.Areas.Admin.Controllers
{
    [Area("Admin")]
    [CustomAuthorize("Admin")]
    public class OwnersController : Controller
    {
        private readonly DataContext _context;
        public OwnersController(DataContext context)
        {
            _context = context;
        }

        // GET: OwnersController
        public async Task<ActionResult> Index()
        {
            var owners = await _context.Users
                                       .Include(u=>u.Restaurants)
                                       .Where(u=>u.Rolname.ToLower()=="manager")
                                       .ToListAsync();
            List<OwnerListItemVIewModel> viewModel = new List<OwnerListItemVIewModel>();
            foreach (var owner in owners)
            {
                var ownerViewModel = new OwnerListItemVIewModel
                {
                    Usesysid = owner.Usesysid,
                    Username = owner.Username,
                    Restaurants = string.Join(", ", owner.Restaurants.Select(r=>r.Restaurantname))
                };
                viewModel.Add(ownerViewModel);
            }
            return View(viewModel);
        }

        // GET: OwnersController/Create
        public ActionResult Create()
        {
            var viewModel = new OwnerEditViewModel();
            return View(viewModel);
        }

        // POST: OwnersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OwnerEditViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _context.CreateManager(viewModel.Username, viewModel.Password);
                    return RedirectToAction(nameof(Index));
                }
                return View(viewModel);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(viewModel);
            }
        }


        // GET: OwnersController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await _context.DeleteUser(user.Username);
            return RedirectToAction(nameof(Index));
        }

    }
}
