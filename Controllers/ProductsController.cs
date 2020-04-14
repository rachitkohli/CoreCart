using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreCart.Models;
using CoreCart.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace CoreCart.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager { get; set; }
        private IHostingEnvironment _hostingEnvironment { get; }

        public ProductsController(ApplicationDbContext context, 
            UserManager<ApplicationUser> UserManager,
            IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = UserManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(User);
            return View(_context.Products.Where(j=>j.UserId == userId).ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateNoteViewModel product)
        {
            string uniqueFileName = "";
            string filePath = "";
            string userId = _userManager.GetUserId(User);
            product.UserId = userId;
            if (ModelState.IsValid)
            {
               var category =  _context.Category.Where(a => a.Name == product.Category).FirstOrDefault();
                if (category==null)
                {
                    ModelState.AddModelError("Category", "Category is not allowed");
                    return View(product);
                }
                if (product.File!=null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(product.File.FileName);
                    filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    product.File.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                                
            }
            Product newProduct = new Product
            {
                UserId = product.UserId,
                Category = product.Category,
                Title = product.Title,
                Price = product.Price,
                Description = product.Description,
                FilePath = uniqueFileName
            };
            _context.Add(newProduct);
            _context.SaveChanges();
            return View(product);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id!=null)
            {
                var prod = _context.Products.Where(j => j.Id == id).FirstOrDefault();
                return View(prod);
            }    
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            _context.Update(product);
            _context.SaveChanges();
            return View(product);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var prod = _context.Products.Where(n => n.Id == id).FirstOrDefault();
                return View(prod);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var prod = _context.Products.Where(n => n.Id == id).FirstOrDefault();
            _context.Remove(prod);
            _context.SaveChanges();
            return Redirect("/Products/Index");
        }

        public IActionResult Details(int id)
        {
            var prod = _context.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(prod);
        }
    }
}