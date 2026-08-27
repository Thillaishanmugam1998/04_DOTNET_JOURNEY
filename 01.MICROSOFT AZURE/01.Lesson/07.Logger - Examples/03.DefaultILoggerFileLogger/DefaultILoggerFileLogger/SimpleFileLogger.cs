using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefaultILoggerFileLogger
{
    public class SimpleFileLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly SimpleFileLoggerProvider _provider;

        public SimpleFileLogger(string categoryName, SimpleFileLoggerProvider provider)
        {
            _categoryName = categoryName;
            _provider = provider;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            string message = formatter(state, exception);
            if (exception != null)
            {
                message += $"\nException: {exception.Message}";
            }

            // Provider moolama file la eludhurom
            _provider.WriteLog(_categoryName, logLevel, message);
        }
    }
}
