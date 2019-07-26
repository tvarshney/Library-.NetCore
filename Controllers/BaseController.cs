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
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using System.Text.RegularExpressions;

namespace MvcMovie.Controllers
{
    public class BaseController:Controller
    {
        public DataContext db;

        public BaseController(DataContext context)
        {
            db=context;
            Menu Menu = new Menu();
            Menu.db = db;
            HtmlString MenuModule = Menu.GetMenuItems();
            ViewBag.Menu = MenuModule;
        }

        
    }
}