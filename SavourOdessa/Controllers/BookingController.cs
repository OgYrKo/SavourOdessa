using DataLayer.EfClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SavourOdessa.Controllers
{
    public class BookingController : Controller
    {
        private readonly DataContext _context;
        public BookingController(DataContext context)
        {
            _context = context;
        }
        public IActionResult BookingTables(int id)
        {
            return View(id);
        }
    }
}
