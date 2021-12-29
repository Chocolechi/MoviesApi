using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPelicula.Models.Dtos
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class UserAuthDto
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="The username is required")]
        public string User { get; set; }
        [Required(ErrorMessage ="The password is required")]
        [StringLength(10, MinimumLength = 4,ErrorMessage = "The password must be between a minimum of 4 and a maximum of 10 characters")]
        public string Password { get; set; }

    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
