using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiPelicula.Models;
using WebApiPelicula.Models.Dtos;
using WebApiPelicula.Repository.IRepository;

namespace WebApiPelicula.Controllers
{
    [Authorize]
    [Route("api/Users")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserRepository _uRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public UsersController(IUserRepository uRepo, IMapper mapper, IConfiguration config)
        {
            _uRepo = uRepo;
            _mapper = mapper;
            _config = config;
        }
        /// <summary>
        /// To get All Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUsers()
        {
            var listUsers = _uRepo.GetUsers();
            var listUsersDto = new List<UserDto>();

            foreach (var lista in listUsers)
            {
                listUsersDto.Add(_mapper.Map<UserDto>(lista));
            }
            return Ok(listUsersDto);
        }
        /// <summary>
        /// To Get a User by its id 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{userId:int}", Name = "GetUser")]
        public IActionResult GetUser(int userId)
        {
            var itemUser = _uRepo.GetUser(userId);

            if (itemUser == null)
            {
                return NotFound();
            }
            var itemUserDto = _mapper.Map<UserDto>(itemUser);
            return Ok(itemUserDto);
        }
        /// <summary>
        /// To Register a new User
        /// </summary>
        /// <param name="userAuthDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserAuthDto userAuthDto)
        {
            userAuthDto.User = userAuthDto.User.ToLower();
            if (_uRepo.ExistUser(userAuthDto.User))
            {
                return BadRequest("The user alredy exists");
            }
            var userToCreate = new User
            {
                UserA = userAuthDto.User
            };

            var createdUser = _uRepo.Register(userToCreate, userAuthDto.Password);
            return Ok(createdUser);
        }
        /// <summary>
        /// To log in
        /// </summary>
        /// <param name="userAuthLogin"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserAuthLoginDto userAuthLogin)
        {
            var userFromRepo = _uRepo.Login(userAuthLogin.User, userAuthLogin.Password);

            if (userFromRepo == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.UserA.ToString())
            };

            // token generator
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}
