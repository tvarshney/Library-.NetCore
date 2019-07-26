
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



namespace MvcMovie.Models
{
    public class Category
    {
        
        public int Id { get; set; }

        [Display(Name = "Назва категорії")]  
        [StringLength(28, MinimumLength = 3, ErrorMessage = "Довжина назви категорії за велика!")]
        public string Name { get; set; }

        [Display(Name = "Опис категорії")]  
        public string Desc1 { get; set; }
        public int parent_id { get; set; }
        public string Desc2 { get; set; }


    }
    
}

