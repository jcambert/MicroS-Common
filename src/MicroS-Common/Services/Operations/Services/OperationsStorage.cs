﻿using Chronicle;
using MicroS_Common.Services.Operations.Dto;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MicroS_Common.Services.Operations.Services
{
    public class OperationsStorage : IOperationsStorage
    {
        private readonly IDistributedCache _cache;

        public OperationsStorage(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetAsync(/*Guid*/SagaId id, Guid userId, string name, OperationState state,
            string resource, string code = null, string reason = null)
        {
            var newState = state.ToString().ToLowerInvariant();
            var operation = await GetAsync(id);
            operation = operation ?? new OperationDto();
            operation.Id =  string.IsNullOrEmpty( id)?SagaId.NewSagaId():id;
            operation.UserId = userId;
            operation.Name = name;
            operation.State = newState;
            operation.Resource = resource;
            operation.Code = code ?? string.Empty;
            operation.Reason = reason ?? string.Empty;

            await _cache.SetStringAsync(operation.Id,
                JsonConvert.SerializeObject(operation),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(1)
                });
        }

        public async Task<OperationDto> GetAsync(/*Guid*/string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var operation = await _cache.GetStringAsync(id);

            return string.IsNullOrWhiteSpace(operation) ? null : JsonConvert.DeserializeObject<OperationDto>(operation);
        }
    }
}
