/*
 * @author            : Blake Pell
 * @initial date      : 2008-08-21
 * @last updated      : 2010-11-17
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System.Data;

namespace Argus.Data
{
    /// <summary>
    /// Copies a table from one database to another.  The only requirement is that the table schema is identical.
    /// </summary>
    /// <remarks>
    /// This class allows for copying from one database table to another with the same schema (even
    /// across database platforms as long as they have an ADO.Net provider).  This code was based off
    /// of C# source from:  http://www.codeproject.com/KB/database/GenericCopyTableDataFcn.aspx
    /// The additions to the original source include table deletion/truncation options, transaction
    /// support, variable symbol toggling, last elapsed time tracking and the ability to toggle on
    /// SQL Server specific data type checking if needed.
    /// </remarks>
    public class CopyTable
    {
        /// <summary>
        /// Enumeration for how deletions should be handled.
        /// </summary>
        public enum DeleteAction
        {
            /// <summary>
            /// Uses a SQL delete command to delete the table data.
            /// </summary>
            DeleteTableData,

            /// <summary>
            /// Uses a SQL truncate command to delete the table data.
            /// </summary>
            TruncateTableData,

            /// <summary>
            /// Performs no deletion action.
            /// </summary>
            NoAction
        }

        /// <summary>
        /// Whether or not to close and dispose of the connections after the CopyTable command has executed.  This is false
        /// by default meaning that the database connections will be closed and disposed of.  If you set this to true, they
        /// will be left open and you will be responsible for closing and cleaning them up.
        /// </summary>
        public bool LeaveConnectionsOpen { get; set; } = false;

        /// <summary>
        /// Whether or not the destination connection is a Sql Server.  This allows us to have Sql Server specific checks in but
        /// not mess up other provider types
        /// </summary>
        public bool IsDestinationSqlServer { get; set; } = false;

        /// <summary>
        /// The symbol to use in the prepare statement.  @ is the default in this class which works with SQL Server.  Other ADO.Net providers may require
        /// another symbol like a ?.
        /// </summary>
        public string VariableSymbol { get; set; } = "@";

        /// <summary>
        /// What action to take with the destination table before the load begins.
        /// </summary>
        public DeleteAction DeleteDestinationTableAction { get; set; } = DeleteAction.NoAction;

        /// <summary>
        /// Whether or not the code should use a database transaction.  The result of this will be that if there are any
        /// errors the entire load will be rolled back (including the delete statement).  The transaction insert performs
        /// much faster than individual inserts.
        /// </summary>
        public bool UseTransaction { get; set; } = false;

        /// <summary>
        /// The time elapsed in milliseconds for the last database copy.
        /// </summary>
        public double TimeElapsed { get; private set; }

        /// <summary>
        /// Copies the data from a source table to a destination table with the same schema.
        /// </summary>
        /// <param name="sourceConnection">The source connection.  This should at a minimum be initialized but it can either be opened or not at this point.</param>
        /// <param name="destinationConnection">The destination connection.  This should at a minimum be initialized but it can either be opened or not at this point.</param>
        /// <param name="sourceSql">The select statement to retrieve the source data, typically something like Select * From YourTableName</param>
        /// <param name="destinationTableName">The name of the destination table, the schema of which should match the source table.</param>
        public void ExecuteCopyTable(IDbConnection sourceConnection, IDbConnection destinationConnection, string sourceSql, string destinationTableName)
        {
            // If exceptions occur we'll let them be handled in the source code that is calling this

            var sw = new Stopwatch();
            sw.Start();

            var sourceCmd = sourceConnection.CreateCommand();
            sourceCmd.CommandText = sourceSql;

            if (sourceConnection.State != ConnectionState.Open)
            {
                sourceConnection.Open();
            }

            if (destinationConnection.State != ConnectionState.Open)
            {
                destinationConnection.Open();
            }

            IDbTransaction destinationTransaction = null;

            if (this.UseTransaction)
            {
                destinationTransaction = destinationConnection.BeginTransaction();
            }

            // Whether or not we should perform some kind of delete action on the destination table first.  
            switch (this.DeleteDestinationTableAction)
            {
                case DeleteAction.DeleteTableData:
                {
                    var cmdDelete = destinationConnection.CreateCommand();

                    if (this.UseTransaction)
                    {
                        // Compiler warning here, it won't get here though without the transaction being initialized
                        cmdDelete.Transaction = destinationTransaction;
                    }

                    cmdDelete.CommandText = $"delete from {destinationTableName}";
                    cmdDelete.ExecuteNonQuery();
                    cmdDelete.Dispose();

                    break;
                }
                case DeleteAction.TruncateTableData:
                {
                    var cmdDelete = destinationConnection.CreateCommand();

                    if (this.UseTransaction)
                    {
                        // Compiler warning here, it won't get here though without the transaction being initialized
                        cmdDelete.Transaction = destinationTransaction;
                    }

                    cmdDelete.CommandText = $"truncate table {destinationTableName}";
                    cmdDelete.ExecuteNonQuery();
                    cmdDelete.Dispose();

                    break;
                }
            }

            var dr = sourceCmd.ExecuteReader();
            var schemaTable = dr.GetSchemaTable();

            var insertCmd = destinationConnection.CreateCommand();

            if (this.UseTransaction)
            {
                insertCmd.Transaction = destinationTransaction;
            }

            string paramsSql = string.Empty;
            string columnsSql = string.Empty;

            // Build the insert statement
            foreach (DataRow row in schemaTable.Rows)
            {
                if (paramsSql.Length > 0)
                {
                    paramsSql += ", ";
                    columnsSql += ", ";
                }

                paramsSql += this.VariableSymbol + row["ColumnName"].ToString().Replace(" ", "");
                columnsSql += $"[{row["ColumnName"]}]";

                var param = insertCmd.CreateParameter();
                param.ParameterName = this.VariableSymbol + row["ColumnName"].ToString().Replace(" ", "");
                param.SourceColumn = row["ColumnName"].ToString();

                if (row["DataType"] is DateTime)
                {
                    param.DbType = DbType.DateTime;
                }

                insertCmd.Parameters.Add(param);
            }

            insertCmd.CommandText = $"insert into [{destinationTableName}] ( {columnsSql} ) values ( {paramsSql} )";

            while (dr.Read())
            {
                foreach (IDbDataParameter param in insertCmd.Parameters)
                {
                    var col = dr[param.SourceColumn];

                    // Special check for SQL Server and datetimes less than 1753
                    if (param.DbType == DbType.DateTime)
                    {
                        if (Convert.IsDBNull(col) == false)
                        {
                            if (Convert.ToDateTime(col).Year < 1753)
                            {
                                param.Value = DBNull.Value;

                                continue;
                            }
                        }
                    }

                    param.Value = col;
                }

                insertCmd.ExecuteNonQuery();
            }

            dr.Close();

            if (this.UseTransaction)
            {
                // Compiler warning here, it won't get here though without the transaction being initialized
                destinationTransaction?.Commit();
                destinationTransaction?.Dispose();
            }

            // Cleanup
            if (this.LeaveConnectionsOpen == false)
            {
                sourceConnection.Close();
                sourceConnection.Dispose();
                destinationConnection.Close();
                destinationConnection.Dispose();
            }

            sw.Stop();
            this.TimeElapsed = sw.ElapsedMilliseconds;
        }

        /// <summary>
        /// Copies the data from a source IDataReader to a destination table with the same schema.  This assumes that the IDataReader is open and not read.
        /// </summary>
        /// <param name="dr">The source DataReader used to copy into the destination.</param>
        /// <param name="destinationConnection">The destination connection.  This should at a minimum be initialized but it can either be opened or not at this point.</param>
        /// <param name="destinationTableName">The name of the destination table, the schema of which should match the source table.</param>
        public void ExecuteCopyTable(IDataReader dr, IDbConnection destinationConnection, string destinationTableName)
        {
            // If exceptions occur we'll let them be handled in the source code that is calling this
            var sw = new Stopwatch();
            sw.Start();

            if (destinationConnection.State != ConnectionState.Open)
            {
                destinationConnection.Open();
            }

            IDbTransaction destinationTransaction = null;

            if (this.UseTransaction)
            {
                destinationTransaction = destinationConnection.BeginTransaction();
            }

            // Whether or not we should perform some kind of delete action on the destination table first.  
            switch (this.DeleteDestinationTableAction)
            {
                case DeleteAction.DeleteTableData:
                {
                    var cmdDelete = destinationConnection.CreateCommand();

                    if (this.UseTransaction)
                    {
                        // Compiler warning here, it won't get here though without the transaction being initialized
                        cmdDelete.Transaction = destinationTransaction;
                    }

                    cmdDelete.CommandText = $"delete from {destinationTableName}";
                    cmdDelete.ExecuteNonQuery();
                    cmdDelete.Dispose();

                    break;
                }
                case DeleteAction.TruncateTableData:
                {
                    var cmdDelete = destinationConnection.CreateCommand();

                    if (this.UseTransaction)
                    {
                        // Compiler warning here, it won't get here though without the transaction being initialized
                        cmdDelete.Transaction = destinationTransaction;
                    }

                    cmdDelete.CommandText = $"truncate table {destinationTableName}";
                    cmdDelete.ExecuteNonQuery();
                    cmdDelete.Dispose();

                    break;
                }
            }

            var schemaTable = dr.GetSchemaTable();
            var insertCmd = destinationConnection.CreateCommand();

            if (this.UseTransaction)
            {
                insertCmd.Transaction = destinationTransaction;
            }

            string paramsSql = string.Empty;
            string columnsSql = string.Empty;

            // Build the insert statement
            foreach (DataRow row in schemaTable.Rows)
            {
                if (paramsSql.Length > 0)
                {
                    paramsSql += ", ";
                    columnsSql += ", ";
                }

                paramsSql += this.VariableSymbol + row["ColumnName"].ToString().Replace(" ", "");
                columnsSql += $"[{row["ColumnName"]}]";

                var param = insertCmd.CreateParameter();
                param.ParameterName = this.VariableSymbol + row["ColumnName"].ToString().Replace(" ", "");
                param.SourceColumn = row["ColumnName"].ToString();

                if (row["DataType"] is DateTime)
                {
                    param.DbType = DbType.DateTime;
                }

                insertCmd.Parameters.Add(param);
            }

            insertCmd.CommandText = $"insert into [{destinationTableName}] ( {columnsSql} ) values ( {paramsSql} )";

            while (dr.Read())
            {
                foreach (IDbDataParameter param in insertCmd.Parameters)
                {
                    var col = dr[param.SourceColumn];

                    // Special check for SQL Server and datetimes less than 1753
                    if (param.DbType == DbType.DateTime)
                    {
                        if (Convert.IsDBNull(col) == false)
                        {
                            if (Convert.ToDateTime(col).Year < 1753)
                            {
                                param.Value = DBNull.Value;

                                continue;
                            }
                        }
                    }

                    param.Value = col;
                }

                insertCmd.ExecuteNonQuery();
            }

            dr.Close();

            if (this.UseTransaction)
            {
                // Compiler warning here, it won't get here though without the transaction being initialized
                destinationTransaction?.Commit();
                destinationTransaction?.Dispose();
            }

            // Cleanup
            if (this.LeaveConnectionsOpen == false)
            {
                destinationConnection.Close();
                destinationConnection.Dispose();
            }

            sw.Stop();
            this.TimeElapsed = sw.ElapsedMilliseconds;
        }
    }
}