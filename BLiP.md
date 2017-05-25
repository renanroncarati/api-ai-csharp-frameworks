##### Implementation

1. Create new Class Libra Project - Visual Studio

![Visual Studio Project](http://i.imgur.com/upA4QJn.png)

    1.1 Install Api.Ai.Csharp.Blip.Ai nuget
    
![Visual Studio Nuget](http://i.imgur.com/lg3WFRO.png)

    1.2 Create ServiceProvider
    
   Using [Dependency Injection](https://en.wikipedia.org/wiki/Dependency_injection), the configuration of the 
   application with [Simple Injector](https://simpleinjector.org/index.html) might look something like this:

```csharp
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
```  
    1.3 Configure service provider type in application.json
    
```json
{
  "identifier": "YOUR_BLIPAI_IDENTIFIER",
  "accessKey": "YOUR_BLIPAI_ACCESS_KEY",
  "messageReceivers": [
    {
      "type": "PlainTextMessageReceiver",
      "mediaType": "text/plain"
    }
  ],
  "settings": {
    "setting1": "value1"
  },
  "startupType": "Startup",
  "serviceProviderType": "ServiceProvider",
  "schemaVersion": 2
}
 ```
    1.4 Implemente PlainTextMessageReceiver class

```csharp
public class PlainTextMessageReceiver : IMessageReceiver
{
    #region Private fields

    private readonly IApiAiAppServiceFactory _apiAiAppServiceFactory;
    private readonly IBlipAiMessageTranslator _blipAiMessageTranslator;
    private readonly IMessagingHubSender _sender;

    #endregion

    public PlainTextMessageReceiver(IMessagingHubSender sender, IApiAiAppServiceFactory apiAiAppServiceFactory,
        IBlipAiMessageTranslator blipAiMessageTranslator)
    {
        _apiAiAppServiceFactory = apiAiAppServiceFactory;
        _blipAiMessageTranslator = blipAiMessageTranslator;
        _sender = sender;
    }

    public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
    {
        var queryAppService = _apiAiAppServiceFactory.CreateQueryAppService("https://api.api.ai/v1", 
        "YOUR_ACCESS_TOKEN");

        var queryRequest = new QueryRequest
        {
            SessionId = "1",
            Query = new string[] { message.Content.ToString() },
            Lang = Api.Ai.Domain.Enum.Language.Portuguese
        };

        var queryResponse = await queryAppService.PostQueryAsync(queryRequest);

        var documents = await _blipAiMessageTranslator.TranslateAsync(queryResponse);

        foreach (var document in documents)
        {
            await _sender.SendMessageAsync(document, message.From, cancellationToken);
        }
    }
}
```  
[Download the examples](https://goo.gl/ew06Zi) for test your chatbot  </br>
