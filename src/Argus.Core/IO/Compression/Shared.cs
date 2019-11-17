// Shared.cs
//
// Copyright (c) 2006, 2007 Microsoft Corporation.  All rights reserved.
//
// Part of an implementation of a zipfile class library. 
// See the file ZipFile.cs for further information.
//
// Tue, 27 Mar 2007  15:30

using System;
using System.IO;

namespace Argus.IO.Compression
{
    internal class Shared
    {
        protected internal static string StringFromBuffer(byte[] buf, int start, int maxlength)
        {
            int i;
            var c = new char[maxlength];

            for (i = 0; i < maxlength && i < buf.Length && buf[i] != 0; i++)
            {
                c[i] = (char) buf[i]; // System.BitConverter.ToChar(buf, start+i*2);
            }

            string s = new string(c, 0, i);

            return s;
        }

        protected internal static int ReadSignature(Stream s)
        {
            int n = 0;
            var sig = new byte[4];
            n = s.Read(sig, 0, sig.Length);

            if (n != sig.Length)
            {
                throw new Exception("Could not read signature - no data!");
            }

            int signature = ((sig[3] * 256 + sig[2]) * 256 + sig[1]) * 256 + sig[0];

            return signature;
        }

        protected internal static long FindSignature(Stream s, int SignatureToFind)
        {
            long startingPosition = s.Position;

            int BATCH_SIZE = 1024;
            var targetBytes = new byte[4];
            targetBytes[0] = (byte) (SignatureToFind >> 24);
            targetBytes[1] = (byte) ((SignatureToFind & 0x00FF0000) >> 16);
            targetBytes[2] = (byte) ((SignatureToFind & 0x0000FF00) >> 8);
            targetBytes[3] = (byte) (SignatureToFind & 0x000000FF);
            var batch = new byte[BATCH_SIZE];
            int n = 0;
            bool success = false;

            do
            {
                n = s.Read(batch, 0, batch.Length);

                if (n != 0)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (batch[i] == targetBytes[3])
                        {
                            s.Seek(i - n, SeekOrigin.Current);
                            int sig = ReadSignature(s);
                            success = sig == SignatureToFind;

                            if (!success)
                            {
                                s.Seek(-3, SeekOrigin.Current);
                            }

                            break; // out of for loop
                        }
                    }
                }
                else
                {
                    break;
                }

                if (success)
                {
                    break;
                }
            } while (true);

            if (!success)
            {
                s.Seek(startingPosition, SeekOrigin.Begin);

                return -1; // or throw?
            }

            // subtract 4 for the signature.
            long bytesRead = s.Position - startingPosition - 4;

            // number of bytes read, should be the same as compressed size of file            
            return bytesRead;
        }

        protected internal static DateTime PackedToDateTime(int packedDateTime)
        {
            short packedTime = (short) (packedDateTime & 0x0000ffff);
            short packedDate = (short) ((packedDateTime & 0xffff0000) >> 16);

            int year = 1980 + ((packedDate & 0xFE00) >> 9);
            int month = (packedDate & 0x01E0) >> 5;
            int day = packedDate & 0x001F;


            int hour = (packedTime & 0xF800) >> 11;
            int minute = (packedTime & 0x07E0) >> 5;
            int second = packedTime & 0x001F;

            var d = DateTime.Now;

            try
            {
                d = new DateTime(year, month, day, hour, minute, second, 0);
            }
            catch
            {
                Console.Write("\nInvalid date/time?:\nyear: {0} ", year);
                Console.Write("month: {0} ", month);
                Console.WriteLine("day: {0} ", day);
                Console.WriteLine("HH:MM:SS= {0}:{1}:{2}", hour, minute, second);
            }

            return d;
        }

        protected internal static int DateTimeToPacked(DateTime time)
        {
            ushort packedDate = (ushort) ((time.Day & 0x0000001F) | ((time.Month << 5) & 0x000001E0) | (((time.Year - 1980) << 9) & 0x0000FE00));
            ushort packedTime = (ushort) ((time.Second & 0x0000001F) | ((time.Minute << 5) & 0x000007E0) | ((time.Hour << 11) & 0x0000F800));

            return (int) ((uint) (packedDate << 16) | packedTime);
        }
    }
}