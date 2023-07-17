/*using Confluent.Kafka;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

namespace N5.Infrastructure.ElasticSearch
{
    public static class Setup
    {
        public static IServiceCollection AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var url =  configuration["ElasticSearch:Uri"];
            var defaultIndex = configuration["ElasticSearch:DefaultIndex"];

            var pool = new SingleNodeConnectionPool(new Uri(url));
            var settings = new ConnectionSettings(pool).DefaultIndex(defaultIndex);
            var client = new ElasticClient(settings);
            services.AddSingleton(client);
            return services;
        }
    }
}
*/