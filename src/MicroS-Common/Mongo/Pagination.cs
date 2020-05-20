﻿using MicroS_Common.Types;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MicroS_Common.Mongo
{
    public static class Pagination
    {
        public static PagedResult<T>Paginate<T>(this IQueryable<T> collection,PagedQueryBase query)
        {
            if(!collection.Any())
                return PagedResult<T>.Empty;
            var totalResults = collection.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalResults /query.Results);
            var data =collection.Skip((query.Page - 1) * query.Results).Take(query.Results);

            return PagedResult<T>.Create(data, query.Page, query.Results, totalPages, totalResults);
        }
        public static async Task<PagedResult<T>> PaginateAsync<T>(this IMongoQueryable<T> collection, PagedQueryBase query)
            => await collection.PaginateAsync(query.Page, query.Results);

        public static async Task<PagedResult<T>> PaginateAsync<T>(this IMongoQueryable<T> collection,
            int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (resultsPerPage <= 0)
            {
                resultsPerPage = 10;
            }
            var isEmpty = await collection.AnyAsync() == false;
            if (isEmpty)
            {
                return PagedResult<T>.Empty;
            }
            var totalResults = await collection.CountAsync();
            var totalPages = (int)Math.Ceiling((decimal)totalResults / resultsPerPage);
            var data = await collection.Limit(page, resultsPerPage).ToListAsync();

            return PagedResult<T>.Create(data, page, resultsPerPage, totalPages, totalResults);
        }

        public static IMongoQueryable<T> Limit<T>(this IMongoQueryable<T> collection, PagedQueryBase query)
            => collection.Limit(query.Page, query.Results);

        public static IMongoQueryable<T> Limit<T>(this IMongoQueryable<T> collection,
            int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
            {
                page = 1;
            }
            if (resultsPerPage <= 0)
            {
                resultsPerPage = 10;
            }
            var skip = (page - 1) * resultsPerPage;
            var data = collection.Skip(skip)
                .Take(resultsPerPage);

            return data;
        }
    }
}
