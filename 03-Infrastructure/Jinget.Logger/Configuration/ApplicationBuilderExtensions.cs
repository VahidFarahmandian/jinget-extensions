﻿using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http;
using Jinget.Core.ExtensionMethods;
using Jinget.ExceptionHandler.Entities;

namespace Jinget.Logger.Configuration;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Add logging required middlewares to pipeline
    /// </summary>
    public static IApplicationBuilder UseJingetLogging(this IApplicationBuilder app)
    {
        app.UseExceptionHandler();
        app.Use(async (ctx, next) =>
        {
            var baseSetting = ctx.RequestServices.GetJingetService<BaseSettingModel>();
            await next();
            if (baseSetting.Handle4xxResponses)
            {
                if (ctx.Response.StatusCode is >= 400 and < 500)
                {
                    throw new HttpRequestException(ReasonPhrases.GetReasonPhrase(ctx.Response.StatusCode), null, (System.Net.HttpStatusCode?)ctx.Response.StatusCode);
                }
            }
        });
        app.UseMiddleware<LogRequestMiddleware>();
        app.UseMiddleware<LogResponseMiddleware>();

        return app;
    }
}