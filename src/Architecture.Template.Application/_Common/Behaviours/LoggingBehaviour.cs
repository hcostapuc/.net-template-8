﻿using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Application._Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;

    public LoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("CleanArchitecture Request: {@Request}", request);
        return Task.CompletedTask;
    }
}