/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2009-05-14
 * @last updated      : 2012-01-09
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT
 */

using System;
using System.Data;
using System.Text;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for the DbCommand types.  Most will target IDbCommand but this will also include specialized
    /// extensions for types like SqlCommand.
    /// </summary>
    public static class DbCommandExtensions
    {
        /// <summary>
        /// Shows the SQL of the CommandText property but replaces all of the parameters with their actual values.
        /// </summary>
        /// <param name="cmd"></param>
        /// <remarks>
        /// This will allow you to see the SQL with the real values behind the parameters so that you take that text
        /// and paste it straight into a SQL editor to run without having to swap out parameters for values.
        /// A known issue occurs when parameter names overlap, for instance @typ_cd1, @typ_cd11.  This function loops through
        /// the parameters as they were added, thus replacing @typ_cd1 would also replace @typ_cd1 in @typ_cd11 leaving a stray
        /// one at the end.  This method should loop through the values in descending order by length.  The alternative would have
        /// the query to use parameters like @type_cd001 instead of @type_cd1.  This would work with this command up to 999 parameters
        /// which will never occur (right?).
        /// </remarks>
        public static string ActualCommandText(this IDbCommand cmd)
        {
            var sb = new StringBuilder(cmd.CommandText);

            foreach (IDataParameter p in cmd.Parameters)
            {
                switch (p.DbType)
                {
                    case DbType.AnsiString:
                    case DbType.AnsiStringFixedLength:
                    case DbType.Date:
                    case DbType.DateTime:
                    case DbType.DateTime2:
                    case DbType.Guid:
                    case DbType.String:
                    case DbType.StringFixedLength:
                    case DbType.Time:
                    case DbType.Xml:
                        sb = sb.Replace(p.ParameterName, $"'{p.Value.ToString().Replace("'", "''")}'");

                        break;

                    default:
                        if (p.Value != null && p.Value != DBNull.Value)
                        {
                            sb = sb.Replace(p.ParameterName, p.Value.ToString());
                        }
                        else
                        {
                            sb = sb.Replace(p.ParameterName, "null");
                        }

                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Adds a parameter into the IDbCommand and sets it's name, value and type.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <param name="dbType"></param>
        public static void AddWithValue(this IDbCommand cmd, string paramName, object paramValue, DbType dbType)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.DbType = dbType;
            param.Value = paramValue;
            cmd.Parameters.Add(param);
        }

        /// <summary>
        /// Adds a parameter into the IDbCommand and name and value.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public static void AddWithValue(this IDbCommand cmd, string paramName, object paramValue)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue;
            cmd.Parameters.Add(param);
        }

        /// <summary>
        /// Adds a parameter into the IDbCommand and sets it's name, value and type.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        public static void AddWithValue(this IDbCommand cmd, string paramName, object paramValue, DbType dbType, int size)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.DbType = dbType;
            param.Value = paramValue;
            param.Size = size;
            cmd.Parameters.Add(param);
        }
    }
}