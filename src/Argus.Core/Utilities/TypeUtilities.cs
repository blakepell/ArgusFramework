﻿using System;

namespace Argus.Utilities
{

    /// <summary>
    /// Utilities for working with and identifying types.
    /// </summary>
    /// <remarks></remarks>
    public class TypeUtilities
    {
        //*********************************************************************************************************************
        //
        //             Class:  TypeUtilities
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  08/24/2010
        //      Last Updated:  12/10/2018
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// The System.String type
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Type StringType()
        {
            return Type.GetType("System.String");
        }

        /// <summary>
        /// The System.Integer type (which returns System.Int32).  Even though .Net shows Integer as a type, System.Integer
        /// does not exist, it maps to this.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Type IntegerType()
        {
            return Type.GetType("System.Int32");
        }

        /// <summary>
        /// The System.Int16 Type
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Type Int16Type()
        {
            return Type.GetType("System.Int16");
        }

        /// <summary>
        /// The System.Int32 type
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Type Int32Type()
        {
            return Type.GetType("System.Int32");
        }

        /// <summary>
        /// The System.Int64 type
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Type Int64Type()
        {
            return Type.GetType("System.Int64");
        }

        /// <summary>
        /// The System.DateTime type
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Type DateTimeType()
        {
            return Type.GetType("System.DateTime");
        }

        /// <summary>
        /// The System.Boolean type
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Type BooleanType()
        {
            return Type.GetType("System.Boolean");
        }

        /// <summary>
        /// Returns System.Double type
        /// </summary>
        /// <returns></returns>
        public static Type DoubleType()
        {
            return Type.GetType("System.Double");
        }

        /// <summary>
        /// Returns System.Single type
        /// </summary>
        /// <returns></returns>
        public static Type SingleType()
        {
            return Type.GetType("System.Single");
        }

        /// <summary>
        /// Returns System.Decimal type
        /// </summary>
        /// <returns></returns>
        public static Type DecimalType()
        {
            return Type.GetType("System.Decimal");
        }

    }

}