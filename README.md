# C# api-ai-csharp-frameworks

A C# wrapper [api.ai](https://api.ai/) to frameworks.

## Installation
#### Blip.Ai
To install Api.Ai.Csharp.Blip.Ai, run the following command in the [Package Manager Console](https://docs.nuget.org/consume/package-manager-console)
>PM> Install-Package Api.Ai.Csharp.Blip.Ai

#### BotFramework
To install Api.Ai.Csharp.BotFramework, run the following command in the [Package Manager Console](https://docs.nuget.org/consume/package-manager-console)
>PM> Install-Package Api.Ai.Csharp.BotFramework

### Init

"Api.ai provides developers and companies with the advanced tools they need to build conversational user interfaces for apps and 
hardware devices". To begin, you need to have an [api.ai](https://api.ai/) account.

See api.ai [documentation](https://docs.api.ai/docs) for more details.

##### Using BLiP

See [api.ai To blip.ai](https://www.messenger.com/t/477688789289489) chatbot with docs.</br>

###### Implementation blip.ai

1. Create new Class Libra Project - Visual Studio

![Visual Studio Project](http://i.imgur.com/upA4QJn.png)

    1.1 Install Api.Ai.Csharp.Blip.Ai nuget
    
![Visual Studio Nuget](http://i.imgur.com/lg3WFRO.png)

2. Create ServiceProvider
    
   Using [Dependency Injection](https://en.wikipedia.org/wiki/Dependency_injection), the configuration of the 
   application with [Simple Injector](https://simpleinjector.org/index.html) 
   might look something like this:

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
    2.1 Configure application.json - Define service provider type and set apiai key.
    
```json
{
  "identifier": "brbots",
  "accessKey": "aVdEZzZqSFQ1QzYyODFya0EzS0Q=",
  "messageReceivers": [
    {
      "type": "PlainTextMessageReceiver",
      "mediaType": "text/plain"
    }
  ],
  "settingsType": "ExampleSettings",
  "settings": {
    "ApiAiAccessToken": "e97233fa58454cf0a8215a5538327e3b"
  },
  "startupType": "Startup",
  "serviceProviderType": "ServiceProvider",
  "schemaVersion": 2
}
 ```
3 Implemente PlainTextMessageReceiver class

```csharp
public class PlainTextMessageReceiver : IMessageReceiver
{
    #region Private fields

    private readonly IApiAiAppServiceFactory _apiAiAppServiceFactory;
    private readonly IBlipAiMessageTranslator _blipAiMessageTranslator;
    private readonly IMessagingHubSender _sender;
    private readonly ExampleSettings _settings;

    #endregion

    public PlainTextMessageReceiver(IMessagingHubSender sender, IApiAiAppServiceFactory apiAiAppServiceFactory,
        IBlipAiMessageTranslator blipAiMessageTranslator, ExampleSettings settings)
    {
        _apiAiAppServiceFactory = apiAiAppServiceFactory;
        _blipAiMessageTranslator = blipAiMessageTranslator;
        _sender = sender;
        _settings = settings;
    }

    public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
    {
        var queryAppService = _apiAiAppServiceFactory.CreateQueryAppService("https://api.api.ai/v1", 
            _settings.ApiAiAccessToken);

        var queryRequest = new QueryRequest
        {
            SessionId = message.From.Name,
            Query = new string[] { message.Content.ToString() },
            Lang = Api.Ai.Domain.Enum.Language.BrazilianPortuguese
        };

        var queryResponse = await queryAppService.PostQueryAsync(queryRequest);

        var documents = await _blipAiMessageTranslator.TranslateAsync(queryResponse);

        for (int i = 0; i < documents.Count; i++)
        {
            await _sender.SendMessageAsync(documents[i], message.From, cancellationToken);
            await documents[i].SendChatStateAsync(_sender, message.From, cancellationToken, i, documents.Count);
        }
    }
}
```  
[Download the examples](https://goo.gl/ew06Zi) for test your chatbot  </br>


### TODO

- [x] Create translator Blip.ai
- [x] Create translator BotFramework
- [ ] Implement QuickReply (wait the api.ai [issues])
- [ ] Write unit tests

### License

This software is open source, licensed under the Apache License. </br>
See [LICENSE.me](https://github.com/brunobrandes/api-ai-csharp-frameworks/blob/master/LICENSE.me) for details.
