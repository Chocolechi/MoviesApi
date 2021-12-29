using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiPelicula.Models;

namespace WebApiPelicula.Repository.IRepository
{
    public interface IMovieRepository
    {
        ICollection<Movie> GetMovies();
        ICollection<Movie> GetMoviesInCategory(int categoryId);

        Movie GetMovie(int movieId);
        bool ExistMovie(string name);
        IEnumerable<Movie> SearchMovie(string name);
        bool ExistMovie(int id);
        bool CreateMovie(Movie movie);
        bool UpdateMovie(Movie movie);
        bool DeleteMovie(Movie movie);
        bool Save();
    }
}
