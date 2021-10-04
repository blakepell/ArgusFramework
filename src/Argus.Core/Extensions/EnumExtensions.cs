/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2016-03-01
 * @last updated      : 2021-10-04
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBitSet<T>(this T enumValue, T bit) where T : Enum
        {
            var size = Unsafe.SizeOf<T>();

            if (size == 8)
            {
                return (enumValue.AsULong() & bit.AsULong()) != 0;
            }
            else if (size == 4)
            {
                return (enumValue.AsUInt() & bit.AsUInt()) != 0;
            }
            else if (size == 2)
            {
                return (enumValue.AsUShort() & bit.AsUShort()) != 0;
            }

            throw new Exception("Type mismatch.");
        }

        /// <summary>
        /// Sets a bit based off of the <see cref="Enum"/> provided and returns an updated <see cref="Enum"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <param name="bit"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T SetBit<T>(this T enumValue, T bit) where T : Enum
        {
            var size = Unsafe.SizeOf<T>();

            if (size == 8)
            {
                var result = (enumValue.AsULong() | bit.AsULong());
                return Unsafe.As<ulong, T>(ref result);
            }
            else if (size == 4)
            {
                var result = (enumValue.AsUInt() | bit.AsUInt());
                return Unsafe.As<uint, T>(ref result);
            }
            else if (size == 2)
            {
                var result = (ushort)(enumValue.AsUShort() | bit.AsUShort());
                return Unsafe.As<ushort, T>(ref result);
            }

            throw new Exception("Type mismatch.");
        }

        /// <summary>
        /// Removes a bit from the <see cref="Enum"/> provided and returns an updated <see cref="Enum"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <param name="bit"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RemoveBit<T>(this T enumValue, T bit) where T : Enum
        {
            var size = Unsafe.SizeOf<T>();

            if (size == 8)
            {
                var result = (enumValue.AsULong() & ~bit.AsULong());
                return Unsafe.As<ulong, T>(ref result);
            }
            else if (size == 4)
            {
                var result = (enumValue.AsUInt() & ~bit.AsUInt());
                return Unsafe.As<uint, T>(ref result);
            }
            else if (size == 2)
            {
                var result = (ushort)(enumValue.AsUShort() & ~bit.AsUShort());
                return Unsafe.As<ushort, T>(ref result);
            }

            throw new Exception("Type mismatch.");
        }

        /// <summary>
        /// Removes a bit from the <see cref="Enum"/> provided and returns an updated <see cref="Enum"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <param name="bit"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ToggleBit<T>(this T enumValue, T bit) where T : Enum
        {
            var size = Unsafe.SizeOf<T>();

            if (size == 8)
            {
                var result = (enumValue.AsULong() ^ bit.AsULong());
                return Unsafe.As<ulong, T>(ref result);
            }
            else if (size == 4)
            {
                var result = (enumValue.AsUInt() ^ bit.AsUInt());
                return Unsafe.As<uint, T>(ref result);
            }
            else if (size == 2)
            {
                var result = (ushort)(enumValue.AsUShort() ^ bit.AsUShort());
                return Unsafe.As<ushort, T>(ref result);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TOutputType AsGeneric<TEnum, TOutputType>(this TEnum enumValue) where TEnum : Enum
        {
            // We don't know the size of either so we have to get both.
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long AsLong<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            // Make sure the TEnum is the proper size (hard coded short length for perf).
            if (Unsafe.SizeOf<TEnum>() != 8)
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong AsULong<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            // Make sure the TEnum is the proper size (hard coded short length for perf).
            if (Unsafe.SizeOf<TEnum>() != 8)
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AsInt<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            // Make sure the TEnum is the proper size (hard coded short length for perf).
            if (Unsafe.SizeOf<TEnum>() != 4)
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint AsUInt<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            // Make sure the TEnum is the proper size (hard coded short length for perf).
            if (Unsafe.SizeOf<TEnum>() != 4)
            {
                throw new Exception("Type mismatch.");
            }

            return Unsafe.As<TEnum, uint>(ref enumValue);
        }

        /// <summary>
        /// Casts the generic enum to an <see cref="short"/>.  An exception will be thrown if the <see cref="Enum"/>
        /// does not have a matching SizeOf.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumValue"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short AsShort<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            // Make sure the TEnum is the proper size (hard coded short length for perf).
            if (Unsafe.SizeOf<TEnum>() != 2)
            {
                throw new Exception("Type mismatch.");
            }

            return Unsafe.As<TEnum, short>(ref enumValue);
        }

        /// <summary>
        /// Casts the generic enum to an <see cref="ushort"/>.  An exception will be thrown if the <see cref="Enum"/>
        /// does not have a matching SizeOf.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumValue"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort AsUShort<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            // Make sure the TEnum is the proper size (hard coded short length for perf).
            if (Unsafe.SizeOf<TEnum>() != 2)
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