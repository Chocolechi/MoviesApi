using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPelicula.Models.Dtos
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class UserDto
    {
        public string UserA { get; set; }
        public byte[] PasswordHash { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
