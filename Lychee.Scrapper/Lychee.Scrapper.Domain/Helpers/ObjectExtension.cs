using System;

namespace Lychee.Scrapper.Domain.Helpers
{
    public static class ObjectExtension
    {
        public static int ToInt(this object number)
        {
            try
            {
                return Convert.ToInt32(number);
            }
            catch
            {
                // ignored
            }

            return 0;
        }

        public static DateTime ToDateTime(this object date)
        {
            try
            {
                return Convert.ToDateTime(date);
            }
            catch
            {
                // ignored
            }

            return DateTime.MinValue;
        }

        public static bool ToBool(this object boolean)
        {
            try
            {
                return Convert.ToBoolean(boolean);
            }
            catch
            {
                // ignored
            }

            return false;
        }

        public static decimal ToDecimal(this object dec)
        {
            try
            {
                return Convert.ToDecimal(dec);
            }
            catch
            {
                // ignored
            }

            return 0;
        }
    }
}
