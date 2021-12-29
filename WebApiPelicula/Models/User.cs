using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPelicula.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserA { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
