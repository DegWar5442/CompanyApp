using System.Diagnostics;
using CompanyApp.Models;
using CompanyApp.ViewModels; 
using Microsoft.AspNetCore.Mvc;
using CompanyApp.Data;
using Microsoft.EntityFrameworkCore; 


namespace CompanyApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context; 

        public HomeController(ILogger<HomeController> logger, AppDbContext context) 
        {
            _logger = logger;
            _context = context; 
        }

        public async Task<IActionResult> Index() 
        {
           
            var company = await _context.Companies
                                        .Include(c => c.Address) 
                                            .ThenInclude(a => a.City) 
                                        .FirstOrDefaultAsync(); 

            if (company == null)
            {
                
                return View(null); 
            }

            
            return View(company);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}