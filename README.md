# C# api-ai-csharp-frameworks

A C# wrapper [api.ai](https://api.ai/) to frameworks.

## Installation
#### Blip.Ai
To install Api.Ai.Csharp.Blip.Ai, run the following command in the [Package Manager Console](https://docs.nuget.org/consume/package-manager-console)
>PM> Install-Package Api.Ai.Csharp.Blip.Ai

#### BotFramework
To install Api.Ai.Csharp.Blip.Ai, run the following command in the [Package Manager Console](https://docs.nuget.org/consume/package-manager-console)
>PM> Install-Package Api.Ai.Csharp.BotFramework

### Init

"Api.ai provides developers and companies with the advanced tools they need to build conversational user interfaces for apps and 
hardware devices". To begin, you need to have an [api.ai](https://api.ai/) account.

See api.ai [documentation](https://docs.api.ai/docs) for more details.

Using [Dependency Injection](https://en.wikipedia.org/wiki/Dependency_injection), the configuration of the application with [Simple Injector](https://simpleinjector.org/index.html) might look something like this:

```csharp
    var container = new Container();  
    container.Register<IApiAiAppServiceFactory, ApiAiAppServiceFactory>(Lifestyle.Singleton);
    container.Register<IHttpClientFactory, HttpClientFactory>(Lifestyle.Singleton);
    
```  
##### Using blip.ai [Get to know](https://blip.ai)

```csharp
container.Register<IBlipAiMessageTranslator, BlipAiMessageTranslator>(Lifestyle.Singleton);
```  

```csharp
var queryAppService = _apiAiAppServiceFactory.CreateQueryAppService("https://api.api.ai/v1", "YOUR_ACCESS_TOKEN");

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
```  

##### Using BotFramework [Get to know](https://botframework.com)

```csharp
container.Register<IBotFrameworkMessageTranslator, BotFrameworkMessageTranslator>(Lifestyle.Singleton);
```  

```csharp
if (activity.Type == ActivityTypes.Message)
{
    var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

    var queryAppService = _apiAiAppServiceFactory.CreateQueryAppService("https://api.api.ai/v1", "YOUR_ACCESS_TOKEN");

    var queryRequest = new QueryRequest
    {
      SessionId = "1",
      Query = new string[] { activity.Text },
      Lang = Api.Ai.Domain.Enum.Language.English
    };

    var queryResponse = await queryAppService.PostQueryAsync(queryRequest);

    var activities = await _botFrameworkMessageTranslator.TranslateAsync(queryResponse, activity);
    foreach (var reply in activities)
    {
      await connector.Conversations.ReplyToActivityAsync(reply);
    }   
}
```  
[Download the examples](https://goo.gl/ew06Zi) for test your implementation  </br>

### TODO

- [x] Create translator Blip.ai
- [x] Create translator BotFramework
- [ ] Implement QuickReply (wait the api.ai [issues])
- [ ] Write unit tests

### License

This software is open source, licensed under the Apache License. </br>
See [LICENSE.me](https://github.com/brunobrandes/api-ai-csharp-frameworks/blob/master/LICENSE.me) for details.
