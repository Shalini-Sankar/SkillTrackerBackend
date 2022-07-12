using Admin.Application.Contracts;
using Admin.Domain.Entities;
using Admin.Infrastructure.Cache;
using Admin.Infrastructure.ESCache;
using Admin.Infrastructure.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

namespace Admin.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        private static readonly string EndpointUri = "https://skilltrackercosmosdb.documents.azure.com:443/";
        // The primary key for the Azure Cosmos account.
        private static readonly string PrimaryKey = "2LKbWVWuOnnYj9F5ALaE6p2JJ9Xd2T56I5pIw4xgGMcxMQiuLOjW5KRnR8NoPSypzYxv0rsinYG6fTRpIG2dYQ==";
        private static readonly string ConnectionString = "AccountEndpoint=https://skilltrackercosmosdb.documents.azure.com:443/;AccountKey=2LKbWVWuOnnYj9F5ALaE6p2JJ9Xd2T56I5pIw4xgGMcxMQiuLOjW5KRnR8NoPSypzYxv0rsinYG6fTRpIG2dYQ==;";

        // The name of the database and container we will create
        private static string databaseId = "SkillTracker";
        private static string containerId = "SkillTrackerContainer";

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEnyimMemcached(configuration);
            
            CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            services.AddSingleton<CosmosClient>(cosmosClient);
            Database database = cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId).Result;
            database.CreateContainerIfNotExistsAsync(containerId, "/empId");
            services.AddDbContext<CosmosDbContext>(option => option.UseCosmos(ConnectionString, databaseId));

            services.AddSingleton<ICacheProvider, CacheProvider>();
            services.AddSingleton<ICacheRepository, CacheRepository>();

            services.AddScoped<IProfileRepository, ProfileRepository>();
            
            return services;
        }

        public static void AddElasticsearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            var settings = new ConnectionSettings(new Uri(url))

                .DefaultIndex(defaultIndex)
                .DefaultMappingFor<ESDocument>(m => m
                    .PropertyName(c => c.EmpId, "empId")
                    .PropertyName(c => c.Name, "name")
                    .PropertyName(c => c.Skills, "skills")
                );

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }
    }
}
