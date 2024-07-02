using CategoryAndProductCruds.Models;
using Cores.Entities;
using Cores.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using System.Buffers;
using X.PagedList;

namespace CategoryAndProductCruds.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(string SearchValue="",int page=1)
        {

            var pagnum = page;
            var pagesize = 5;
            IEnumerable<Product> prod;
            if (string.IsNullOrEmpty(SearchValue))
            {
                prod = _unitOfWork.ProductRepository.GetAll(IncludeWord: "Category").ToPagedList(pagnum,pagesize);
            }
            else if(SearchValue.ToLower()=="active")
            {
                prod =_unitOfWork.ProductRepository.GetAll(x=>x.Status==true,IncludeWord: "Category").ToPagedList(pagnum, pagesize);
                   
            }else if (SearchValue.ToLower() == "inactive")
            {
                prod = _unitOfWork.ProductRepository.GetAll(x => x.Status == false, IncludeWord: "Category").ToPagedList(pagnum, pagesize);
            }
            else
            {
                prod = _unitOfWork.ProductRepository.GetAll(x => x.Name.Contains(SearchValue), IncludeWord: "Category").ToPagedList(pagnum, pagesize);
            }

            return View(prod);
        }

        public IActionResult Insert()
        {
            ProductVm productVm = new ProductVm()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })

            };
            return View(productVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Insert(ProductVm prod, IFormFile file)
        {
            

            string rootpath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string filename = Guid.NewGuid().ToString();
                var upload = Path.Combine(rootpath, "Images", "Products");
                var ext = Path.GetExtension(file.FileName);
                using (var filstream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
                {
                    file.CopyTo(filstream);
                }
                prod.Product.ImageUrl = Path.Combine("Images", "Products", filename + ext);
            }
            var products = _unitOfWork.ProductRepository.GetAll(x => x.Name == prod.Product.Name);
            if (products.Any())
            {
                return BadRequest();
            }
            else
            {
                _unitOfWork.ProductRepository.Add(prod.Product);
                _unitOfWork.compelete();
                TempData["Create"] = "Data has Inserted successfully";
                return RedirectToAction("Index");
            }

            
        }
        public IActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            ProductVm vm = new ProductVm()
            {
                Product = _unitOfWork.ProductRepository.GetFirstOrDefault(x => x.Id == id),
                CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(x => new SelectListItem
                {
                    
                Text  = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVm vm, IFormFile? file)
        {
            
                string rootpath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload = Path.Combine(rootpath, "Images", "Products");
                    //@"Images\Products"
                    var ext = Path.GetExtension(file.FileName);
                    if (vm.Product.ImageUrl != null)
                    {
                        var oldimg = Path.Combine(rootpath, vm.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimg))
                        {
                            System.IO.File.Delete(oldimg);
                        }
                    }
                    using (var filestream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    vm.Product.ImageUrl = Path.Combine("Images", "Products", filename + ext);
                }
                var products = _unitOfWork.ProductRepository.GetAll();
            foreach(var item in products)
    {
                if (item.Id == vm.Product.Id)
                {
                    if (item.Name == vm.Product.Name || item.Name != vm.Product.Name)
                    {
                        _unitOfWork.ProductRepository.Update(vm.Product);
                        _unitOfWork.compelete();
                        TempData["Update"] = "Data has been updated successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                else if (item.Name == vm.Product.Name)
                {
                    return BadRequest("Product name must be unique.");
                }
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            ProductVm vm = new ProductVm()
            {
                Product = _unitOfWork.ProductRepository.GetFirstOrDefault(x => x.Id == id),
                CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(x => new SelectListItem
                {

                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(vm);
        }
        public IActionResult Delete(ProductVm vm,int id)
        {
            var prod = _unitOfWork.ProductRepository.GetFirstOrDefault(x => x.Id == vm.Product.Id);

            _unitOfWork.ProductRepository.Remove(prod);
            var oldimg = Path.Combine(_webHostEnvironment.WebRootPath, prod.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldimg))
            {
                System.IO.File.Delete(oldimg);
            }
            _unitOfWork.compelete();
            TempData["Delete"] = "Data has deleted successfully";
            return RedirectToAction(nameof(Index));
           
        }
        //public IActionResult Delete(int id)
        //{
        //    var prod = _unitOfWork.ProductRepository.GetFirstOrDefault(x => x.Id == id);

        //    _unitOfWork.ProductRepository.Remove(prod);
        //    var oldimg = Path.Combine(_webHostEnvironment.WebRootPath, prod.ImageUrl.TrimStart('\\'));
        //    if (System.IO.File.Exists(oldimg))
        //    {
        //        System.IO.File.Delete(oldimg);
        //    }
        //    _unitOfWork.compelete();
        //    TempData["Delete"] = "Data has deleted successfully";
        //    return RedirectToAction(nameof(Index));
        //        //Json(new { success = true, message = "Product Deleted Successfully" });
        //}


    }
}
