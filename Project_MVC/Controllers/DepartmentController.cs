using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	[Authorize]
	public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            var depts=Enumerable.Empty<Department>();
            if (string.IsNullOrEmpty(SearchValue))
            {
                depts =await _unitOfWork._DepartmentRepository.GetAll();
            }
            else
            {
                depts =await _unitOfWork._DepartmentRepository.SearchByName(SearchValue);
            }
             
            var MappedDepartments=_mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(depts);
            return View(MappedDepartments);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {   
            if(ModelState.IsValid)
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                await _unitOfWork._DepartmentRepository.Add(MappedDepartment);
                return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }
        public async Task<IActionResult> Details(int? id,string ViewName= "Details")
        {
            if (id == null)
                return NotFound();
            var dept=await _unitOfWork._DepartmentRepository.GetById(id.Value);
            if(dept==null)
                return NotFound();
            var MappedDepartment = _mapper.Map<Department, DepartmentViewModel>(dept);
            return View(ViewName, MappedDepartment);
        }
        public async Task<IActionResult> Edit(int? id)
        {
           return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id,DepartmentViewModel departmentVM)
        {
            if(id != departmentVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    await _unitOfWork._DepartmentRepository.Update(MappedDepartment);
                    return RedirectToAction(nameof(Index));
                }
                catch(System.Exception ex) 
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(departmentVM);
                }
            }
            return View(departmentVM);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id,"Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute]int id,DepartmentViewModel departmentVM)
        {
            if(id != departmentVM.Id)
                return BadRequest();
            try
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                await _unitOfWork._DepartmentRepository.Delete(MappedDepartment);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(departmentVM);
            }
        }
    }
}
