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
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace JeanCraftLibrary.Mapper
{
    public class AutoMapperProduct : Profile
    {
        public AutoMapperProduct()
        {
            CreateMap<CreateProductByBookingRequest, Product>().ReverseMap();
            CreateMap<List<IGrouping<Guid?, Component>>, ComponentListResponse>()
               .ConvertUsing(source => new ComponentListResponse
               {
                   ListComponentForType = source.Select(group => new ComponentForType
                   {
                       Type = group.Key ?? Guid.Empty, // Chuyển null thành Guid.Empty nếu cần
                       Components = group.AsEnumerable()
                   }).ToList()
               });
        }
    }
}
