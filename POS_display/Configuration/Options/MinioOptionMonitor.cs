using Microsoft.Extensions.Options;
using System;

namespace POS_display.Configuration.Options
{
    public class MinioOptionsMonitor<T> : IOptionsMonitor<T>
        where T : class, new()
    {
        public MinioOptionsMonitor(T currentValue)
        {
            CurrentValue = currentValue;
        }

        public T Get(string name)
        {
            return CurrentValue;
        }

        public IDisposable OnChange(Action<T, string> listener)
        {
            throw new NotImplementedException();
        }

        public T CurrentValue { get; }
    }
}
