using CompanyApp.Models;
using System.ComponentModel.DataAnnotations;


namespace CompanyApp.ViewModels
{
    public class EmployeeViewModel
    {
        public EmployeeViewModel()
        {
            
           AvailableDepartments = new List<Department>();
            AvailablePositions = new List<Position>();
            AvailableCities = new List<City>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(100, ErrorMessage = "Last Name cannot exceed 100 characters.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(100, ErrorMessage = "First Name cannot exceed 100 characters.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [StringLength(100, ErrorMessage = "Middle Name cannot exceed 100 characters.")]
        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [StringLength(50, ErrorMessage = "Phone cannot exceed 50 characters.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Birth Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Birth Date")]
        public DateOnly BirthDate { get; set; }

        [Required(ErrorMessage = "Hire Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateOnly HireDate { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0.01, 1000000.00, ErrorMessage = "Salary must be between 0.01 and 1,000,000.00.")]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }

        [Required(ErrorMessage = "Position is required.")]
        [Display(Name = "Position")]
        public int PositionId { get; set; }
        public string? PositionName { get; set; }





        [Required(ErrorMessage = "City is required.")]
        [Display(Name = "City")]
        public int CityId { get; set; }
        public string CityName { get; set; } = null!; 

        [Required(ErrorMessage = "Street name is required.")]
        [StringLength(100, ErrorMessage = "Street name cannot exceed 100 characters.")]
        public string StreetName { get; set; } = null!;

        [Required(ErrorMessage = "Building number is required.")]
        [StringLength(20, ErrorMessage = "Building number cannot exceed 20 characters.")]
        public string BuildingNumber { get; set; } = null!;

        [StringLength(20, ErrorMessage = "Apartment number cannot exceed 20 characters.")]
        public string? ApartmentNumber { get; set; } 

       
        public string FullAddress
        {
            get
            {
               
                string fullAddress = $"{CityName}, {StreetName}, {BuildingNumber}";

                
                if (!string.IsNullOrEmpty(ApartmentNumber))
                {
                    fullAddress += $", apt. {ApartmentNumber}";
                }

                return fullAddress;
            }
        }



        public IEnumerable<Department>? AvailableDepartments { get; set; }
        public IEnumerable<Position>? AvailablePositions { get; set; }
        public IEnumerable<City>? AvailableCities { get; set; }
       

    }
    public class EmployeeListViewModel
    {
        public IEnumerable<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();
    }
}