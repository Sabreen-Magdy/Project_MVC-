using Demo.DAL.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Demo.PL.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Code Is Required!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name Is Required!")]
        [MaxLength(50, ErrorMessage = "Max Length Name Is 50 Chars.")]
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
