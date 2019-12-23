# Funky
NetStandard extensions and utilities set, focused towards Azure Functionsand serverless applications.

## Components

### JwtValidator

Custom Function execution filter for validating Json Web Tokens, only usable in Http-triggered functions


### HttpContextExtensions

#### General

- IIdentity GetIdentity() => Returns the current user identity inside HttpContext
- bool AuthorizedByClientSecret() => ONLY FOR AZURE AD TOKENS, Returns if the token has been generated using a ClientId and ClientSecret
- bool AuthorizedByCertificate() => ONLY FOR AZURE AD TOKENS, Returns if the token has been generated using a Certificate

#### JwtValidator specific
- bool IsJwtInvalid() => Returns if the provided token couldn't be validated by JwtValidator (malformed, expired, wrong signature...)
- bool IsUserAnonymous() => Returns if JwtValidator has determined that the user is anonymous (no auth header found in the request)

### ActionFilters

Provides a .Net MVC filters-like method of implementing custom bussiness logic filters that run before the actual code of the function starts executing. Focused towards Http-triggered functions.

Funky's filters system is based on three main parts.

#### IActionFilter

First an IActionFilter interface is provided, every filter should be based upon it and implement it's ExecuteFilter() method, the current httpcontext is provided as params to the method.

```csharp
    public interface IActionFilter
    {
        Task<bool> ExecuteFilter(IHttpContextAccessor httpContextAccessor);
    }
```

The returned bool must indicate the result of the filter's logic execution.

#### FilterMapper

The FilterMapper class provides the capability to map certain filters to an specific key (usually a class name of method name) so they can be executed as a group with a single call to the filter executor.

```csharp
    public class FilterMapper
    {
        public void MapFilter<TClass, TFilter>();

        public void MapFilter<TFilter>(string actionName);

        public IEnumerable<Type> GetFilters(string actionName);
    }
```

#### MainFilterExecutor

WIP



