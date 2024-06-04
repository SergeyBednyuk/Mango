using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/shoppingcart")]
    [ApiController]
    public sealed class ShoppingCartAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private IMapper _mapper;
        private readonly ResponseDto _responseDto;
        private readonly IProductService _productService;

        public ShoppingCartAPIController(AppDbContext db, IMapper mapper, IProductService productService)
        {
            _db = db;
            _mapper = mapper;
            _responseDto = new ResponseDto();
            _productService = productService;
        }

        [HttpGet]
        [Route("{userId:int}")]
        public async Task<ResponseDto> Get(int userId)
        {
            try
            {
                var cartHeader = await _db.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId.ToString());
                if (cartHeader != null) 
                {
                    var cartDetails = await _db.CartDetails.Where(x => x.CartHeaderId == cartHeader.CartHeaderId).ToListAsync();
                    var cartToReturn = new CartDto()
                    {
                        CartHeader = _mapper.Map<CartHeaderDto>(cartHeader),
                        CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(cartDetails)
                    };

                    var products = await _productService.GetProductsAsync();

                    foreach (var cart in cartToReturn.CartDetails)
                    {
                        //TODO is it best practice?
                        cart.Product = products.FirstOrDefault(x => x.ProductId == cart.ProductId);
                        cart.CartHeader.CartTotal += cart.Count * cart.Product.Price;
                    }
                    _responseDto.Result = cartToReturn;
                }
            }
            catch(Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost("cartupsert")]
        public async Task<ResponseDto> Cartupsert(CartDto cart)
        {
            try
            {
                var cartHeader = await _db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == cart.CartHeader.UserId);
                if (cartHeader == null)
                {
                    //create new header
                    var newCartHeader = _mapper.Map<CartHeader>(cart.CartHeader);
                    _db.CartHeaders.Add(newCartHeader);
                    await _db.SaveChangesAsync();

                    if (cart.CartDetails != null)
                    {
                        var newCartDetails = _mapper.Map<IEnumerable<CartDetails>>(cart.CartDetails);
                        if (newCartDetails.Count() > 0)
                        {
                            foreach (var item in newCartDetails)
                            {
                                item.CartHeaderId = newCartHeader.CartHeaderId;
                            }
                            _db.CartDetails.AddRange(newCartDetails);
                        }
                    }
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //TODO investigation for best way to update details
                    //var cartDetail = await _db.CartDetails.FirstOrDefaultAsync(x => x.CartHeaderId == cartHeader.CartHeaderId
                    //                                              && cart.CartDetails.Select(x => x.ProductId).Contains(x.ProductId));
                    //var cartDetails = await _db.CartDetails.Where(x => x.CartHeaderId == cartHeader.CartHeaderId).ToListAsync();
                    //if (cartDetails == null && cartDetails.Select(x => x.ProductId).Contains(x => ))
                    //{
                    //    var newCartDetails = _mapper.Map<IEnumerable<CartDetails>>(cart.CartDetails);
                    //    if (newCartDetails.Count() > 0)
                    //    {
                    //        foreach (var item in newCartDetails)
                    //        {
                    //            item.CartHeaderId = cartHeader.CartHeaderId;
                    //        }
                    //        _db.CartDetails.AddRange(newCartDetails);
                    //        await _db.SaveChangesAsync();
                    //    }
                    //}
                    //else
                    //{
                    //    var updatedCartsDetails = _mapper.Map<IEnumerable<CartDetails>>(cart.CartDetails);

                    //}

                    //TMP solution
                    var cartDetails = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == cart.CartDetails.Last().ProductId
                                                                                && x.CartHeaderId == cartHeader.CartHeaderId);
                    if (cartDetails == null)
                    {
                        cart.CartDetails.Last().CartHeaderId = cartHeader.CartHeaderId;
                        var newCartDetails = _mapper.Map<CartDetails>(cart.CartDetails.Last());
                        _db.CartDetails.Add(newCartDetails);
                    }
                    else
                    {
                        cartDetails.Count += cart.CartDetails.Last().Count;
                        _db.CartDetails.Update(cartDetails);
                    }
                    await _db.SaveChangesAsync();
                }
                _responseDto.Result = cart;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        //TODO send parameter throw rout?
        [HttpDelete("deletecartheader")]
        public async Task<ResponseDto> DeleteCartHeader([FromBody] int cartHeaderId)
        {
            try
            {
                //TODO delete cart header and connected details from db
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        //TODO send parameter throw rout?
        [HttpDelete("deletecartdetails")]
        public async Task<ResponseDto> DeleteCartDetails([FromBody] int cartDetailsId)
        {
            try
            {
                var cartDetails = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(x => x.CartDetailsId == cartDetailsId);
                if (cartDetails != null)
                {
                    _db.CartDetails.Remove(cartDetails);
                    //TODO is it best aproche to get all details?
                    var cartDetailsAmountRelatedToCartHeader = await _db.CartDetails.Where(x => x.CartHeaderId == cartDetails.CartHeaderId)
                                                                                 .CountAsync();
                    if (cartDetailsAmountRelatedToCartHeader <= 1)
                    {
                        var cartHeaderToDelete = await _db.CartHeaders.FirstOrDefaultAsync(x => x.CartHeaderId == cartDetails.CartHeaderId);
                        if (cartHeaderToDelete != null)
                        {
                            _db.CartHeaders.Remove(cartHeaderToDelete);
                        }
                    }
                }
                await _db.SaveChangesAsync();
                _responseDto.Result = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpPost]
        public async Task<ResponseDto> ApplyCoupon([FromBody] CartDto cart)
        {
            try
            {
                var cartHeader = await _db.CartHeaders.FirstOrDefaultAsync(x => x.UserId == cart.CartHeader.UserId);
                if (cartHeader != null)
                {
                    cartHeader.CouponCode = cart.CartHeader.CouponCode;
                    _db.CartHeaders.Update(cartHeader);
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpPost]
        public async Task<ResponseDto> RemoveCoupon([FromBody] CartDto cart)
        {
            try
            {
                var cartHeader = await _db.CartHeaders.FirstOrDefaultAsync(x => x.UserId == cart.CartHeader.UserId);
                if (cartHeader != null)
                {
                    cartHeader.CouponCode = String.Empty;
                    _db.CartHeaders.Update(cartHeader);
                    await _db.SaveChangesAsync();
                }
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
