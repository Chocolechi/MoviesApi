using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApiPelicula.Models;
using WebApiPelicula.Models.Dtos;
using WebApiPelicula.Repository.IRepository;

namespace WebApiPelicula.Controllers
{
    [Authorize]
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _ctRepo;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoryRepository ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;
        }
        /// <summary>
        /// To get all categories
        /// </summary>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CategoryDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategories()
        {
            var listCategories = _ctRepo.GetCategories();
            var listCategoriesDto = new List<CategoryDto>();

            foreach (var lista in listCategories)
            {
                listCategoriesDto.Add(_mapper.Map<CategoryDto>(lista));
            }
            return Ok(listCategoriesDto);
        }
        /// <summary>
        /// To get a category
        /// </summary>
        /// <param name="categoryId"> the category id </param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpGet("{categoryId:int}", Name = "GetCategory")]
        [ProducesResponseType(200, Type = typeof(List<CategoryDto>))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetCategory(int categoryId)
        {
            var itemCategory = _ctRepo.GetCategory(categoryId);

            if (itemCategory == null)
            {
                return NotFound();
            }
            var itemCategoryDto = _mapper.Map<CategoryDto>(itemCategory);
            return Ok(itemCategoryDto);
        }
        /// <summary>
        /// To create a new category
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(List<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_ctRepo.ExistCategory(categoryDto.Nombre))
            {
                ModelState.AddModelError("", "La categoria ya existe :c");
                return StatusCode(404, ModelState);
            }

            var category = _mapper.Map<Category>(categoryDto);

            if (!_ctRepo.CreateCategory(category))      
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro {category.Nombre}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCategory", new { categoryId = category.Id }, category);
        }
        /// <summary>
        /// To update a category that alredy exists
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        [HttpPatch("{categoryId:int}", Name = "UpdateCategory")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null || categoryId != categoryDto.Id)
            {
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(categoryDto);

            if (!_ctRepo.UpdateCategory(category))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {category.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        /// <summary>
        /// To delete a category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpDelete("{categoryId:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategory(int categoryId)
        {

            if (!_ctRepo.ExistCategory(categoryId))
            {
                return NotFound();
            }

            var category = _ctRepo.GetCategory(categoryId);

            if (!_ctRepo.DeleteCategory(category))
            {
                ModelState.AddModelError("", $"Algo salió mal eliminando el registro {category.Nombre}");
                return StatusCode(500, ModelState);
            }


            return NoContent();
        }
    }
}
