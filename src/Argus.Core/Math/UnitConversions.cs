namespace Argus.Math
{

    /// <summary>
    /// Provides various unit conversions.
    /// </summary>
    public class UnitConversions
    {
        //*********************************************************************************************************************
        //
        //             Class:  UnitConversions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  02/20/2015
        //      Last Updated:  04/04/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Converts Fahrenheit to Celsius.
        /// </summary>
        /// <param name="degreesFahrenheit"></param>
        /// <returns>Celsius degrees</returns>
        public static double FahrenheitToCelsius(double degreesFahrenheit)
        {
            return ((degreesFahrenheit - 32) * 5) / 9;
        }

        /// <summary>
        /// Converts Celsius to Fahrenheit.
        /// </summary>
        /// <param name="degreesCelsius"></param>
        /// <returns>Fahrenheit degrees</returns>
        public static double CelsiusToFahrenheit(double degreesCelsius)
        {
            return ((degreesCelsius * 9) / 5) + 32;
        }

        /// <summary>
        /// Converts miles to kilometers.
        /// </summary>
        /// <param name="miles"></param>
        /// <returns>Kilometers</returns>
        public static double MilesToKilometers(double miles)
        {
            return miles / 0.62137;
        }

        /// <summary>
        /// Converts kilometers to miles.
        /// </summary>
        /// <param name="kilometers"></param>
        /// <returns>Miles</returns>
        public static double KilometersToMiles(double kilometers)
        {
            return kilometers * 0.62137;
        }

        /// <summary>
        /// Converts kilograms to pounds
        /// </summary>
        /// <param name="kilograms"></param>
        /// <returns>Pounds</returns>
        public static double KilogramsToPounds(double kilograms)
        {
            return kilograms * 2.2046;
        }

        /// <summary>
        /// Converts pounds to kilograms
        /// </summary>
        /// <param name="pounds"></param>
        /// <returns>Kilograms</returns>
        public static double PoundsToKilograms(double pounds)
        {
            return pounds / 2.2046;
        }

        /// <summary>
        /// Converts meters to feet
        /// </summary>
        /// <param name="meters"></param>
        /// <returns>Feet</returns>
        public static double MetersToFeet(double meters)
        {
            return meters * 3.2808;
        }

        /// <summary>
        /// Converts feet to meters
        /// </summary>
        /// <param name="feet"></param>
        /// <returns>Meters</returns>
        public static double FeetToMeters(double feet)
        {
            return feet / 3.2808;
        }

        /// <summary>
        /// Converts inches to centimeters
        /// </summary>
        /// <param name="inches"></param>
        /// <returns>Centimeters</returns>
        public static double InchesToCentimeters(double inches)
        {
            return inches / 0.3937;
        }

        /// <summary>
        /// Converts centimeters to inches.
        /// </summary>
        /// <param name="centimeters"></param>
        /// <returns>Inches</returns>
        public static double CentimetersToInches(double centimeters)
        {
            return centimeters * 0.3937;
        }

        /// <summary>
        /// Converts inches to feet.
        /// </summary>
        /// <param name="inches"></param>
        /// <returns>Feet</returns>
        public static double InchesToFeet(double inches)
        {
            return inches * 0.0833333;
        }

        /// <summary>
        /// Converts feet to inches.
        /// </summary>
        /// <param name="feet"></param>
        /// <returns>Inches</returns>
        public static double FeetToInches(double feet)
        {
            return feet / 0.833333;
        }

        /// <summary>
        /// Miles per hour to kilometers per hour
        /// </summary>
        /// <param name="mph"></param>
        /// <returns>Kilometers per hour (KPH)</returns>
        public static double MilesPerHourToKilometersPerHour(double mph)
        {
            return mph * 1.60934;
        }

        /// <summary>
        /// Kilometers per hour to miles per hour
        /// </summary>
        /// <param name="kph"></param>
        /// <returns>Miles per hour (MPH)</returns>
        public static double KilometersPerHourToMilesPerHour(double kph)
        {
            return kph / 1.60934;
        }

        /// <summary>
        /// Converts inches to millimeters
        /// </summary>
        /// <param name="inches"></param>
        /// <returns>Millimeters</returns>
        public static double InchesToMillimeters(double inches)
        {
            return inches / 0.03937;
        }

        /// <summary>
        /// Converts millimeters to inches
        /// </summary>
        /// <param name="millimeters"></param>
        /// <returns>Inches</returns>
        public static double MillimetersToInches(double millimeters)
        {
            return millimeters * 0.03937;
        }
    }
}