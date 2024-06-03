using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dtos;
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

        public ShoppingCartAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _responseDto = new ResponseDto();
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
                        //Is it ok?
                        cartDetails.Count += cart.CartDetails.Last().Count;
                        //cartDetails.CartHeaderId = cart.CartDetails.Last().CartHeaderId;
                        //cartDetails.CartDetailsId = cart.CartDetails.Last().CartDetailsId;
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
    }
}
