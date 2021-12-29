using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static WebApiPelicula.Models.Movie;

namespace WebApiPelicula.Models.Dtos
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class MovieDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string RutaImagen { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Duracion { get; set; }
        public TipoClasificacion Clasificacion { get; set; }      

        //To create a relationship between both models category and movies
        public int categoryId { get; set; }
        public Category Category { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
