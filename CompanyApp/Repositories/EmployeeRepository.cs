using CompanyApp.Data;
using CompanyApp.Models;
using CompanyApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CompanyApp.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _dbContext;

        public EmployeeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // 1. AddAsync: creates the Employee 
        public async Task AddAsync(EmployeeViewModel employeeVm)
        {
            
            var newAddress = new Address
            {
                CityId = employeeVm.CityId,
                StreetName = employeeVm.StreetName,
                BuildingNumber = employeeVm.BuildingNumber,
                ApartmentNumber = employeeVm.ApartmentNumber
            };

            await _dbContext.Addresses.AddAsync(newAddress);
            await _dbContext.SaveChangesAsync();

            
            var newEmployee = new Employee
            {
                FirstName = employeeVm.FirstName,
                LastName = employeeVm.LastName,
                MiddleName = employeeVm.MiddleName,
                BirthDate = employeeVm.BirthDate,
                Phone = employeeVm.Phone,
                HireDate = employeeVm.HireDate,
                Salary = employeeVm.Salary,
                DepartmentId = employeeVm.DepartmentId,
                PositionId = employeeVm.PositionId,
                AddressId = newAddress.Id,
                Address = newAddress
            };

            await _dbContext.Employees.AddAsync(newEmployee);
            await _dbContext.SaveChangesAsync();
        }

        // 2. DeleteAsync: Deletes the Employee 
        public async Task DeleteAsync(int Id)
        {
            
            var employeeToDelete = await _dbContext.Employees
                                             .Include(e => e.Address)
                                             .FirstOrDefaultAsync(e => e.Id == Id);

            if (employeeToDelete == null)
            {
                
                return;
            }

            
            _dbContext.Addresses.Remove(employeeToDelete.Address);
            _dbContext.Employees.Remove(employeeToDelete);
            await _dbContext.SaveChangesAsync();
        }

        // 3. GetAllAsync: Retrieves all employees and maps them to EmployeeViewModel.
        
        public IQueryable<EmployeeViewModel> GetAllAsync()
        {
            var employees = _dbContext.Employees
                .Include(e => e.Address)        
                    .ThenInclude(a => a.City)   
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Select(e => new EmployeeViewModel
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    MiddleName = e.MiddleName,
                    BirthDate = e.BirthDate,
                    Phone = e.Phone,
                    HireDate = e.HireDate,
                    Salary = e.Salary,

                   
                    CityId = e.Address.CityId,
                    CityName = e.Address.City.Name, 
                    StreetName = e.Address.StreetName,
                    BuildingNumber = e.Address.BuildingNumber,
                    ApartmentNumber = e.Address.ApartmentNumber, 

                    DepartmentId = e.DepartmentId,
                    PositionId = e.PositionId
                });

            return employees;
        }


        // 4. GetByIdAsync: Retrieves a single employee by ID and maps it to EmployeeViewModel.

        public async Task<EmployeeViewModel> GetByIdAsync(int id)
        {
            var employee = await _dbContext.Employees
                                           .Include(e => e.Address)
                                               .ThenInclude(a => a.City)
                                           .Include(e => e.Department)
                                           .Include(e => e.Position)
                                           .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return null; 
            }

            var employeeViewModel = new EmployeeViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                BirthDate = employee.BirthDate,
                Phone = employee.Phone,
                HireDate = employee.HireDate,
                Salary = employee.Salary,

               
                CityId = employee.Address.CityId,
                CityName = employee.Address.City.Name, 
                StreetName = employee.Address.StreetName,
                BuildingNumber = employee.Address.BuildingNumber,
                ApartmentNumber = employee.Address.ApartmentNumber,

                DepartmentId = employee.DepartmentId,
                PositionId = employee.PositionId,
            };
            return employeeViewModel;
        }

        // 5. UpdateAsync: Updates an existing Employee 

        public async Task UpdateAsync(EmployeeViewModel employeeUpdated)
        {
            var employeeToUpdate = await _dbContext.Employees
                                                   .Include(e => e.Address) 
                                                   .FirstOrDefaultAsync(e => e.Id == employeeUpdated.Id);

            if (employeeToUpdate == null)
            {
                return; 
            }

            employeeToUpdate.FirstName = employeeUpdated.FirstName;
            employeeToUpdate.LastName = employeeUpdated.LastName;
            employeeToUpdate.MiddleName = employeeUpdated.MiddleName;
            employeeToUpdate.BirthDate = employeeUpdated.BirthDate;
            employeeToUpdate.Phone = employeeUpdated.Phone;
            employeeToUpdate.HireDate = employeeUpdated.HireDate;
            employeeToUpdate.Salary = employeeUpdated.Salary;
            employeeToUpdate.DepartmentId = employeeUpdated.DepartmentId;
            employeeToUpdate.PositionId = employeeUpdated.PositionId;


            employeeToUpdate.Address.CityId = employeeUpdated.CityId;
            employeeToUpdate.Address.StreetName = employeeUpdated.StreetName;
            employeeToUpdate.Address.BuildingNumber = employeeUpdated.BuildingNumber;
            employeeToUpdate.Address.ApartmentNumber = employeeUpdated.ApartmentNumber;

            _dbContext.Employees.Update(employeeToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Department>> GetAllDepartments()
        {
            return await _dbContext.Departments.ToListAsync();
        }

        public async Task<List<City>> GetAllCities()
        {
            return await _dbContext.Cities.ToListAsync();
        }

        public async Task<List<Position>> GetAllPositions()
        {
            return await _dbContext.Positions.ToListAsync();
        }
    }
}