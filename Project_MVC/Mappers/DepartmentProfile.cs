using AutoMapper;
using Demo.DAL.Entities;
using Demo.PL.ViewModels;

namespace Demo.PL.Mappers
{
    public class DepartmentProfile:Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentViewModel,Department>().ReverseMap();
        }
    }
}
