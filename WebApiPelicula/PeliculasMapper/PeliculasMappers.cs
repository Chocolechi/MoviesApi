using AutoMapper;
using WebApiPelicula.Models;
using WebApiPelicula.Models.Dtos;

namespace WebApiPelicula.PeliculasMapper
{
    public class PeliculasMappers : Profile
    {
        public PeliculasMappers()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<Movie, CreateMoviesDto>().ReverseMap();
            CreateMap<Movie, UpdateMovieDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            //CreateMap<User, UserAuthDto>().ReverseMap();
            //CreateMap<User, UserAuthLoginDto>().ReverseMap();
        }
    }
}
