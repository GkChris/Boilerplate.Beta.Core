﻿namespace Boilerplate.Beta.Core.Application.Services.Abstractions
{
    public interface ISignalRPublisherService
    {
        Task SendMessageToAllAsync(string message);
        Task SendMessageToClientAsync(string targetClientId, string message);
    }
}
