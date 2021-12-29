using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPelicula.Models.Dtos
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class UserAuthLoginDto
    {
        [Required(ErrorMessage = "The username is required")]
        public string User { get; set; }
        [Required(ErrorMessage = "The password is required")]
        public string Password { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
