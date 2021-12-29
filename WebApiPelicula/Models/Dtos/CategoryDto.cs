using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPelicula.Models.Dtos
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class CategoryDto
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }

    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
