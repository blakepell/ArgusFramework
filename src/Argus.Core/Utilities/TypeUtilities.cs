/*
 * @author            : Blake Pell
 * @initial date      : 2010-08-24
 * @last updated      : 2018-12-10
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

namespace Argus.Utilities
{
    /// <summary>
    /// Utilities for working with and identifying common types.
    /// </summary>
    public static class TypeUtilities
    {
        /// <summary>
        /// The System.String type
        /// </summary>
        public static Type StringType()
        {
            return Type.GetType("System.String");
        }

        /// <summary>
        /// The System.Integer type (which returns System.Int32).  Even though .Net shows Integer as a type, System.Integer
        /// does not exist, it maps to this.
        /// </summary>
        public static Type IntegerType()
        {
            return Type.GetType("System.Int32");
        }

        /// <summary>
        /// The System.Int16 Type
        /// </summary>
        public static Type Int16Type()
        {
            return Type.GetType("System.Int16");
        }

        /// <summary>
        /// The System.Int32 type
        /// </summary>
        public static Type Int32Type()
        {
            return Type.GetType("System.Int32");
        }

        /// <summary>
        /// The System.Int64 type
        /// </summary>
        public static Type Int64Type()
        {
            return Type.GetType("System.Int64");
        }

        /// <summary>
        /// The System.DateTime type
        /// </summary>
        public static Type DateTimeType()
        {
            return Type.GetType("System.DateTime");
        }

        /// <summary>
        /// The System.Boolean type
        /// </summary>
        public static Type BooleanType()
        {
            return Type.GetType("System.Boolean");
        }

        /// <summary>
        /// Returns System.Double type
        /// </summary>
        public static Type DoubleType()
        {
            return Type.GetType("System.Double");
        }

        /// <summary>
        /// Returns System.Single type
        /// </summary>
        public static Type SingleType()
        {
            return Type.GetType("System.Single");
        }

        /// <summary>
        /// Returns System.Decimal type
        /// </summary>
        public static Type DecimalType()
        {
            return Type.GetType("System.Decimal");
        }
    }
}