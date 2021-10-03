/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2016-03-01
 * @last updated      : 2021-10-03
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Argus.Extensions
{
    /// <summary>
    /// Enum Extensions
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets whether the specified bit or bits is provided.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue">A flags enum.</param>
        /// <param name="bit">A single bit or multiple bits. Use | to check or for multiple bits and &amp; to check for all bits.</param>
        public static bool IsBitSet<T>(this T enumValue, T bit) where T : Enum
        {
            if (Unsafe.SizeOf<T>() == Unsafe.SizeOf<ulong>())
            {
                return (enumValue.AsULong() & bit.AsULong()) != 0;
            }
            else if (Unsafe.SizeOf<T>() == Unsafe.SizeOf<long>())
            {
                return (enumValue.AsLong() & bit.AsLong()) != 0;
            }
            else if (Unsafe.SizeOf<T>() == Unsafe.SizeOf<int>())
            {
                return (enumValue.AsInt() & bit.AsInt()) != 0;
            }
            else if (Unsafe.SizeOf<T>() == Unsafe.SizeOf<uint>())
            {
                return (enumValue.AsUInt() & bit.AsInt()) != 0;
            }
            else if (Unsafe.SizeOf<T>() == Unsafe.SizeOf<short>())
            {
                return (enumValue.AsShort() & bit.AsInt()) != 0;
            }
            else if (Unsafe.SizeOf<T>() == Unsafe.SizeOf<ushort>())
            {
                return (enumValue.AsUShort() & bit.AsInt()) != 0;
            }

            throw new Exception("Type mismatch.");
        }

        /// <summary>
        /// Casts the generic enum to an specified type.  An exception will be thrown if the <see cref="Enum"/>
        /// does not have a matching SizeOf which means the type of TEnum needs to match the type of TOutputType.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TOutputType"></typeparam>
        /// <param name="enumValue"></param>
        public static TOutputType AsGeneric<TEnum, TOutputType>(this TEnum enumValue) where TEnum : Enum
        {
            if (Unsafe.SizeOf<TEnum>() == Unsafe.SizeOf<TOutputType>())
            {
                return Unsafe.As<TEnum, TOutputType>(ref enumValue);
            }

            throw new Exception("Type mismatch.");
        }

        /// <summary>
        /// Casts the generic enum to a <see cref="long"/>.  An exception will be thrown if the <see cref="Enum"/>
        /// does not have a matching SizeOf.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumValue"></param>
        public static long AsLong<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            if (Unsafe.SizeOf<TEnum>() != Unsafe.SizeOf<long>())
            {
                throw new Exception("Type mismatch.");
            }

            return Unsafe.As<TEnum, long>(ref enumValue);
        }

        /// <summary>
        /// Casts the generic enum to a <see cref="ulong"/>.  An exception will be thrown if the <see cref="Enum"/>
        /// does not have a matching SizeOf.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumValue"></param>
        public static ulong AsULong<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            if (Unsafe.SizeOf<TEnum>() != Unsafe.SizeOf<ulong>())
            {
                throw new Exception("Type mismatch.");
            }

            return Unsafe.As<TEnum, ulong>(ref enumValue);
        }

        /// <summary>
        /// Casts the generic enum to an <see cref="int"/>.  An exception will be thrown if the <see cref="Enum"/>
        /// does not have a matching SizeOf.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumValue"></param>
        public static int AsInt<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            if (Unsafe.SizeOf<TEnum>() != Unsafe.SizeOf<int>())
            {
                throw new Exception("Type mismatch.");
            }

            return Unsafe.As<TEnum, int>(ref enumValue);
        }

        /// <summary>
        /// Casts the generic enum to an <see cref="uint"/>.  An exception will be thrown if the <see cref="Enum"/>
        /// does not have a matching SizeOf.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumValue"></param>
        public static uint AsUInt<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            if (Unsafe.SizeOf<TEnum>() != Unsafe.SizeOf<uint>())
            {
                throw new Exception("Type mismatch.");
            }

            return Unsafe.As<TEnum, uint>(ref enumValue);
        }

        /// <summary>
        /// Casts the generic enum to a <see cref="short"/>.  An exception will be thrown if the <see cref="Enum"/>
        /// does not have a matching SizeOf.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumValue"></param>
        public static short AsShort<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            if (Unsafe.SizeOf<TEnum>() != Unsafe.SizeOf<short>())
            {
                throw new Exception("Type mismatch.");
            }

            return Unsafe.As<TEnum, short>(ref enumValue);
        }

        /// <summary>
        /// Casts the generic enum to a <see cref="ushort"/>.  An exception will be thrown if the <see cref="Enum"/>
        /// does not have a matching SizeOf.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumValue"></param>
        public static ushort AsUShort<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            if (Unsafe.SizeOf<TEnum>() != Unsafe.SizeOf<ushort>())
            {
                throw new Exception("Type mismatch.");
            }

            return Unsafe.As<TEnum, ushort>(ref enumValue);
        }

        /// <summary>
        /// Get's an attribute from an Enum.  This can be used to iterate over the items in an enum.
        /// Example:
        /// <code>
        /// foreach (MenuAttribute.MenuGroup group in Enum.GetValues(typeof(MenuAttribute.MenuGroup))) {}
        /// </code>
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="value"></param>
        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            string name = Enum.GetName(type, value);

            return type.GetField(name)
                       .GetCustomAttributes(false)
                       .OfType<TAttribute>()
                       .SingleOrDefault();
        }
    }
}