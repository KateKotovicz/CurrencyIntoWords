using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CurrencyToWordsConverter.Extensions
{
    public static class DoubleExtensions
    {
        public static int GetIntegerPart(this double number)
        {
            return Convert.ToInt32(Math.Truncate(number));
        }

        public static int GetDecimalPart(this double number, int positions=2)
        {
            int decimalPlaces = Convert.ToInt32(Math.Pow(10, positions));
            return ((number * decimalPlaces) % decimalPlaces).GetIntegerPart();
        }
    }
}