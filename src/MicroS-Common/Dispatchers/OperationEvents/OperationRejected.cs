﻿using Chronicle;
using MicroS_Common.Messages;
using Newtonsoft.Json;
using System;

namespace MicroS_Common.Dispatchers.Operations.Events
{
    [MessageNamespace("operations")]
    public class OperationRejected : IEvent
    {
        public /*Guid*/SagaId Id { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string Resource { get; }
        public string Code { get; }
        public string Message { get; }

        [JsonConstructor]
        public OperationRejected(/*Guid*/SagaId id,
            Guid userId, string name, string resource,
            string code, string message)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Resource = resource;
            Code = code;
            Message = message;
        }
    }
}