﻿using System;

namespace Argus.Extensions
{
    /// <summary>
    ///     Type Extensions
    /// </summary>
    public static class TypeExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  TypeExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  01/17/2016
        //      Last Updated:  11/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Returns whether a type is a nullable enum.
        /// </summary>
        /// <param name="t"></param>
        /// <remarks>http://stackoverflow.com/questions/2723048/checking-if-type-instance-is-a-nullable-enum-in-c-sharp</remarks>
        public static bool IsNullableEnum(this Type t)
        {
            var u = Nullable.GetUnderlyingType(t);

            return u != null && u.IsEnum;
        }

    }
}