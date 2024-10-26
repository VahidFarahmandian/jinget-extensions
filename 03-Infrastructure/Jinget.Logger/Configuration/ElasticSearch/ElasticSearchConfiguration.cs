﻿namespace Jinget.Logger.Configuration.ElasticSearch;

public static class ElasticSearchConfiguration
{
    public static void ConfigureElasticSearchLogger(this IServiceCollection services, ElasticSearchSettingModel elasticSearchSetting = null)
    {
        services.ConfigureJingetLoggerPrerequisites();
        services.TryAddScoped<IElasticSearchLoggingDomainService, ElasticSearchLoggingDomainService>();

        services.TryAddScoped<IElasticSearchLoggingRepository, ElasticSearchLoggingRepository>();

        if (elasticSearchSetting != null)
        {
            services.TryAddSingleton(elasticSearchSetting);

            var client = CreateClient(elasticSearchSetting);
            services.AddSingleton(typeof(IElasticClient), client);
            CreateIndex(client, elasticSearchSetting);
        }
    }

    static ElasticClient CreateClient(ElasticSearchSettingModel elasticSearchSetting)
    {
        ConnectionSettings connectionSettings;
        string protocol = elasticSearchSetting.UseSsl ? "https" : "http";
        Uri elasticUrl = new($"{protocol}://{elasticSearchSetting.Url}");
        if (string.IsNullOrEmpty(elasticSearchSetting.UserName) && string.IsNullOrEmpty(elasticSearchSetting.Password))
            connectionSettings = new ConnectionSettings(elasticUrl)
            .DefaultDisableIdInference(true);
        else
        {
            connectionSettings = new ConnectionSettings(elasticUrl)
                    .BasicAuthentication(elasticSearchSetting.UserName, elasticSearchSetting.Password)
                    .DefaultDisableIdInference(true);
            if (elasticSearchSetting.UseSsl && elasticSearchSetting.BypassCertificateValidation)
            {
                connectionSettings = connectionSettings.ServerCertificateValidationCallback(CertificateValidations.AllowAll);
            }
        }

        return new ElasticClient(connectionSettings);
    }

    static void CreateIndex(ElasticClient client, ElasticSearchSettingModel elasticSearchSetting)
    {
        if (!elasticSearchSetting.CreateIndexPerPartition)
        {
            string indexName = $"{AppDomain.CurrentDomain.FriendlyName}".ToLower();
            var invalidChars = @" \*\\<|,>/?".ToCharArray();
            indexName = new(indexName.Where(c => !invalidChars.Contains(c)).ToArray());
            if (!client.Indices.Exists(indexName).Exists)
            {
                var result = client.Indices
                    .Create(indexName, index => index.Map(m => m.AutoMap(typeof(LogModel)).NumericDetection(true)));
                if (!result.IsValid)
                    throw new JingetException("Jinget Says: " + result.OriginalException);
            }
        }
    }
}