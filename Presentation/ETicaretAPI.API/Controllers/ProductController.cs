using ETicaretAPI.Application.Repositories.Products;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistince.Repositories.Concrete.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;

        public ProductController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok("Mehmet");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetId(string id)
        {
            Product product = await _productReadRepository.GetByIdAsync(id);
            return Ok(product);

        }
    }
}
