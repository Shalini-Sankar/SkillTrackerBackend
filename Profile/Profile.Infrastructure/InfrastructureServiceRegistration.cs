﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Profile.Application.Contracts;
using Profile.Infrastructure.Repositories;
using System;
using Microsoft.Azure.Cosmos;
using Profile.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Profile.Infrastructure
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
            CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            services.AddSingleton<CosmosClient>(cosmosClient);
            Database database = cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId).Result;
            database.CreateContainerIfNotExistsAsync(containerId, "/empId");
            services.AddDbContext<SkillTrackerContext>(option => option.UseCosmos(ConnectionString, databaseId));
            
            services.AddScoped<IProfileRepository, ProfileRepository>();         
            return services;
        }
    }
}
