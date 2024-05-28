using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;

namespace JeanCraftLibrary.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountDTO>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<Order, OrderFormModel>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailFormModel>().ReverseMap();
        }
    }
}
