using System;
using System.Threading;

namespace App.Helper
{
    public class DateTimeContext : IDisposable
    {
        private static readonly ThreadLocal<DateTime?> ThreadDate = new ThreadLocal<DateTime?>();

        public DateTimeContext(DateTime fakeDateTime)
        {
            ThreadDate.Value = fakeDateTime;
        }

        public DateTime Value
        {
            get
            {
                return ThreadDate.Value.Value;
            }
        }
        public static DateTime? FakeDate
        {
            get
            {
                if ((ThreadDate.IsValueCreated) && (ThreadDate.Value.HasValue))
                {
                    return ThreadDate.Value.Value;
                }

                return null;
            }
        }

        public void Dispose()
        {
            ThreadDate.Value = null;
        }
    }
}
