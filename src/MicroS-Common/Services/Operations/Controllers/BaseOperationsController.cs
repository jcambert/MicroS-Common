using Chronicle;
using MicroS_Common.Controllers;
using MicroS_Common.Dispatchers;
using MicroS_Common.Services.Operations.Dto;
using MicroS_Common.Services.Operations.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroS_Common.Services.Operations.Controllers
{

    public class BaseOperationsController : BaseController
    {
        private readonly IOperationsStorage _operationsStorage;

        public BaseOperationsController(
            IDispatcher dispatcher,
            IConfiguration config,
            IOperationsStorage operationsStorage,
            IOptions<AppOptions> appOptions) : base(dispatcher,config,appOptions)
        {
            _operationsStorage = operationsStorage;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OperationDto>> Get(/*Guid*/string id)
            => Single(await _operationsStorage.GetAsync(id));
    }
}
