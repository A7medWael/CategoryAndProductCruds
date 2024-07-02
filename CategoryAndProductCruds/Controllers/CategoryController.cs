using Cores.Entities;
using Cores.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Buffers;
using X.PagedList;

namespace CategoryAndProductCruds.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string SearchValue="", int page=1)
        {
            var pagnum = page;
            var pagesize = 5;

            IEnumerable<Category> Categ;
            if (string.IsNullOrEmpty(SearchValue))
            {
                Categ= _unitOfWork.CategoryRepository.GetAll().ToPagedList(pagnum,pagesize);
            }
            else
            {
                Categ = _unitOfWork.CategoryRepository.Search(SearchValue).ToPagedList(pagnum, pagesize);
            }
           
            return View(Categ);
        }
        
        public IActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Insert(Category category)
        {
            if(ModelState.IsValid)
            {
                var CA=_unitOfWork.CategoryRepository.GetAll(x=>x.Name==category.Name);
                if (CA.Any())
                {
                    return BadRequest();
                }
                else
                {
                    _unitOfWork.CategoryRepository.Add(category);
                    _unitOfWork.compelete();
                    TempData["Create"] = "Data has Inserted successfully";
                    return RedirectToAction(nameof(Index));
                }        
               
            }
            return View(category);
        }
        public IActionResult Edit(int id)
        {
            var category=_unitOfWork.CategoryRepository.GetFirstOrDefault(x=>x.Id==id);
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var CA = _unitOfWork.CategoryRepository.GetAll();
                foreach (var item in CA)
                {
                    if (item.Id==category.Id&&item.Name==category.Name|| item.Id == category.Id && item.Name != category.Name)
                    {
                        _unitOfWork.CategoryRepository.Update(category);
                        _unitOfWork.compelete();
                        TempData["Update"] = "Data has Updated successfully";
                        return RedirectToAction(nameof(Index));
                    }
                    else if(item.Id!=category.Id&&item.Name==category.Name)
                    {
                            return BadRequest("Must isert Name Unique");
                    }
                }


            }
            return View(category);
        }
    
        public IActionResult Delete(Category category,int id)
        {
            var prod=_unitOfWork.ProductRepository.GetAll(p=>p.CategoryId==category.Id);
            if (prod.Any())
            {
                return BadRequest("Cannot delete category because this category has products");

            }
            else
            {
                var categ = _unitOfWork.CategoryRepository.GetFirstOrDefault(x => x.Id == id);
                _unitOfWork.CategoryRepository.Remove(categ);
                _unitOfWork.compelete();
                TempData["Delete"] = "Data has deleted successfully";
                return RedirectToAction(nameof(Index));
            }
          
        }
        public IActionResult AddActive(Category category)
        {
            _unitOfWork.CategoryRepository.AddActiveOrInActive(category);
            _unitOfWork.compelete();
            return RedirectToAction(nameof(Index));
        }

    }
}
