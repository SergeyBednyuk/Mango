using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dtos;

namespace Mango.Services.CouponAPI
{
    public sealed class MappingConfig : Profile
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapperConfig = new MapperConfiguration(config => 
            { 
                //config.CreateMap<CouponDto, CouponDto>();
                config.CreateMap<Coupon, CouponDto>().ReverseMap();
            });

            return mapperConfig;
        }
    }
}
