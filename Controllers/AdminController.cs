using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
//using MySql.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using MySql.Data.EntityFrameworkCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Text.Encodings;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;



namespace MvcMovie.Controllers
{
    //   [Route("admin")]
    
    public class AdminController : Controller
    {
        

        IHostingEnvironment _appEnvironment;
        private DataContext db;



        public AdminController(DataContext context, IHostingEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
            
           
        }

[Authorize(Roles = "Publisher,Admin,Director,Manager")]
        public IActionResult Index()
        {
            
            //string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

            //var users = db.Users.Include(r => r.UserRoles).Include(x => x.Roles).ToList();
            //int[] array = { 1, 2, 3, 4, 5 };
            //ViewData["ID"] = id;
            //ViewBag.Array = array.ToList();
            ViewBag.TotalUsers=db.Users.Count();
            ViewBag.TotalDocuments=db.Documents.Count();
            ViewBag.TotalCategories=db.Categories.Count();


            //search.InitializeSearch();

            return View();
        }


        [Authorize(Roles = "Publisher,Admin,Director,Manager")]
        public IActionResult AddFile()
        {
            List<Category> categories = db.Categories.ToList();

            // categories.Insert(0, new Category { Name = "Все", Id = 0 });
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View();
        }

        [Authorize(Roles = "Publisher,Admin,Director,Manager")]
        [HttpPost]
        //[RequestSizeLimit(40000000)]
        //public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        public async Task<IActionResult> AddFile(AddFileModelView file)
        {
           
            //var s=User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (file.uploadedFile != null)
            {
                string ext=Path.GetExtension(file.uploadedFile.FileName).ToLowerInvariant();
                var types=GetMimeTypes();
                string newName=DateTime.Now.ToString("M-d-yyyy_hh-mm-ss")+ext;
                string path ="";
                string MonthNumber=DateTime.Now.Month.ToString();
                string YearNumber=DateTime.Now.Year.ToString();

                DirectoryInfo directory=new DirectoryInfo(_appEnvironment.WebRootPath+@"\Files\"+YearNumber+@"\"+MonthNumber);
                if(directory.Exists)
                {
                   path = "\\Files\\" +YearNumber+@"\"+MonthNumber+@"\"+newName;
                }
                else
                {
                    directory.Create();
                    path = "\\Files\\" +YearNumber+@"\"+MonthNumber+@"\"+newName;

                }
                //DirectoryInfo directory=new DirectoryInfo("/Files/"); //Linux
                // путь к папке Files
                 //path = "\\Files\\" + newName;

                
                //string s=pDF.ReadPdfFile(path);
                // сохраняем файл в папку Files в каталоге wwwroot

                // Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                //                             Encoding utf8 = Encoding.GetEncoding("utf-8");
                //                             Encoding win1251 = Encoding.GetEncoding("windows-1251");
                //                             byte[] utf8Bytes = win1251.GetBytes(pDF.ReadPdfFile(path));
                //                             byte[] win1251Bytes = Encoding.Convert(win1251, utf8, utf8Bytes);
                //                             string currentText = utf8.GetString(win1251Bytes);



                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await file.uploadedFile.CopyToAsync(fileStream);
                }
                string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
                LibDocument  newfile = new LibDocument();

                switch (ext)
                {
                    case ".pdf":
                    

                    //PDFParser pDF = new PDFParser(_appEnvironment);
                   //Task<string> pdfText=Task.Factory.StartNew(()=>pDF.ReadPdfFile(path));
                        newfile = new LibDocument {
                        Name = file.Name,
                        Path = path,
                        Desc1 = file.Desc1,
                        Desc2 = "",//pDF.ReadPdfFile(path)
                        UserId=Int32.Parse(userId),
                        CategoryId=file.categories,
                        DbName=newName,
                        Published=file.Published,
                        AccessLinkOnly=file.AccessLinkOnly,
                        ContentType=types[ext]
                            };
                    break;
                    
                    case ".mp4":
                        newfile = new LibDocument {
                     Name = file.Name,
                      Path = path,
                      Desc1 = file.Desc1,
                       Desc2 = "",
                       UserId=Int32.Parse(userId),
                       CategoryId=file.categories,
                       DbName=newName,
                       Published=file.Published,
                       AccessLinkOnly=file.AccessLinkOnly,
                       ContentType=types[ext]
                        };
                    break;

                    case ".doc":
                    case ".docx":
                    DocxParser docx=new DocxParser(_appEnvironment);
                            newfile = new LibDocument {
                        Name = file.Name,
                        Path = path,
                        Desc1 = file.Desc1,
                        Desc2 = docx.GetText(path),
                        UserId=Int32.Parse(userId),
                        CategoryId=file.categories,
                        DbName=newName,
                        Published=file.Published,
                        AccessLinkOnly=file.AccessLinkOnly,
                        ContentType=types[ext]
                            };
                    break;

                    // case ".xls":
                    // case ".xlsx":
                    //         newfile = new LibDocument {
                    //     Name = file.Name,
                    //     Path = path,
                    //     Desc1 = file.Desc1,
                    //     Desc2 = "",
                    //     UserId=Int32.Parse(userId),
                    //     CategoryId=file.categories,
                    //     DbName=newName,
                    //     Published=file.Published,
                    //     AccessLinkOnly=file.AccessLinkOnly,
                    //     ContentType=types[ext]
                    //         };
                    // break;


                }


                // if(ext==".pdf")
                // {
                //    newfile = new LibDocument {
                //      Name = file.Name,
                //       Path = path,
                //       Desc1 = file.Desc1,
                //        Desc2 = pDF.ReadPdfFile(path),
                //        UserId=Int32.Parse(userId),
                //        CategoryId=file.categories,
                //        DbName=newName,
                //        Published=file.Published,
                //        AccessLinkOnly=file.AccessLinkOnly,
                //        ContentType=types[ext]
                //         };
                // }
                // else if(ext==".mp4")
                // {
                //    newfile = new LibDocument {
                //      Name = file.Name,
                //       Path = path,
                //       Desc1 = file.Desc1,
                //        Desc2 = "",
                //        UserId=Int32.Parse(userId),
                //        CategoryId=file.categories,
                //        DbName=newName,
                //        Published=file.Published,
                //        AccessLinkOnly=file.AccessLinkOnly,
                //        ContentType=types[ext]
                //         };
                // }
                
                                    db.Documents.Add(newfile);
                                    
                                    await db.SaveChangesAsync();
                                    await Task.Factory.StartNew(()=>InsertPdfTextToDbAsync(newfile.id,path));
                                    //int id =file.id;
                                                                       

                                   //AddToIndexDoc(newfile.id);

                //db.Entry(file).Reload()//Encoding.UTF8

                List<Category> categories = db.Categories.ToList();

                // categories.Insert(0, new Category { Name = "Все", Id = 0 });
                ViewBag.Categories = new SelectList(categories, "Id", "Name");

            }

            return RedirectToAction("MyFiles","File");
        }

