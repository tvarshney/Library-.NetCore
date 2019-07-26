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
//using MySql.Data.EntityFrameworkCore.DataAnnotations;
namespace MvcMovie.Models
{
    public class User
    {
        public int id { get; set; }
        
       // [MySqlCharset("utf8")]
        [Display(Name = "Імя")]
        [Required(ErrorMessage = "Не вказано ім'я")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string name { get; set; }

        [Display(Name = "Прізвище")]
        [Required(ErrorMessage = "Не вказано прізвище")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string surName { get; set; }

        [Display(Name = "По-батькові")]
        [Required(ErrorMessage = "Не вказано по-батькові")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string middleName { get; set; }

        [Display(Name = "Логін")]        
        public string login { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Не вказан пароль")]    

        //[RegularExpression(@"^[A-Za-z0-9]*")]    
        //(?!^[0-9]*$)(?!^[a-zA-Z]*$)^(.{8,15})
        public int password { get; set; }

         [Display(Name = "Виберіть роль")]  

         public int Rolesid { get; set; }

         [Display(Name = "Додаткові данні")]  

         public string Desc { get; set; }

          public Role Roles { get; set; }

         public ICollection<LibDocument> Documents { get; set; }

          public ICollection<UserRole> UserRoles { get; set; }
          
         
         
    }
    
}