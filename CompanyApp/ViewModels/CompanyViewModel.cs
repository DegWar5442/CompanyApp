using System.ComponentModel.DataAnnotations;

namespace CompanyApp.ViewModels
{
    public class CompanyViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Company Name")] 
        public string Name { get; set; } = null!;


        [Display(Name = "Company Phone")]
        public string Phone { get; set; }

        public string CityName { get; set; }
        public string StreetName { get; set; }
        public string BuildingNumber { get; set; }
        public string? ApartmentNumber { get; set; }

        [Display(Name = "Full Address")]
        public string FullAddressDisplay { get; set; }
    }
}