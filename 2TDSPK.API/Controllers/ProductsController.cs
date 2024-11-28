using _2TDSPK.API.DTO.Request;
using _2TDSPK.Database.Models;
using _2TDSPK.Repository.Interface;
using _2TDSPK.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace _2TDSPK.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Tags("Cadastro de Produtos")]
    
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Endpoint responsavel por inserir um usuário
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult Post([FromBody] Product product)
        {
            try
            {
                _productRepository.Add(product);

                return Created();

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


    }
}
