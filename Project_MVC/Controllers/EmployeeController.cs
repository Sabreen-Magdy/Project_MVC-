using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
           var employees = Enumerable.Empty<Employee>();
            if (string.IsNullOrEmpty(SearchValue))
            {
                employees=await _unitOfWork._EmployeeRepository.GetAll();
            }else
            {
                employees =await _unitOfWork._EmployeeRepository.SearchByName(SearchValue);
            }
             
            var MappedEmployees=_mapper.Map<IEnumerable<Employee>,IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployees);
        }
        public async Task<IActionResult> Create()
        {
            var depts =await _unitOfWork._DepartmentRepository.GetAll();
            var MappedDepartments = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel> >(depts);
            if (MappedDepartments == null || !MappedDepartments.Any())
            {
                ModelState.AddModelError("", "No departments found");
            }
            ViewBag.departments= MappedDepartments;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                employeeVM.ImageName= DocumentsSetting.UploadFile(employeeVM.Image,"Images");
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                await _unitOfWork._EmployeeRepository.Add(MappedEmployee);
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id == null)
                return NotFound();
            var employee =await _unitOfWork._EmployeeRepository.GetById(id.Value);
            if (employee == null)
                return NotFound();
            var depts =await _unitOfWork._DepartmentRepository.GetAll();
            var MappedDepartments = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(depts);
            ViewBag.departments = MappedDepartments;
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(ViewName, MappedEmployee);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id,"Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var ExistingEmployee=await _unitOfWork._EmployeeRepository.GetById(employeeVM.Id);
                    if (employeeVM.Image != null)
                    {
                        var NewImage = DocumentsSetting.UploadFile(employeeVM.Image, "Images");
                        if (!string.IsNullOrEmpty(ExistingEmployee.ImageName))
                        {
                            DocumentsSetting.DeleteFile(ExistingEmployee.ImageName, "Images");
                        }
                        employeeVM.ImageName = NewImage;
                    }
                    else
                    {
                        employeeVM.ImageName=ExistingEmployee.ImageName;
                    }
                    _unitOfWork._DbContext.Entry(ExistingEmployee).State = EntityState.Detached;

                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                     await _unitOfWork._EmployeeRepository.Update(MappedEmployee);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(employeeVM);
                }
            }
            var depts =await _unitOfWork._DepartmentRepository.GetAll();
            var MappedDepartments = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(depts);
            ViewBag.departments = MappedDepartments;
            return View(employeeVM);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            try
            {
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                int count=await _unitOfWork._EmployeeRepository.Delete(MappedEmployee);
                if (count > 0)
                    DocumentsSetting.DeleteFile(employeeVM.ImageName,"Images");
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(employeeVM);
            }
        }
    }
}
