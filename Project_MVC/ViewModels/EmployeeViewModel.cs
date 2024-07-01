using Demo.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        [MaxLength(50, ErrorMessage = "Max length is 50 chars")]
        [MinLength(5, ErrorMessage = "Min length is 5 chars")]
        public string Name { get; set; }
        [Range(22, 30, ErrorMessage = "Age must be between 22 and 30")]
        public int? Age { get; set; }
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{5,10}-[a-zA-Z]{5,10}$"
           , ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        [Range(4000, 8000)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        public string ImageName { get; set; }
        public IFormFile Image { get; set; }
    }
}
