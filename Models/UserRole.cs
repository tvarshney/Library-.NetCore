
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
using System.ComponentModel.DataAnnotations.Schema;
namespace MvcMovie.Models
{

    public class UserRole
    {
        [Key, Required]
        public int Id { get; set; }
 
        [Required,ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
 
        [Required, ForeignKey(nameof(Role))]
        public int RoleId { get; set; }
        public Role Role { get; set; }
 
        //public virtual Role Role { get; set; }
        //public virtual User User { get; set; }

        // public Role Role { get; set; }
        // public User User { get; set; }


    }
}