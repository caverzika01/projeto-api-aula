using _2TDSPK.API.DTO.Request;
using _2TDSPK.API.Extensions;
using _2TDSPK.API.Service;
using _2TDSPK.Database.Models;
using _2TDSPK.Repository;
using _2TDSPK.Repository.Interface;
using _2TDSPK.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;

namespace _2TDSPK.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Tags("Cadastro de Usuário")]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _userRepository; 
        private readonly UserService _userService;
        private readonly ProductService _productService;

        public UserController(IRepository<User> userRepository, UserService userService, ProductService productService)
        {
            _userRepository = userRepository;
            _userService = userService;
            _productService = productService;
        }


        /// <summary>
        /// Endpoint responsavel por inserir um usuário
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult Post([FromBody] UserRequest userRequest)
        {
            try
            {
                var x = userRequest.Email.ToFIAP();


                _userService.CreateUser(userRequest);
                return Created();

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Cadastro de like em um determinado produto
        /// </summary>
        /// <param name="userLike"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("likeuser")]
        public async Task<IActionResult> PostUserLikeAsync([FromBody] UserLike userLike)
        {
            _userService.UserLike(userLike);

            //_recommendationEngine.TrainModel(_userLikeRepository.GetAll());

            return Created();
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetRecommendations(string userId)
        {
            if (!ObjectId.TryParse(userId, out ObjectId parsedUserId))
            {
                return BadRequest("Invalid user ID");
            }

            return Ok(_productService.RecommendedProducts(parsedUserId));
        }


        /// <summary>
        /// Endpoint responsavel por listar todos os usuarios
        /// </summary>
        /// <remarks>
        /// Exemplo de Solicitação
        /// 
        ///     GET /users
        /// 
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<User>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]

        public IActionResult GetAll()
        {
            var users = _userRepository.GetAll();
            return Ok(users);
        }


        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult Patch([FromBody] User user)
        {
            _userRepository.Update(user);
            return Ok();
        }


        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult Delete([FromBody] User user)
        {
            _userRepository.Delete(user);
            return Created();
        }
    }
}
