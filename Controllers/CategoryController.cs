using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using Microsoft.AspNetCore.Html;
//using MySql.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using MySql.Data.EntityFrameworkCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
//using MySql.Data.EntityFrameworkCore;
//using MySql.Data.EntityFrameworkCore.Extensions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Encodings;

using Microsoft.AspNetCore.Identity;
using System.Security.Principal;

namespace MvcMovie.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private DataContext db;
         private  IHostingEnvironment _appEnvironment;

        public CategoryController(DataContext context,IHostingEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }

        [Authorize(Roles = "Admin,Director")]
        public IActionResult CategoryList()
        {
            Menu Menu = new Menu();
            Menu.db = db;
            if(User.IsInRole("Admin")||User.IsInRole("Director"))
            {
            Menu.userIsAdmin = true;
            }
            // Menu m = new Menu();
            // ViewBag.Menu=m.GetMenuItems();
            HtmlString MenuModule = Menu.GetMenuItems();
            ViewBag.Menu = MenuModule;
            return View();
        }

          [Authorize(Roles = "Admin,Director")]
        [HttpGet]
        public IActionResult addCategory()
        {
           List<Category> categories = db.Categories.ToList();

            categories.Insert(0, new Category { Name = "Головний пункт меню", Id = 0 });
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.Categories1=categories;
            return View();
        }


          [Authorize(Roles = "Admin,Director")]
        [HttpPost]
        public IActionResult AddCategory(AddCategoryModel category)
        {
           
            if (ModelState.IsValid)
            {
                Category cat = new Category();
                cat.Name = category.Name;
                cat.parent_id = category.categories;
                cat.Desc1 = category.Desc1;
                db.Categories.Add(cat);
                db.SaveChanges();
                return RedirectToAction("CategoryList");
            }
           List<Category> categories = db.Categories.ToList();

            categories.Insert(0, new Category { Name = "Головний пункт меню", Id = 0 });
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(category);
        }

         //[AllowAnonymous]
         [Authorize]
        public async Task<IActionResult> Show(int id,int page=1)
        {
            
             int pageSize = 5; // количество элементов на странице
            IQueryable<LibDocument> source = db.Documents
            .Where(x=>x.CategoryId==id)
            .Where(r=>r.DocChecked==true)
            .Where(r=>r.AccessLinkOnly==false)
            .Include(r=>r.Category);

            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
 
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            CategoriesViewModel viewModel = new CategoriesViewModel
            {
                PageViewModel = pageViewModel,
                Documents =items
            };
            return View(viewModel);
        }
            //var list=db.Documents.Where(x=>x.CategoryId==id).ToList();
            //return View(list);
        //}

          [Authorize(Roles = "Admin,Director")]
        public IActionResult EditCategory(int id)
        {
            List<Category> categories = db.Categories.ToList();

            categories.Insert(0, new Category { Name = "Головний пункт меню", Id = 0 });
            categories.Remove(categories.Find(c=>c.Id==id));
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            var category = db.Categories.Where(c => c.Id == id).SingleOrDefault();
            
            if (category != null)
            {
                //db.Categories.Remove(categoryToDelete);
                // db.SaveChanges();
            }

            return View(category);
        }

          [Authorize(Roles = "Admin,Director")]
         [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            var categories = db.Categories.ToList();
            
            ViewBag.Categories = new SelectList(categories, "id", "name");

            if (ModelState.IsValid)
            {
                var cat = db.Categories.Where(e=>e.Id==category.Id).FirstOrDefault();
                cat.Name = category.Name;
                cat.parent_id = category.parent_id;
                cat.Desc1 = category.Desc1;
                //db.Categories.Add(cat);
                db.SaveChanges();
                return RedirectToAction("CategoryList");
            }


            return View();
        }

          [Authorize(Roles = "Admin,Director")]
        public IActionResult DeleteCategory(int id)
        {
            DeleteCategoryRecurse(id);
            //ViewBag.Categories = new SelectList(categories, "id", "name");
            return RedirectToAction("CategoryList","Category");

        }

        public void DeleteCategoryRecurse(int id)
        {
            var categoryToDelete = db.Categories.Where(c => c.Id == id).SingleOrDefault();
            var ChildCategories=db.Categories.Where(c=>c.parent_id==id).ToList();
            var files=db.Documents.Where(s=>s.CategoryId==id).ToList();

            if (categoryToDelete != null)
            {
                if(files.Count>0)
                {
                    foreach(var file in files)
                    DeleteFileFromCategory(file.id);
                }

               if(ChildCategories.Count>0)
                {
                   foreach(var child in ChildCategories)
                   {
                       DeleteCategoryRecurse(child.Id);
                   }
                }
                
             db.Categories.Remove(categoryToDelete);
             db.SaveChanges();
            }
        }

        private  void DeleteFileFromCategory(int id)
        {
            LibDocument doc=db.Documents.Find(id);
            System.IO.FileInfo di = new FileInfo(_appEnvironment.WebRootPath+doc.Path);
           
           string webRootPath = _appEnvironment.WebRootPath;
            string contentRootPath = _appEnvironment.ContentRootPath;
         
            if(di.Exists)
            {
            di.Delete();
            }

            Search index = new Search();
            index.DeleteFromIndex(doc);

            db.Documents.Remove(doc);
           db.SaveChanges();

        }
    }
}