        private  void InsertPdfTextToDbAsync(int id,string path)
        {
            var record=db.Documents.Where(rec=>rec.id==id).FirstOrDefault();
            PDFParser pDF = new PDFParser(_appEnvironment);
            //string pdfText= await Task<string>.Run(()=>pDF.ReadPdfFile(path));
            Task<string> task=Task<string>.Factory.StartNew(pDF.ReadPdfFile, path);
            task.Wait();
            record.Desc2=task.Result;
            //record.Desc2=pDF.ReadPdfFile(path);
            AddToIndexDocAsync(id);
            db.SaveChanges();
        }

       


        private async void AddToIndexDocAsync(int id)
        {
            var doc = await db.Documents.FindAsync(id);
            Search index = new Search();
            index.createIndex(doc);
        }

        [Authorize(Roles = "Publisher,Admin,Director,Manager")]
        public string Search(string field = "")
        {
           // string field1 = field.Replace("(!)", "");
            if (field != "")
            {
                Search search = new Search();
                var arr = search.searchfield(field);
                if (arr != null)
                {
                    return "Seacrh compleated";
                }
            }
            return "No records";
        }


[Authorize(Roles = "Publisher,Admin,Director")]
        public IActionResult ExecutorList()
        {
            var DocsToChecked=db.Documents
            .Where(x=>x.DocChecked==false)
            .Where(e=>e.Published==true)
            .Include(m=>m.Category)
            .Include(t=>t.User)
            .ToList();

            ViewBag.DocsToCheckedCount=DocsToChecked.Count();         

            return View(DocsToChecked);
        }

        [Authorize(Roles = "Publisher,Admin,Director")]
        [HttpPost]
            public IActionResult executorList(int docId, bool DocChecked)
        {
            string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            var doc=db.Documents
            .Where(x=>x.id==docId).FirstOrDefault();
            doc.DocChecked=DocChecked;
            doc.ExecutorId=Int32.Parse(userId);
            db.SaveChanges();
            

            return RedirectToAction("ExecutorList");
        }


        
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/msword"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".mp4","video/mp4"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
















    }
}