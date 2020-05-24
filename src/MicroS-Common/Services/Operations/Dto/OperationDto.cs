using Chronicle;
using System;

namespace MicroS_Common.Services.Operations.Dto
{
    public class OperationDto
    {
        public /*Guid*/string Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Resource { get; set; }
        public string Code { get; set; }
        public string Reason { get; set; }
    }
}
