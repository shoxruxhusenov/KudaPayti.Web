﻿using kudapoyti.Service.Common.Utils;
using kudapoyti.Service.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace kudapoyti.Service.Services.Common
{
    public class PaginatonService : IPaginationService
    {
        private readonly IHttpContextAccessor _accessor;
        public PaginatonService(IHttpContextAccessor accessor)
        {
            this._accessor = accessor;
        }

        public async Task<IList<T>> ToPagedAsync<T>(IQueryable<T> items, int pageNumber, int pageSize)
        {
            int totalItems = items.Count();
            PaginationMetaData paginationMetaData = new PaginationMetaData(pageNumber, pageSize, totalItems)
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / (double)pageSize),
                HasPrevious = pageNumber > 1
            };
            paginationMetaData.HasNext = paginationMetaData.CurrentPage < paginationMetaData.TotalPages;


            string json = JsonConvert.SerializeObject(paginationMetaData);
            _accessor.HttpContext!.Response.Headers.Add("X-Pagination", json);

            return await items.Skip(pageNumber * pageSize - pageSize)
                              .Take(pageSize)
                              .ToListAsync();
        }
    }
}
