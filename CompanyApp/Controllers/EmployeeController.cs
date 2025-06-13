using CompanyApp.Models;
using CompanyApp.Repositories;
using CompanyApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET: Employee
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["BirthDateSortParm"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewData["CurrentFilter"] = searchString; 

         
            var employeesQuery = _employeeRepository.GetAllAsync();

           
            if (!string.IsNullOrEmpty(searchString))
            {
                employeesQuery = employeesQuery.Where(e => e.FirstName.Contains(searchString)
                                                        || e.LastName.Contains(searchString)
                                                        || (e.MiddleName != null && e.MiddleName.Contains(searchString)));
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "name_desc":
                    employeesQuery = employeesQuery.OrderByDescending(e => e.FirstName);
                    break;
                case "date_asc":
                    employeesQuery = employeesQuery.OrderBy(s => s.BirthDate);
                    break;
                case "date_desc":
                    employeesQuery = employeesQuery.OrderByDescending(s => s.BirthDate);
                    break;
                default:
                    employeesQuery = employeesQuery.OrderBy(e => e.FirstName); 
                    break;
            }

           
            var employeeViewModels = await employeesQuery.ToListAsync(); 

            var viewModel = new EmployeeListViewModel
            {
                Employees = employeeViewModels
            };

            return View(viewModel);
        }

        // GET: Employee/Add
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var viewModel = new EmployeeViewModel
            {
                AvailableDepartments = await _employeeRepository.GetAllDepartments(),
                AvailablePositions = await _employeeRepository.GetAllPositions(),
                AvailableCities = await _employeeRepository.GetAllCities()
            };
            return View(viewModel);
        }

        // POST: Employee/Add
        [HttpPost]
        [ValidateAntiForgeryToken] // Always good practice for POST methods
        public async Task<IActionResult> Add(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                
                model.AvailableDepartments = await _employeeRepository.GetAllDepartments();
                model.AvailablePositions = await _employeeRepository.GetAllPositions();
                model.AvailableCities = await _employeeRepository.GetAllCities();
                return View(model);
            }

            await _employeeRepository.AddAsync(model);
            return RedirectToAction("Index"); 
        }

        // GET: Employee/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            
            employee.AvailableDepartments = await _employeeRepository.GetAllDepartments();
            employee.AvailablePositions = await _employeeRepository.GetAllPositions();
            employee.AvailableCities = await _employeeRepository.GetAllCities();

            return View(employee);
        }

        // POST: Employee/Edit
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit(EmployeeViewModel employeeViewModel)
        {
         

            if (!ModelState.IsValid)
            {
                
                employeeViewModel.AvailableDepartments = await _employeeRepository.GetAllDepartments();
                employeeViewModel.AvailablePositions = await _employeeRepository.GetAllPositions();
                employeeViewModel.AvailableCities = await _employeeRepository.GetAllCities();
                return View(employeeViewModel);
            }

            await _employeeRepository.UpdateAsync(employeeViewModel);
            return RedirectToAction("Index");
        }

        // GET: Employee/Delete/5 (Confirmation Page)
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee); 
        }

        // POST: Employee/Delete/5 (Actual Deletion)
        [HttpPost, ActionName("Delete")] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}