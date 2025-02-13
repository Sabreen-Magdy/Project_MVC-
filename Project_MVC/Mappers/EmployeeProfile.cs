﻿using AutoMapper;
using Demo.DAL.Entities;
using Demo.PL.ViewModels;

namespace Demo.PL.Mappers
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel,Employee>().ReverseMap();
        }
    }
}
