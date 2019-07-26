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
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
namespace MvcMovie.Components
{
    public class NotificationViewComponent
    {
        private  DataContext db;
        public bool userIsAdmin=false;
        public NotificationViewComponent(DataContext dataContext)
        {
            db=dataContext;

        }
        public string Invoke()
        {
            
             return GetDocsToExecute();
        }

        public   String GetDocsToExecute()
        {    
            var DocsToChecked=db.Documents
            .Where(x=>x.DocChecked==false)
            .Where(e=>e.Published==true)
            .Include(m=>m.Category)
            .Include(t=>t.User)
            .Count();

            return DocsToChecked.ToString();
           
        }

       
    }

        
    }
