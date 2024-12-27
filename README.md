
# Cache Manager

This manager handles cache

## 1. Installation

### 1.1 Register the services in `Program.cs`

```csharp
using Pixsys.Library.Cache.CacheManager;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddCacheManager();
```

### 1.2 Usage

#### 1.2.1 Inject the required service into your controller

```csharp
private readonly ICacheManager _cacheManager;

public MyController(ICacheManager cacheManager)
{
    _cacheManager = cacheManager;
}
```

#### 1.2.2 Methods

```csharp
var tpr = cacheManager.GetFromCache<TickerPricesResult>(pavAtDate.l_tickerPriceResult);
if (tpr == null)
{
    tpr = context.TickerPricesResults.Find(pavAtDate.l_tickerPriceResult);
    cacheManager.AddToCache<TickerPricesResult>(pavAtDate.l_tickerPriceResult, tpr);
}
```