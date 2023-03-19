using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Features.Commands.Product.CreateProduct;
using ETicaretAPI.Application.Features.Commands.Product.RemoveProduct;
using ETicaretAPI.Application.Features.Commands.Product.UpdateProduct;
using ETicaretAPI.Application.Features.Commands.ProductImageFile.ChangeShowCase;
using ETicaretAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ETicaretAPI.Application.Features.Queries.Product.GetAllProduct;
using ETicaretAPI.Application.Features.Queries.Product.GetByIdProduct;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.Repositories.Products;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistince.Repositories.Concrete.Products;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        readonly IStorageService _storageService;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IConfiguration configuration;


        readonly IMediator _mediator;

        public ProductController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IWebHostEnvironment webHostEnvironment, IStorageService storageService, IProductImageFileWriteRepository productImageFileWriteRepository, IMediator mediator, IConfiguration configuration)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;
            _storageService = storageService;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _mediator = mediator;
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {
            GetAllProductQueryResponse response =  await _mediator.Send(getAllProductQueryRequest);

            return Ok(response);
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetId([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest)
        {
            GetByIdProductQueryResponse response = await _mediator.Send(getByIdProductQueryRequest);

            return Ok(response);

        }
        [Authorize(AuthenticationSchemes = "Admin")]
        [HttpPost]

        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse createProductCommandResponse = await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }
        [Authorize(AuthenticationSchemes = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse updateProductCommandResponse = await _mediator.Send(updateProductCommandRequest);
            return Ok();
        }
        [Authorize(AuthenticationSchemes = "Admin")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] RemoveProductCommandRequest removeProductCommandRequest)
        {
            RemoveProductCommandResponse removeProductCommandResponse = await _mediator.Send(removeProductCommandRequest);
            return Ok();
        }
        [Authorize(AuthenticationSchemes = "Admin")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {
            uploadProductImageCommandRequest.Files = Request.Form.Files;
            UploadProductImageCommandResponse uploadProductImageCommandResponse = await _mediator.Send(uploadProductImageCommandRequest);
            return Ok();
        }
        [Authorize(AuthenticationSchemes = "Admin")]
        [HttpDelete("[action]/{Id}")]

        public async Task<IActionResult> DeleteProductImage([FromRoute] 
        RemoveProductImageCommandRequest productImageCommandRequest, [FromQuery] string imageId)
        {
            productImageCommandRequest.ImageId = imageId;
            RemoveProductImageCommandResponse removeProductImageCommand = await _mediator.Send(productImageCommandRequest);
            return Ok();
        }
        [Authorize(AuthenticationSchemes = "Admin")]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductImages(string id)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));


            return Ok(product.ProductImageFiles.Select(p => new
            {
                Id = p.Id,
                Path = $"{ configuration["BaseStorageUrl"]}{p.FileName}",
                p.FileName,
            }));
        }
        [Authorize(AuthenticationSchemes = "Admin")]
        [HttpGet("[action]/{id}/{imageId}")]
        public async Task<IActionResult> DeleteProductImage(string id, string imageId)
        {
            Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles)
                .FirstOrDefaultAsync(p => p.Id == Guid.Parse(id));


            ProductImageFile productImageFile = product.ProductImageFiles.FirstOrDefault(p => p.Id == Guid.Parse(imageId));

            product.ProductImageFiles.Remove(productImageFile);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
        [Authorize(AuthenticationSchemes = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> ChangeShowCaseImage([FromQuery] 
        ChangeShowCaseImageCommandRequest changeShowCaseImageCommandRequest)
        {
            ChangeShowCaseImageCommandRespnse response = await _mediator.Send(changeShowCaseImageCommandRequest);
            return Ok(response);
        }

    }
}
