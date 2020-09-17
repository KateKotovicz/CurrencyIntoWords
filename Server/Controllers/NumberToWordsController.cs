using CurrencyToWordsConverter.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CurrencyToWordsConverter.Controllers
{
    [RoutePrefix("NumberToWords")]
    public class NumberToWordsController : ApiController
    {
        /// <summary>
        /// Pairs of digits and their representations
        /// </summary>
        private readonly Dictionary<int, string> Digits = new Dictionary<int, string>()
        {
            { 1000000,"million"},
            { 1000,"thousand"}
        };

        /// <summary>
        /// 28 pairs of all necessary numbers and their text representations
        /// </summary>
        private readonly Dictionary<int, string> BasicNumbers = new Dictionary<int, string>()
        {
            { 0,"zero"},
            { 1,"one"},
            { 2,"two"},
            { 3,"three"},
            { 4,"four"},
            { 5,"five"},
            { 6,"six"},
            { 7,"seven"},
            { 8,"eight"},
            { 9,"nine"},
            { 10,"ten"},
            { 11,"eleven"},
            { 12,"twelve"},
            { 13,"thirteen"},
            { 14,"fourteen"},
            { 15,"fifteen"},
            { 16,"sixteen"},
            { 17,"seventeen"},
            { 18,"eighteen"},
            { 19,"nineteen"},
            { 20,"twenty"},
            { 30,"thirty"},
            { 40,"forty"},
            { 50,"fifty"},
            { 60,"sixty"},
            { 70,"seventy"},
            { 80,"eighty"},
            { 90,"ninety"}
        };

        /// <summary>
        /// Gets number not bigger than 99 and converts it to text
        /// </summary>
        /// <param name="number"></param>
        /// <returns>Converted to words number</returns>
        public string GetStringFrom2DigitsNumber(int number)
        {
            if (BasicNumbers.ContainsKey(number)) //if number is one of the basic numbers - return it's text representation.
                return BasicNumbers[number];

            int lastDigit = number % 10;
            string lastDigitString = BasicNumbers[lastDigit];
            int firstDigit = number - lastDigit;
            return $"{BasicNumbers[firstDigit]}-{lastDigitString}";
        }

        /// <summary>
        /// Gets number not bigger than 999 and converts it to text
        /// </summary>
        /// <param name="number"></param>
        /// <returns>Converted to words number</returns>
        public string GetStringFrom3DigitsNumber(int number)
        {
            string result = string.Empty;
            int hundrets = (number / 100.0).GetIntegerPart(); //checks for hundreds and converts them to words
            if(hundrets!=0)
                result = BasicNumbers[hundrets] + " hundred ";
            if (number % 100 != 0 || number < 100)//if the number if smaller than 100 or is not divisible by 100, tens and units are also converted to text
                result += GetStringFrom2DigitsNumber(number - (hundrets * 100));
            return result.Trim();
        }

        /// <summary>
        /// Converts number to text
        /// </summary>
        /// <param name="currentDigitIndex">Index of digit from dictionary <see cref="Digits"/>, which is currently converted</param>
        /// <param name="currentString">Text, generated from the digits converted previously. Empty if it's the first run of the metod</param>
        /// <param name="currentNumber">Number to be converted to text. With every run of the method this number is subtracted converted part</param>
        /// <returns>Converted to words number</returns>
        public string GetTextFromNumber(int currentDigitIndex, string currentString, int currentNumber)
        {
            if (currentNumber < 1000)
                return currentString + GetStringFrom3DigitsNumber(currentNumber);
            KeyValuePair<int, string> currentDigit = Digits.ElementAt(currentDigitIndex);
            if (currentNumber >= currentDigit.Key)
            {
                double divisionResult = currentNumber / currentDigit.Key;
                int intDevisionResult = divisionResult.GetIntegerPart();
                currentString += GetStringFrom3DigitsNumber(intDevisionResult) + " " + currentDigit.Value + " ";
                currentNumber -= intDevisionResult * currentDigit.Key;
            }
            return currentNumber == 0 ?
                currentString.Trim() :
                GetTextFromNumber(++currentDigitIndex, currentString, currentNumber);
        }
        
        [HttpGet]
        [Route("Convert/{StrInput}")]
        //NumberToWords/Convert/45125
        public string ConvertNumber(string StrInput)
        {
            try
            {
                double number;
                string InvariantDecimalSeparator = CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator;
                if (!double.TryParse(StrInput.Replace(",", InvariantDecimalSeparator), NumberStyles.Any, CultureInfo.InvariantCulture, out number))
                {
                    return "Wrong number format"; 
                }
                int integerPart = number.GetIntegerPart();
                int decimalPart = number.GetDecimalPart();

                string dollarsString = GetTextFromNumber(0, string.Empty, integerPart) + " dollar" + (integerPart != 1 ? "s" : string.Empty);

                string centsString =
                    decimalPart == 0 ?
                    string.Empty :
                    " and " + GetStringFrom2DigitsNumber(decimalPart) + " cent" + (decimalPart != 1 ? "s" : string.Empty);
                return dollarsString + centsString;
            }
            catch
            {
                return "Error. Most probably number has invalid format.";
            }
        }
    }
}
