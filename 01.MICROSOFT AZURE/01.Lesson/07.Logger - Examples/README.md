# Logging Examples & Core Differences in .NET

This directory contains four Console Applications demonstrating various logging implementations in .NET, ranging from a basic custom logger to enterprise-grade Serilog structured logging:

1. **[CustomLogger](file:///d:/01.THILLAI/02.GIT%20REPO/04_DOTNET_JOURNEY/01.MICROSOFT%20AZURE/01.Lesson/07.Logger%20-%20Examples/CustomLogger)**: A manual, custom-written logger class.
2. **[ILoggerExample](file:///d:/01.THILLAI/02.GIT%20REPO/04_DOTNET_JOURNEY/01.MICROSOFT%20AZURE/01.Lesson/07.Logger%20-%20Examples/ILoggerExample)**: Using .NET's built-in Microsoft `ILogger` abstraction with a custom file provider.
3. **[SerilogExample](file:///d:/01.THILLAI/02.GIT%20REPO/04_DOTNET_JOURNEY/01.MICROSOFT%20AZURE/01.Lesson/07.Logger%20-%20Examples/SerilogExample)**: An implementation using Serilog's standard API directed to a custom file sink.
4. **[SerilogStandard](file:///d:/01.THILLAI/02.GIT%20REPO/04_DOTNET_JOURNEY/01.MICROSOFT%20AZURE/01.Lesson/07.Logger%20-%20Examples/SerilogStandard)**: Pure Serilog out-of-the-box configuration using standard Console, File, and JSON formatting (no custom sink code).

---

## 1. 📂 Log Folder Directory Structure

When executed, the first three projects partition logs dynamically under their respective `bin/Debug/net10.0/` directories following this layout:

```text
bin/Debug/net10.0/
├── LOG/                              <-- Information and request tracing logs
│   ├── Authentication/               <-- [Module] Authentication logs
│   │   └── {yyyy-MM-dd}/
│   │       ├── LoginService.txt      <-- Method entry tracing (Request entered)
│   │       └── log.txt               <-- Info: Login successes
│   └── UserProfile/                  <-- [Module] User Profile logs
│       └── {yyyy-MM-dd}/
│           ├── UserProfileService.txt<-- Method entry tracing
│           └── log.txt               <-- Info: Profile loaded successfully
│
├── ERRORLOG/                         <-- Logical warnings and validation errors
│   ├── Authentication/
│   │   └── {yyyy-MM-dd}/
│   │       └── wrong_users.txt       <-- Login failed validation errors
│   └── UserProfile/
│       └── {yyyy-MM-dd}/
│           └── unauthorized.txt      <-- User profile access denied logical errors
│
└── EXCEPTIONLOG/                     <-- System crashes and try-catch exceptions
    ├── Authentication/
    │   └── {yyyy-MM-dd}/
    │       └── LoginService.txt      <-- System crash / Exception Stacktrace
    └── UserProfile/
        └── {yyyy-MM-dd}/
            └── UserProfileService.txt<-- System crash / Exception Stacktrace
```

---

## 2. 🔌 Real-World Analogy: The Water Distribution System

| Logger Type | Analogy | Description |
| :--- | :--- | :--- |
| **Custom Logger** | **Manual DIY Plumbing** | Laying a specific, permanent pipe yourself from a well directly to a bucket in the garden. It is simple to do, but if you want to route the water to your bathroom tomorrow, you must physically cut, solder, and reconstruct the pipes (Tight Coupling). |
| **ILogger** | **Standard Water Tap Fixture** | A standardized water tap. You open the tap, and water flows. The tap does not need to know whether the source is a well, municipal water supply, or a water delivery truck. It is just an interface. The source is configured outside the tap. |
| **Serilog** | **Smart Filtration & Distribution Router** | A smart system attached to the water main. It automatically filters drinking water, separates greywater for the garden, and routes streams to multiple tanks (Sinks like Console, File, Cloud) concurrently at high speed. |

---

## 3. 📊 Technical Differences Matrix

| Feature | Custom Logger | ILogger (Microsoft) | Serilog |
| :--- | :--- | :--- | :--- |
| **Dependencies** | None (Pure C# / System.IO) | `Microsoft.Extensions.Logging` | `Serilog.AspNetCore`, `Serilog.Sinks.File` |
| **Coupling** | **Tightly Coupled**: Class files directly reference the custom logger. | **Decoupled**: Services only refer to the standard `ILogger` interface. | **Decoupled**: Serilog acts as the concrete implementation behind `ILogger`. |
| **Log Format** | Flat Text String | Abstracted representation | Structured Logging (JSON objects out-of-the-box) |
| **Performance** | Low (Synchronous File Locking) | Medium | Extremely High (Supports Asynchronous logging engine) |
| **Cloud Integration** | Requires manual integration coding | Moderately Easy | Extremely Easy (Using pre-built Cloud Sinks) |

---

## 4. 📝 Code and Output Format (Structured vs Flat Logging)

How parameters and arguments are stored inside logs differ significantly:

### 1. Flat Text Logging (Custom Logger Approach):
* **C# Code:**
  ```csharp
  _logger.LogInfo("Authentication", "log.txt", "Login success for user " + username);
  ```
* **Output in File:**
  ```text
  [2026-08-26 20:24:53.309] [LOG] Login success for user admin
  ```
* **Limitation:** To find actions performed by `admin`, you have to parse the text line-by-line, which is slow and resource-heavy.

### 2. Structured Logging (Serilog Approach):
* **C# Code:**
  ```csharp
  _logger.Information("Login success for user {Username}", username);
  ```
* **Output in File (JSON representation):**
  ```json
  {
    "Timestamp": "2026-08-26T20:45:55.612Z",
    "Level": "Information",
    "MessageTemplate": "Login success for user {Username}",
    "Properties": {
      "Username": "admin"
    }
  }
  ```
* **Advantage:** Since parameters are stored as discrete key-value pairs, log analysis systems can index and query `"Properties.Username = 'admin'"` instantly.

---

## 5. 🚀 How to Run Examples

Each console application simulates five scenarios representing login successes, logical credential errors, system crashes, and module handoffs.

### 1. Custom Logger Project:
```bash
cd CustomLogger
dotnet run
```
* **Feature:** [LoginService.cs](file:///d:/01.THILLAI/02.GIT%20REPO/04_DOTNET_JOURNEY/01.MICROSOFT%20AZURE/01.Lesson/07.Logger%20-%20Examples/CustomLogger/LoginService.cs) and [UserProfileService.cs](file:///d:/01.THILLAI/02.GIT%20REPO/04_DOTNET_JOURNEY/01.MICROSOFT%20AZURE/01.Lesson/07.Logger%20-%20Examples/CustomLogger/UserProfileService.cs) log dynamically by passing target files and levels to `CustomLogger.cs` using a synchronous mutex lock.

### 2. ILogger Project:
```bash
cd ../ILoggerExample
dotnet run
```
* **Feature:** Leverages a custom [FileLoggerProvider.cs](file:///d:/01.THILLAI/02.GIT%20REPO/04_DOTNET_JOURNEY/01.MICROSOFT%20AZURE/01.Lesson/07.Logger%20-%20Examples/ILoggerExample/FileLoggerProvider.cs) registered via `.AddFile(baseDir)`. Service files remain generic, calling only native Microsoft logging functions.

### 3. Serilog Custom Sink Project:
```bash
cd ../SerilogExample
dotnet run
```
* **Feature:** Registers a custom [CustomFileSink.cs](file:///d:/01.THILLAI/02.GIT%20REPO/04_DOTNET_JOURNEY/01.MICROSOFT%20AZURE/01.Lesson/07.Logger%20-%20Examples/SerilogExample/CustomFileSink.cs) to map Serilog events to physical daily folders matching the CustomLogger pattern.

### 4. Serilog Standard Project (Real-World Single File & JSON Logging):
```bash
cd ../SerilogStandard
dotnet run
```
* **Feature:** Implements industry-standard Serilog logging without custom sinks. Writes all logs to a single rolling text file and a single rolling JSON file (`logs/app-log-YYYYMMDD.json`) enriched with `SourceContext` and transaction parameters.

---

## 6. Real-World Production vs Local Logging

While partitioning logs into separate folders (like `LOG/`, `ERRORLOG/`, `EXCEPTIONLOG/`) is intuitive for local debugging on your machine, it introduces fatal flaws in **Production / Cloud Environments**.

### 1. Why Single-Stream Structured JSON is Preferred in Production

* **Multi-Instance Scale:**
  In cloud environments (e.g., Azure App Service), apps scale out to run on multiple concurrent servers (instances). If you split logs into 30 folders across 5 servers, you have **150 scattered log files**. It is impossible to check these files manually.
* **Timeline Correlation (Log Correlation):**
  When debugging a crash, you need to see the entire sequence of events leading up to the error (e.g., request entry ➡ database call ➡ failure ➡ exception stack trace) in chronological order. Splitting logs into separate files fragments the timeline, making debugging much harder.
* **Log Aggregators:**
  Production environments feed a single log stream into indexers like **Seq, Azure Application Insights, or Kibana (ELK)**. These systems ingest the single JSON file and allow you to filter logs by `SourceContext` (module), `Level`, or variables dynamically in milliseconds.

---

### 2. Dangers of Using Custom Loggers or Custom Providers in Production

Using a handmade `CustomLogger.cs` or custom `FileLoggerProvider.cs` under production loads results in critical issues:

#### ❌ a) Thread Blocking & I/O Bottlenecks
* **The Issue:** Manual loggers write to files synchronously using `lock (_lock)`. 
* **The Impact:** Under heavy traffic (e.g., 1000 concurrent login requests), C# threads must wait in line for the lock to release. This freezes request pipelines, resulting in slow APIs, high latency, and timeouts.
* **The Solution:** Production logging frameworks like Serilog use **Asynchronous Sinks**. Logs are pushed to an in-memory queue instantly, allowing web requests to complete without blocking, while a background thread writes them to disk.

#### ❌ b) IIS Application Pool Recycles (Restarts)
* **The Issue:** IIS and Azure App Service monitor directories for files changing inside the application's root directory (`BaseDirectory`).
* **The Impact:** Writing log files constantly inside the root directory triggers directory-monitor alerts, forcing IIS to perform an automatic **AppDomain Recycle (Application Restart)**. This causes dropped user sessions, cold starts, and application downtime.
* **The Solution:** Always write logs outside the web root (e.g., Azure's environment path `D:\home\LogFiles`).

#### ❌ c) Ephemeral Disk Data Loss
* **The Issue:** Virtual machines and containers in cloud platforms auto-scale and recycle regularly.
* **The Impact:** If logs are written to the local disk of a cloud server, they are wiped out forever when that virtual instance scales down or updates.
* **The Solution:** Serilog sinks stream logs off the server over HTTP/TCP directly to a remote centralized log platform, ensuring logs are preserved.

---

## 7. Real-World Load Scenario: On-Premises vs. Azure Cloud Hosting

To fully understand logging under pressure, consider this scenario: **A Web API hosted under continuous, heavy traffic load (1000+ requests per second).**

Here is how the three logging styles perform on **On-Premises PCs** vs. **Azure Cloud Hosting**.

---

### 🏢 Part 1: On-Premises Server (Dedicated Local Server / Local PC)

Under continuous heavy traffic on local hardware:

#### Scenario A: Custom Logger (Synchronous Mutex Lock)
* **What happens:** Every incoming HTTP request attempts to write to disk. Since it uses `lock (_lock)`, only one request can write to the log file at a time. The remaining 999 threads are blocked, waiting in a queue.
* **The Impact:** Response times degrade exponentially (from 50ms to 2.5 seconds). Database connection pools exhaustion errors begin to trigger because database connections are held active while threads wait for file I/O locks. The application lags and eventually timed-out requests build up.

#### Scenario B: ILogger Example (Custom File Provider)
* **What happens:** Unless complex asynchronous buffering is custom-coded into the `FileLoggerProvider`, it blocks threads in the exact same manner as the Custom Logger.
* **The Impact:** Even if written asynchronously using standard `.NET Tasks` (`Task.Run`), writing to disk under high concurrency causes file access violations (`IOException: The process cannot access the file because it is being used by another process`), resulting in unlogged exceptions and critical data loss.

#### Scenario C: Serilog Standard (Rolling File with Async Wrapper)
* **What happens:** The web request thread hits `Log.Information(...)`, dumps the payload instantly into a fast, lock-free memory queue (buffer), and returns a `200 OK` response to the user in 5ms. A dedicated background thread drains this queue, writing logs to the rolling text/JSON files sequentially in chunks.
* **The Impact:** The API remains blazing fast. CPU usage remains stable. Disk writes are consolidated, avoiding I/O lock collisions. No logs are lost, and the user experiences zero lag.

---

### ☁️ Part 2: Azure Cloud Hosting (Azure App Service / Azure Functions)

When scaled out and running on Azure Cloud instances under heavy traffic:

#### Scenario A: Custom Logger (Dynamic Partition Directories)
* **What happens:** 
  1. **IIS Recycles:** Since the Custom Logger writes to folders (`LOG/`, `ERRORLOG/`, `EXCEPTIONLOG/`) created dynamically inside the application's root (`wwwroot`), IIS Web Server detects file system changes and constantly restarts (recycles) the app pool. This drops active user sessions continuously.
  2. **High Latency:** Azure App Service virtual drives have higher write latencies than local SSDs. Synchronous disk locking grinds the app to a halt.
  3. **Scattered Data:** If Azure scales out your API to 3 different VM instances, your logs are split randomly across 3 virtual disks. If Instance #3 crashes and is replaced, those logs are lost forever.

#### Scenario B: ILogger Example (Custom Provider inside wwwroot)
* **What happens:** Same AppDomain recycle issues occur if saving logs in the local web directory. If configured to write to Azure's local path (`D:\home\LogFiles`), IIS restarts are avoided, but the **scaling data loss** remains. You must manually open Kudu consoles for all 3 VM instances to retrieve logs.

#### Scenario C: Serilog Standard (Configured with Central Sinks)
* **What happens:** Serilog is configured to pipe logs directly to **Azure Application Insights** or **Azure Blob Storage** asynchronously using built-in Sinks (e.g. `Serilog.Sinks.ApplicationInsights`).
* **The Impact:** 
  1. No logs are written to the application's local directory, preventing AppDomain restarts completely.
  2. All scaled instances send their structured JSON logs to a single, secure central Azure storage account.
  3. Developers monitor the entire cluster's health from a single cloud dashboard, querying errors using SQL-like expressions (e.g., Kusto Query Language - KQL) within milliseconds.

