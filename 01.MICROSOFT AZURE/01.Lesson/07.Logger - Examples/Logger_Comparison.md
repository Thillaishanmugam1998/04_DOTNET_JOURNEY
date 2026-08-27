# 🪵 Logging in .NET — Custom Logger vs Microsoft ILogger vs Serilog

> **Indha document la** — nammaloda 3 logging approaches compare panrom, real-time examples kodukkurom, 
> and **edhu use pannanum** nnu final recommendation kuduppom.

---

## 📁 Project Structure Reference

```
07.Logger - Examples/
├── 01.CustomLogger/       → Namma own Logger class (manual file write)
├── 02.DefaultILoggerSample/  → Microsoft.Extensions.Logging (Console only)
├── 03.DefaultILoggerFileLogger/ → Microsoft ILogger + Custom File Provider
└── 04.Serilog/            → Serilog package (Console + File sink)
```

---

## 1️⃣ Custom Logger — "Namma Own Logger Class"

### 📌 Enna pannrom?
`File.AppendAllText()` use panni, nammale manually log file la eludhurom.  
Thread safety-kku `lock` use panrom.  
`[CallerFilePath]` use panni, call pannra class name automatic ah catch panrom.

### 🔧 Code Pattern
```csharp
// Logger.cs
public static void WriteLog(string message, [CallerFilePath] string callerFilePath = "")
{
    lock (_lockObj)
    {
        string className = Path.GetFileNameWithoutExtension(callerFilePath);
        string logFilePath = Path.Combine(_basePath, dateFolder, $"{className}.txt");
        File.AppendAllText(logFilePath, logEntry);
    }
}

// Service la use pannum pothu:
Logger.WriteLog("Payment of $100 processed.");
```

### ✅ Advantages
- Simple, easy to understand — beginners-ku nalladu
- No external package dependency
- Full control over folder structure, naming, format

### ❌ Disadvantages
- **Log Levels illa** (Info, Warning, Error differentiate panna mudiyaadhu)
- **Thread safety** — manual ah `lock` manage pannum
- **No structured logging** — JSON, key-value pairs support illa
- **File rolling illa** — date-wise folder nammale create pannum
- **DI integration illa** — static class, so tight coupling
- **Scaling issues** — high-traffic application la `File.AppendAllText` + `lock` → **performance bottleneck**

### 🏭 Real-Time Example
```
✅ Small internal tool, POC, or learning project
✅ Quick debugging — "log file la enna varudhu paarkalaam"
❌ Production application — NEVER USE THIS
```

---

## 2️⃣ Microsoft ILogger — "Built-in .NET Logger"

### 📌 Enna pannrom?
`Microsoft.Extensions.Logging` namespace la irukira **ILogger<T>** interface use panrom.  
DI container moolama inject panrom.  
Console logger built-in ah varum; File logger-kku **custom provider** eludhanum.

### 🔧 Code Pattern (Console — Built-in)
```csharp
// Program.cs
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddTransient<PaymentService>();
    })
    .Build();

// Service la use pannum pothu:
public class PaymentService
{
    private readonly ILogger<PaymentService> _logger;
    
    public PaymentService(ILogger<PaymentService> logger)
    {
        _logger = logger;
    }
    
    public void ProcessPayment(decimal amount)
    {
        _logger.LogInformation("Processing payment: {Amount}", amount);
        _logger.LogWarning("Low balance detected");
        _logger.LogError("Payment failed!");
    }
}
```

### 🔧 Code Pattern (File — Custom Provider)
```csharp
// SimpleFileLoggerProvider + SimpleFileLogger classes eludhanum
// Then register it:
services.AddLogging(builder =>
{
    builder.AddSimpleFileLogger();
});
```

### ✅ Advantages
- **Official Microsoft standard** — .NET ecosystem la default
- **DI integrated** — `ILogger<T>` moolama constructor injection
- **Log Levels** — `LogInformation`, `LogWarning`, `LogError`, `LogCritical`
- **Provider model** — Console, Debug, EventLog built-in
- **ASP.NET Core la automatic** — `Host.CreateDefaultBuilder()` sets it up
- **Structured logging** — `{Amount}` placeholder pattern support

