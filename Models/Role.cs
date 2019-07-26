
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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace MvcMovie.Models
{
    public class Role
    {
        
        public int id { get; set; }

        [Display(Name = "Роль")]
        public string name { get; set; }

        public string Desc1 { get; set; }
        
        public string Desc2 { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

    }
}