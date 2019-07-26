using System.ComponentModel.DataAnnotations;

namespace MvcMovie.Models
{
    public class LoginModel
    {

        [Required(ErrorMessage = "Не вказан Логін")]
        public string login { get; set; }
         
        [Required(ErrorMessage = "Не вказан пароль")]
        [DataType(DataType.Password)]

        //[RegularExpression(@"^[A-Za-z0-9]*")]    
        //(?!^[0-9]*$)(?!^[a-zA-Z]*$)^(.{8,15})
        public int LoginPassword { get; set; } //Change Password to string!!!!!
        

    }
}