### ❌ Disadvantages
- **File logging built-in ah illa!** — Custom ILoggerProvider eludhanum (50+ lines code)
- **Rolling file logic** nammale manage pannum
- **JSON/Seq/Elasticsearch integration** — manual ah build pannum
- **Configuration** — Console logger mathum simple; file logger complex

### 🏭 Real-Time Example
```
✅ ASP.NET Core Web API — idhu already default ah varum
✅ Azure Functions — ILogger<T> inject panrom
✅ Console apps with DI — Host builder pattern use pannumpothu
❌ File logging venum na → Serilog or NLog prefer panrom
```

---

## 3️⃣ Serilog — "Industry Standard 3rd Party Logger"

### 📌 Enna pannrom?
`Serilog` NuGet package install panni, **Console + File + JSON + Seq + Elasticsearch** — ellathukum 
oru line config la redirect panrom. Rolling file, structured logging, enrichment — built-in ah varum.

### 🔧 Code Pattern
```csharp
// Program.cs — Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message}{NewLine}")
    .WriteTo.File(
        path: "Log/.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Service la use pannum pothu:
public class PaymentService
{
    private readonly ILogger _logger;
    
    public PaymentService(ILogger logger)
    {
        _logger = logger.ForContext<PaymentService>();
    }
    
    public void ProcessPayment()
    {
        _logger.Information("Payment of $100 processed.");
    }
}
```

### ✅ Advantages
- **Structured Logging** — logs JSON format la store aagum → querying easy
- **30+ Sinks ready-made** — File, Console, Seq, Elasticsearch, Azure, SQL, Slack...
- **Rolling file built-in** — Day/Hour/Size based rolling, no extra code
- **Thread-safe by default** — no `lock` needed
- **Enrichers** — Machine Name, Thread ID, Environment auto-add
- **Filtering** — per-namespace minimum level set pannalaam
- **Performance** — asynchronous writing support
- **ILogger integration** — `Serilog.Extensions.Hosting` use panni Microsoft ILogger-oda combine pannalaam

### ❌ Disadvantages
- **External package dependency** — NuGet package install pannum
- **Learning curve** — Sinks, Enrichers, Destructuring concepts learn pannum
- **Package updates** — Major version changes-ku attention venum

### 🏭 Real-Time Example
```
✅ Production Web APIs — Console + File + Seq
✅ Microservices — Centralized logging with Elasticsearch/Kibana
✅ Enterprise Applications — Structured JSON logs for monitoring
✅ Cloud Apps — Azure Application Insights, AWS CloudWatch integrations
✅ Background Services — Async file logging for heavy workloads
```

---

## 📊 Feature Comparison Table

| Feature | Custom Logger | Microsoft ILogger | Serilog |
|---------|:---:|:---:|:---:|
| **Log Levels** (Info/Warn/Error) | ❌ Manual | ✅ Built-in | ✅ Built-in |
| **Console Logging** | ✅ Manual | ✅ Built-in | ✅ Sink |
| **File Logging** | ✅ Manual | ❌ Need Provider | ✅ Sink (1 line) |
| **Rolling Files** (daily/size) | ❌ Manual | ❌ Manual | ✅ Built-in |
| **Structured Logging** (JSON) | ❌ | ✅ Partial | ✅ Full |
| **Thread Safety** | ⚠️ Manual lock | ✅ Built-in | ✅ Built-in |
| **DI Integration** | ❌ Static class | ✅ Native | ✅ With adapter |
| **ASP.NET Core Integration** | ❌ | ✅ Default | ✅ With Serilog.AspNetCore |
| **External Sinks** (Seq/ES/SQL) | ❌ | ❌ | ✅ 30+ sinks |
| **Async Logging** | ❌ | ❌ | ✅ With Serilog.Sinks.Async |
| **NuGet Dependency** | ❌ None | ❌ None (built-in) | ⚠️ Required |
| **Setup Complexity** | 🟢 Simple | 🟡 Medium | 🟡 Medium |
| **Performance (High Load)** | 🔴 Poor | 🟡 Good | 🟢 Excellent |
| **Community/Ecosystem** | ❌ | ✅ Microsoft | ✅ Massive |

---

## 🏗️ Real-Time Scenario Comparison

