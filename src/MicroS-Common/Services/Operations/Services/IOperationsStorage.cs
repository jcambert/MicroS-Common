using Chronicle;
using MicroS_Common.Services.Operations.Dto;
using System;
using System.Threading.Tasks;

namespace MicroS_Common.Services.Operations.Services
{
    public interface IOperationsStorage
    {
        Task<OperationDto> GetAsync(/*Guid*/string id);

        Task SetAsync(/*Guid*/SagaId id, Guid userId, string name, OperationState state,
            string resource, string code = null, string reason = null);
    }

}
