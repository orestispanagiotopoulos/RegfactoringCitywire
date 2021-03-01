using System;

namespace App.Helper
{
    public class DateTimeWrapper
    {
        public static DateTime Now => (DateTimeContext.FakeDate != null ? DateTimeContext.FakeDate.Value : DateTime.Now);
    }
}