### Scenario 1: "E-Commerce Order API"
> High-traffic API, 10,000+ requests/minute, logs Azure-kku poganum.

| Approach | Feasibility |
|----------|-------------|
| Custom Logger | ❌ `File.AppendAllText` + `lock` = API lag, timeout errors |
| Microsoft ILogger | ⚠️ Console logger OK, but file logging-kku custom provider eludhanum |
| **Serilog** | ✅ **Best** — Async file sink + Seq/Azure sink → zero API impact |

### Scenario 2: "Internal Admin Tool"
> 5 users, simple CRUD, desktop app vibes.

| Approach | Feasibility |
|----------|-------------|
| Custom Logger | ✅ Works fine for this scale |
| Microsoft ILogger | ✅ Overkill but works |
| Serilog | ✅ Works, but unnecessary complexity |

### Scenario 3: "Microservices Architecture"
> 15 services, Kubernetes la run aagum, centralized logging venum.

| Approach | Feasibility |
|----------|-------------|
| Custom Logger | ❌ Impossible — each service la separate log files, no correlation |
| Microsoft ILogger | ⚠️ Possible with custom providers, but heavy effort |
| **Serilog** | ✅ **Best** — Elasticsearch sink + Correlation ID enricher → Kibana dashboard |

### Scenario 4: "Azure Function / Serverless"
> Event-driven, auto-scaling, logs Application Insights-la poganum.

| Approach | Feasibility |
|----------|-------------|
| Custom Logger | ❌ Serverless la file system unreliable |
| **Microsoft ILogger** | ✅ **Best** — Azure Functions already uses ILogger<T> natively |
| Serilog | ✅ Works with `Serilog.Sinks.ApplicationInsights` |

---

## 🏆 Final Recommendation

```
┌─────────────────────────────────────────────────────────────────────┐
│                                                                     │
│   🥇 PRODUCTION APPLICATION  →  Serilog                           │
│   🥈 ASP.NET Core / Azure    →  Microsoft ILogger + Serilog       │
│   🥉 Learning / Small Tool   →  Custom Logger (understanding-ku)  │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

### 🎯 Best Practice — **ILogger + Serilog Together!**

Real-world production apps la, **Microsoft ILogger interface** use pannitu, **Serilog** ah backend provider ah configure panrom.  
Ipo rendu benefit-um kedaikum:

```csharp
// Program.cs (ASP.NET Core)
var builder = WebApplication.CreateBuilder(args);

// Microsoft ILogger interface use panrom
// But behind the scenes, Serilog handles everything
builder.Host.UseSerilog((context, config) =>
{
    config
        .ReadFrom.Configuration(context.Configuration)  // appsettings.json la config
        .WriteTo.Console()
        .WriteTo.File("Logs/app-.txt", rollingInterval: RollingInterval.Day)
        .WriteTo.Seq("http://localhost:5341");           // Centralized log server
});

// Service la ILogger<T> use panrom (same as before)
public class PaymentService
{
    private readonly ILogger<PaymentService> _logger;  // Microsoft ILogger interface
    
    public PaymentService(ILogger<PaymentService> logger)
    {
        _logger = logger;
    }
    
    public void ProcessPayment(decimal amount)
    {
        // Behind the scenes, Serilog writes to Console + File + Seq
        _logger.LogInformation("Processing payment: {Amount}", amount);
    }
}
```

### 🔑 Summary (One Line)

> **Custom Logger ah learning-kku use pannunga.  
> Production-la ILogger<T> interface use panni, Serilog ah backend-la wire pannunga.  
> Idhu thaan industry standard approach.**

---

## 📦 NuGet Packages Reference

| Package | Purpose |
|---------|---------|
| `Serilog` | Core library |
| `Serilog.Sinks.Console` | Terminal output |
| `Serilog.Sinks.File` | File logging with rolling |
| `Serilog.Sinks.Seq` | Seq centralized logging |
| `Serilog.AspNetCore` | ASP.NET Core integration |
| `Serilog.Extensions.Hosting` | Generic Host integration |
| `Serilog.Enrichers.Thread` | Thread ID enricher |
| `Serilog.Enrichers.Environment` | Machine name enricher |

---

*Created: 2026-08-27 | Part of .NET Logging Journey*
