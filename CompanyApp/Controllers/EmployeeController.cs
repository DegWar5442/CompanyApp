using CompanyApp.Repositories;
using CompanyApp.ViewModels;
using CompanyApp.Models; 
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

        public async Task<IActionResult> Index(string searchString, string sortOrder, string currentFilter)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["BirthDateSortParm"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";

            if (searchString != null)
            {
                ViewData["CurrentFilter"] = searchString;
            }
            else
            {
                searchString = currentFilter;
                ViewData["CurrentFilter"] = currentFilter;
            }

            var employeesQuery = _employeeRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                employeesQuery = employeesQuery.Where(e => e.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                                           e.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

           
            switch (sortOrder)
            {
                case "name_desc":
                    employeesQuery = employeesQuery.OrderByDescending(e => e.LastName);
                    break;
                case "date_asc":
                    employeesQuery = employeesQuery.OrderBy(s => s.BirthDate);
                    break;
                case "date_desc":
                    employeesQuery = employeesQuery.OrderByDescending(s => s.BirthDate);
                    break;
                default: 
                    employeesQuery = employeesQuery.OrderBy(e => e.LastName);
                    break;
            }


            var employees = await employeesQuery.ToListAsync();

         
            var employeeViewModels = employees.Select(e => new EmployeeViewModel
            {
                EmployeeId = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                MiddleName = e.MiddleName,
                Phone = e.Phone,
                BirthDate = e.BirthDate,
                HireDate = e.HireDate,
                Salary = e.Salary,
          
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department.Name,
                PositionId = e.PositionId,
                PositionName = e.Position.Name,
              
                CityId = e.Address.CityId,
                CityName = e.Address.City.Name,
                StreetName = e.Address.StreetName,
                BuildingNumber = e.Address.BuildingNumber,
                ApartmentNumber = e.Address?.ApartmentNumber,
               
            }).ToList();

       
            var viewModel = new EmployeeListViewModel
            {
                
                Employees = employeeViewModels
            };

            return View(viewModel);
        }


        //GET: Employee/Add
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var departments = await _employeeRepository.GetAllDepartments();
            ViewBag.Departments = new SelectList(departments, "Id", "Name"); 
            

            return View();
        }

        //POST: Employee/Add
        [HttpPost]
        public async Task<IActionResult> Add(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
               
                var departments = await _employeeRepository.GetAllDepartments();
                ViewBag.Departments = new SelectList(departments, "Id", "Name", model.DepartmentId);
             
                return View(model);
            }

            await _employeeRepository.AddAsync(model);
            return RedirectToAction("Index", "Employee");
        }


        //GET: Employee/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var departments = await _employeeRepository.GetAllDepartments();
            ViewBag.Departments = new SelectList(departments, "Id", "Name"); 

           


            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound(); 
            }

           
            ViewBag.Departments = new SelectList(departments, "Id", "Name", employee.DepartmentId);
        
            return View(employee);
        }

        //POST: Employee/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel employee)
        {
            if (!ModelState.IsValid)
            {
                
                var departments = await _employeeRepository.GetAllDepartments();
                ViewBag.Departments = new SelectList(departments, "Id", "Name", employee.DepartmentId);
              
                return View(employee);
            }

            await _employeeRepository.UpdateAsync(employee);
            return RedirectToAction("Index", "Employee");
        }
    }
}