﻿namespace Jinget.Blazor.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static IServiceCollection AddJingetBlazor(
        this IServiceCollection services,
        bool addComponents = true,
        bool addAuthenticationStateProvider = true,
        TokenConfigModel? tokenConfigModel = null)
    {
        if (addComponents)
            services.AddMudServices();
        if (tokenConfigModel != null)
        {
            services.Configure<TokenConfigModel>(x =>
            {
                x.Secret = tokenConfigModel.Secret;
                x.ExpirationInMinute = tokenConfigModel.ExpirationInMinute;
                x.TokenName = tokenConfigModel.TokenName;
            });

            services.TryAddScoped<ILocalStorageService, LocalStorageService>();
            services.TryAddScoped<ITokenStorageService>(
                provider => new TokenStorageService(provider.GetRequiredService<ILocalStorageService>(), tokenConfigModel.TokenName));
        }
        if (addAuthenticationStateProvider)
        {
            services.RemoveAll<AuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>();
            services.AddScoped<UserService>();
        }

        return services;
    }
}