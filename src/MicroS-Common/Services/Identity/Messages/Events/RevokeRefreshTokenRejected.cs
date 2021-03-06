﻿using MicroS_Common.Messages;
using Newtonsoft.Json;
using System;

namespace MicroS_Common.Services.Identity.Messages.Events
{
    public class RevokeRefreshTokenRejected : IRejectedEvent
    {
        public Guid UserId { get; }
        public string Reason { get; }
        public string Code { get; }

        [JsonConstructor]
        public RevokeRefreshTokenRejected(Guid userId, string reason, string code)
        {
            UserId = userId;
            Reason = reason;
            Code = code;
        }
    }
}
