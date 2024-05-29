using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dtos;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize]
    public sealed class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ResponseDto _responseDto;
        private readonly IMapper _mapper;

        public ProductAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Product> products = _db.Products.ToList();
                _responseDto.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                var product = _db.Products.First(x => x.ProductId == id);
                _responseDto.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        [Authorize(Roles = SD.RoleAdmin)]
        public ResponseDto Create([FromBody] CreateProductRequestDto product)
        {
            try
            {
                var productObj = _mapper.Map<Product>(product);
                _db.Products.Add(productObj);
                _db.SaveChanges();

                _responseDto.Result = _mapper.Map<ProductDto>(productObj);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpPut]
        [Authorize(Roles = SD.RoleAdmin)]
        public ResponseDto Update([FromBody] ProductDto product)
        {
            try
            {
                var productObj = _mapper.Map<Product>(product);
                _db.Products.Update(productObj);
                _db.SaveChanges();

                _responseDto.Result = _mapper.Map<ProductDto>(productObj);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = SD.RoleAdmin)]
        public ResponseDto Delete(int id)
        {
            try
            {
                var product = _db.Products.First(x => x.ProductId == id);
                _db.Products.Remove(product);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }
    }
}
