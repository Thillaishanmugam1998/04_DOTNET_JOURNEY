using Microsoft.EntityFrameworkCore;
using Bank_Account_API.Data;
using Bank_Account_API.Repositories;
using Bank_Account_API.Services;
using Serilog;

namespace Bank_Account_API   
{
    public class Program   
    {
        public static void Main(string[] args)   
        {
            // ---------- STEP 1: BUILDER ----------
            // Ithu app oda "setup manager" — config load, DI container, server ellam ready pannum
            var builder = WebApplication.CreateBuilder(args);

            // Serilog ah register panrom.
            // "appsettings.json" la irukura "Serilog" section ah vaangi —
            // log level, file path, sink ellam config la irunthu read aagum.
            builder.Host.UseSerilog((context, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration));

            try
            {
                // ---------- STEP 2: DATABASE SETUP ----------
                // "ApplicationDbContext" ah DI la register pannu +
                // SQL Server use pannu + appsettings.json la irukura
                // "DefaultConnection" string ah eduthukko nu sollrom
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

                // ---------- STEP 3: DI REGISTRATIONS ----------
                // "Yaaravathu IBankRepository keka, BankRepository kudu" nu sollrom.
                // Scoped = ORU HTTP REQUEST ku ORU new object (1 request = 1 instance)
                builder.Services.AddScoped<IBankRepository, BankRepository>();
                builder.Services.AddScoped<IBankService, BankService>();
                builder.Services.AddScoped<IAccountRepository, AccountRepository>();
                builder.Services.AddScoped<IAccountService, AccountService>();
                builder.Services.AddScoped<ITransactionsRepository, TransactionsRepository>();
                builder.Services.AddScoped<ITransactionsService, TransactionsService>();

                // ---------- STEP 4: CONTROLLERS + SWAGGER ----------
                // Controllers enable pannu — API endpoints ithula dhan irukum
                builder.Services.AddControllers();

                builder.Services.AddEndpointsApiExplorer();  // API details (routes, params) collect pannum
                builder.Services.AddSwaggerGen();            // Swagger JSON document generate pannum

                // ---------- STEP 5: APP BUILD ----------
                // Mela add pannathu ellam setup aachu — ippo actual app create aagum
                // ⚠️ Ithukku apparam services add panna MUDIYATHU!
                var app = builder.Build();

                // ---------- STEP 6: MIDDLEWARE PIPELINE ----------
                // Request vanthu EPPADI handle aaganum nu order la sollrom
                // (top la irunthu bottom ku, request varum bothu)

                if (app.Environment.IsDevelopment())   // Dev mode la mattum Swagger kaatum
                {
                    app.UseSwagger();     // /swagger/v1/swagger.json file serve pannum
                    app.UseSwaggerUI();   // Browser la UI kaatum — API test panna easy
                }
                // Production la Swagger KAATHOM — attackers ku free API map kudukadhu!

                app.UseHttpsRedirection();  // http:// request ah https:// ku redirect (security)

                // Serilog request logging —
                // Oru request ku oru clean log: "HTTP GET /api/banks responded 200 in 45.2ms"
                app.UseSerilogRequestLogging();

                app.UseAuthorization();  // [Authorize] attributes ah check pannum

                app.MapControllers();    // Controllers la irukura API routes ah register pannum

                // ---------- STEP 7: APP START ----------
                Log.Information("Starting web host");  // "App start aagudhu" nu log
                app.Run();  // 🚀 Server start! Inga BLOCK aagum — request ah kaathukittu irukum
            }
            catch (Exception ex)   // Serious error na (DB connect fail, port busy...)
            {
                // Crash details ah FULL ah log pannum — silent ah death aagakoodathu
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally   // Success-ah, error-ah — idhu EPPAVUMUM last la run aagum
            {
                // Serilog buffer la irukura last logs ah FILE la write pannitu thaan exit aagum
                // Idhu illana crash log e file la varave varadhu! 💥
                Log.CloseAndFlush();
            }
        }
    }
}