using Api.Ai.ApplicationService.Factories;
using Api.Ai.Csharp.Frameworks.BotFramework;
using Api.Ai.Csharp.Frameworks.BotFramework.Factories;
using Api.Ai.Csharp.Frameworks.BotFramework.Interfaces;
using Api.Ai.Csharp.Frameworks.Domain.Service.Factories;
using Api.Ai.Domain.Service.Factories;
using Api.Ai.Infrastructure.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Api.Ai.Csharp.Frameworks.Example.Bot.Application
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Json settings
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Newtonsoft.Json.Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };

            // Web API configuration and services

            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.RegisterSingleton<IServiceProvider>(container);

            container.Register<IApiAiAppServiceFactory, ApiAiAppServiceFactory>(Lifestyle.Singleton);
            container.Register<IHttpClientFactory, HttpClientFactory>(Lifestyle.Singleton);
            container.Register<IMessageParseFactory, MessageParseFactory>(Lifestyle.Singleton);

            container.Register<IBotFrameworkMessageTranslator, BotFrameworkMessageTranslator>(Lifestyle.Scoped);

            container.RegisterWebApiControllers(config);
            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
