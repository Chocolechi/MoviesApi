using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using WebApiPelicula.Models;
using WebApiPelicula.Models.Dtos;
using WebApiPelicula.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace WebApiPelicula.Controllers
{
    [Authorize]
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : Controller
    {
        private readonly IMovieRepository _mvRepo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        public MoviesController(IMovieRepository mvRepo, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _mvRepo = mvRepo;
            _mapper = mapper;
            _hostingEnvironment = hostEnvironment;
        }
        /// <summary>
        /// To Get All Movies
        /// </summary>
        /// <returns></returns>
        ///
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<MovieDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetMovies()
        {
            var listMovies = _mvRepo.GetMovies();
            var listMoviesDto = new List<MovieDto>();

            foreach (var lista in listMovies)
            {
                listMoviesDto.Add(_mapper.Map<MovieDto>(lista));
            }
            return Ok(listMoviesDto);
        }
        /// <summary>
        /// To Get a Movie
        /// </summary>
        /// <param name="movieId">Movie Id</param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpGet("{movieId:int}", Name = "GetMovie")]
        [ProducesResponseType(200, Type = typeof(List<MovieDto>))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetMovie(int movieId)
        {
            var itemMovie = _mvRepo.GetMovie(movieId);

            if (itemMovie == null)
            {
                return NotFound();
            }
            var itemMovieDto = _mapper.Map<MovieDto>(itemMovie);
            return Ok(itemMovieDto);
        }
        /// <summary>
        /// To get the movies by the category
        /// </summary>
        /// <param name="categoryId">Category Id</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetMovieInCategory/{categoryId:int}")]
        public IActionResult GetMovieInCategory(int categoryId)
        {
            var listMovie = _mvRepo.GetMoviesInCategory(categoryId);
            if (listMovie == null)
            {
                return NotFound();
            }

            var itemMovie = new List<MovieDto>();
            foreach (var item in listMovie)
            {
                itemMovie.Add(_mapper.Map<MovieDto>(item));
            }
            return Ok(itemMovie);
        }
        /// <summary>
        /// To get a Movie by its name
        /// </summary>
        /// <param name="name">Movie's Name</param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpGet("Search")]
        public IActionResult Search(string name)
        {
            try
            {
                var result = _mvRepo.SearchMovie(name);
                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "retrieving data...");
            }
        }
        /// <summary>
        /// To create a Movie
        /// </summary>
        /// <param name="createMovieDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(List<MovieDto>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateMovie([FromForm] CreateMoviesDto createMovieDto)
        {
            if (createMovieDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_mvRepo.ExistMovie(createMovieDto.Name))
            {
                ModelState.AddModelError("", "La pelicula ya existe :c");
                return StatusCode(404, ModelState);
            }

            /*Subida de archivos */
            var file = createMovieDto.Photo;
            string firstRoute = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (file.Length > 0)
            {
                // New image
                var photoName = Guid.NewGuid().ToString();
                var upload = Path.Combine(firstRoute, @"photos");
                var extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(upload, photoName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                createMovieDto.RutaImagen = @"/photos" + photoName + extension;
            }

            var movie = _mapper.Map<Movie>(createMovieDto);
            if (!_mvRepo.CreateMovie(movie))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro {movie.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetMovie", new { movieId = movie.Id }, movie);
        }
        /// <summary>
        /// To update a movie
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="movieDto"></param>
        /// <returns></returns>
        [HttpPatch("{movieId:int}", Name = "UpdateMovie")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateMovie(int movieId, [FromBody] UpdateMovieDto movieDto)
        {
            if (movieDto == null || movieId != movieDto.Id)
            {
                return BadRequest(ModelState);
            }

            var movie = _mapper.Map<Movie>(movieDto);

            if (!_mvRepo.UpdateMovie(movie))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {movie.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        /// <summary>
        /// To delete a movie
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        [HttpDelete("{movieId:int}", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteMovie(int movieId)
        {

            if (!_mvRepo.ExistMovie(movieId))
            {
                return NotFound();
            }

            var movie = _mvRepo.GetMovie(movieId);

            if (!_mvRepo.DeleteMovie(movie))
            {
                ModelState.AddModelError("", $"Algo salió mal eliminando el registro {movie.Name}");
                return StatusCode(500, ModelState);
            }


            return NoContent();
        }
    }
}
