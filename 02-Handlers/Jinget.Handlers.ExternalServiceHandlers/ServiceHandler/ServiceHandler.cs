﻿using Jinget.Handlers.ExternalServiceHandlers.ServiceHandler.Factory;

namespace Jinget.Handlers.ExternalServiceHandlers.ServiceHandler;

public abstract class ServiceHandler<T> where T : new()
{
    public T Events { get; set; }

    protected HttpClientFactory HttpClientFactory { get; set; }

    protected ServiceHandler()
    {
        Events = new T();
        HttpClientFactory = new HttpClientFactory();
    }
}