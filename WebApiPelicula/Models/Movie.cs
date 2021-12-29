using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPelicula.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string RutaImagen { get; set; }
        public string Description { get; set; }
        public string Duracion { get; set; }
        public enum TipoClasificacion { Seven, Thirteen, Sixteen, Eighteen }
        public TipoClasificacion Clasificacion { get; set;  }
        public DateTime FechaDeCreacion { get; set; }
        
        
        //To create a relationship between both models category and movies
        public int categoryId { get; set; }
        [ForeignKey("categoryId")]
        public Category Category { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
