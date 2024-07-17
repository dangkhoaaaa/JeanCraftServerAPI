using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JeanCraftLibrary.Entity;
using JeanCraftLibrary.Model;
using JeanCraftLibrary.Model.Request;
using JeanCraftLibrary.Model.Response;

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
            CreateMap<OrderDetail, OrderDetailCreateRequestModel>().ReverseMap();
            CreateMap<Order, OrderUpdateRequestModel>().ReverseMap();
            CreateMap<Order, OrderCreateRequestModel>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailUpdateRequestModel>().ReverseMap();
            CreateMap<OrderDetail, ListOrderDetailUpdateRequestModel>().ReverseMap();
            CreateMap<ProductInventory, ProductInventoryRequest>().ReverseMap();
            CreateMap<CartItemResponse, CartItem>().ReverseMap();
            CreateMap<ShoppingCartItemResponse, ShoppingCart>().ReverseMap();
            CreateMap<Payment, PaymentCreateRequestModel>().ReverseMap();   
        }
    }
}
