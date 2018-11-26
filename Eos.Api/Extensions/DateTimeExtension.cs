using System;

namespace Eos
{
    public static class DateTimeExtension
    {
        public static uint ToUnixTime(this DateTime dateTime)
        {
            return (uint)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
