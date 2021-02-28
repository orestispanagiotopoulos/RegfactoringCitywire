using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Helper
{
    public class DateTimeWrapper
    {
        public static DateTime Now => (DateTimeContext.FakeDate != null ? DateTimeContext.FakeDate.Value : DateTime.Now);
    }
}
