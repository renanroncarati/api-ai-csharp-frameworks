using Api.Ai.ApplicationService.Factories;
using Api.Ai.Csharp.Frameworks.Blip.Ai;
using Api.Ai.Csharp.Frameworks.Blip.Ai.Interfaces;
using Api.Ai.Domain.Service.Factories;
using Api.Ai.Infrastructure.Factories;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Takenet.MessagingHub.Client.Host;

namespace Api.Ai.Csharp.Frameworks.Example.Blip.Ai
{
    public class ServiceProvider : Container, IServiceContainer
    {
        public ServiceProvider()
        {                        
            this.Register<IApiAiAppServiceFactory, ApiAiAppServiceFactory>(Lifestyle.Singleton);
            this.Register<IHttpClientFactory, HttpClientFactory>(Lifestyle.Singleton);

            this.Register<IBlipAiMessageTranslator, BlipAiMessageTranslator>(Lifestyle.Singleton);
        }

        #region IServiceContainer Members

        public void RegisterService(Type serviceType, object instance)
        {
            base.RegisterSingleton(serviceType, instance);
        }

        public void RegisterService(Type serviceType, Func<object> instanceFactory)
        {
            base.RegisterSingleton(serviceType, instanceFactory);
        }

        #endregion

    }
}
