using System;
using System.Data;
using System.Diagnostics;

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
        //*********************************************************************************************************************
        //
        //             Class:  CopyTable
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  08/21/2008
        //      Last Updated:  04/10/2016
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>
        public CopyTable()
        {
        }

        /// <summary>
        /// Copies the data from a source table to a destination table with the same schema.
        /// </summary>
        /// <param name="sourceConnection">The source connection.  This should at a minimum be initialized but it can either be opened or not at this point.</param>
        /// <param name="destinationConnection">The destination connection.  This should at a minimum be initialized but it can either be opened or not at this point.</param>
        /// <param name="sourceSql">The select statement to retrieve the source data, typically something like Select * From YourTableName</param>
        /// <param name="destinationTableName">The name of the destination table, the schema of which should match the source table.</param>
        /// <remarks></remarks>
        public void ExecuteCopyTable(IDbConnection sourceConnection, IDbConnection destinationConnection, string sourceSql, string destinationTableName)
        {
            // If exceptions occur we'll let them be handled in the source code that is calling this

            Stopwatch sw = new Stopwatch();
            sw.Start();

            IDbCommand sourceCmd = sourceConnection.CreateCommand();
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
            if (_useTransaction == true)
            {
                destinationTransaction = destinationConnection.BeginTransaction();
            }

            // Whether or not we should perform some kind of delete action on the destination table first.  
            switch (_deleteDestinationTableAction)
            {
                case DeleteAction.DeleteTableData:
                    {
                        IDbCommand cmdDelete = destinationConnection.CreateCommand();

                        if (_useTransaction == true)
                        {
                            // Compiler warning here, it won't get here though without the transaction being initialized
                            cmdDelete.Transaction = destinationTransaction;
                        }

                        cmdDelete.CommandText = string.Format("delete from {0}", destinationTableName);
                        cmdDelete.ExecuteNonQuery();
                        cmdDelete.Dispose();
                        cmdDelete = null;
                        break;
                    }
                case DeleteAction.TruncateTableData:
                    {
                        IDbCommand cmdDelete = destinationConnection.CreateCommand();

                        if (_useTransaction == true)
                        {
                            // Compiler warning here, it won't get here though without the transaction being initialized
                            cmdDelete.Transaction = destinationTransaction;
                        }

                        cmdDelete.CommandText = string.Format("truncate table {0}", destinationTableName);
                        cmdDelete.ExecuteNonQuery();
                        cmdDelete.Dispose();
                        cmdDelete = null;
                        break;
                    }
            }

            IDataReader dr = sourceCmd.ExecuteReader();
            DataTable schemaTable = dr.GetSchemaTable();

            IDbCommand insertCmd = destinationConnection.CreateCommand();

            if (_useTransaction == true)
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

                paramsSql += _variableSymbol + row["ColumnName"].ToString().Replace(" ", "");
                columnsSql += string.Format("[{0}]", row["ColumnName"].ToString());

                IDbDataParameter param = insertCmd.CreateParameter();
                param.ParameterName = _variableSymbol + row["ColumnName"].ToString().Replace(" ", "");
                param.SourceColumn = row["ColumnName"].ToString();

                if (row["DataType"] is System.DateTime)
                {
                    param.DbType = DbType.DateTime;
                }

                insertCmd.Parameters.Add(param);

            }

            insertCmd.CommandText = string.Format("insert into [{0}] ( {1} ) values ( {2} )", destinationTableName, columnsSql, paramsSql);

            while (dr.Read())
            {
                foreach (IDbDataParameter param in insertCmd.Parameters)
                {
                    object col = dr[param.SourceColumn];

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
            dr = null;

            if (_useTransaction == true)
            {
                // Compiler warning here, it won't get here though without the transaction being initialized
                destinationTransaction.Commit();
                destinationTransaction.Dispose();
            }

            // Cleanup
            if (_leaveConnectionsOpen == false)
            {
                sourceConnection.Close();
                sourceConnection.Dispose();
                destinationConnection.Close();
                destinationConnection.Dispose();
            }

            sw.Stop();
            _timeElapsed = sw.ElapsedMilliseconds;

        }

        /// <summary>
        /// Copies the data from a source IDataReader to a destination table with the same schema.  This assumes that the IDataReader is open and not read.
        /// </summary>
        /// <param name="destinationConnection">The destination connection.  This should at a minimum be initialized but it can either be opened or not at this point.</param>
        /// <param name="destinationTableName">The name of the destination table, the schema of which should match the source table.</param>
        /// <remarks></remarks>
        public void ExecuteCopyTable(IDataReader dr, IDbConnection destinationConnection, string destinationTableName)
        {
            // If exceptions occur we'll let them be handled in the source code that is calling this
            Stopwatch sw = new Stopwatch();
            sw.Start();

            if (destinationConnection.State != ConnectionState.Open)
            {
                destinationConnection.Open();
            }

            IDbTransaction destinationTransaction = null;
            if (_useTransaction == true)
            {
                destinationTransaction = destinationConnection.BeginTransaction();
            }

            // Whether or not we should perform some kind of delete action on the destination table first.  
            switch (_deleteDestinationTableAction)
            {
                case DeleteAction.DeleteTableData:
                    {
                        IDbCommand cmdDelete = destinationConnection.CreateCommand();

                        if (_useTransaction == true)
                        {
                            // Compiler warning here, it won't get here though without the transaction being initialized
                            cmdDelete.Transaction = destinationTransaction;
                        }

                        cmdDelete.CommandText = string.Format("delete from {0}", destinationTableName);
                        cmdDelete.ExecuteNonQuery();
                        cmdDelete.Dispose();
                        cmdDelete = null;
                        break;
                    }
                case DeleteAction.TruncateTableData:
                    {
                        IDbCommand cmdDelete = destinationConnection.CreateCommand();

                        if (_useTransaction == true)
                        {
                            // Compiler warning here, it won't get here though without the transaction being initialized
                            cmdDelete.Transaction = destinationTransaction;
                        }

                        cmdDelete.CommandText = string.Format("truncate table {0}", destinationTableName);
                        cmdDelete.ExecuteNonQuery();
                        cmdDelete.Dispose();
                        cmdDelete = null;
                        break;
                    }
            }

            DataTable schemaTable = dr.GetSchemaTable();

            IDbCommand insertCmd = destinationConnection.CreateCommand();

            if (_useTransaction == true)
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

                paramsSql += _variableSymbol + row["ColumnName"].ToString().Replace(" ", "");
                columnsSql += string.Format("[{0}]", row["ColumnName"].ToString());

                IDbDataParameter param = insertCmd.CreateParameter();
                param.ParameterName = _variableSymbol + row["ColumnName"].ToString().Replace(" ", "");
                param.SourceColumn = row["ColumnName"].ToString();

                if (row["DataType"] is System.DateTime)
                {
                    param.DbType = DbType.DateTime;
                }

                insertCmd.Parameters.Add(param);

            }

            insertCmd.CommandText = string.Format("insert into [{0}] ( {1} ) values ( {2} )", destinationTableName, columnsSql, paramsSql);

            while (dr.Read())
            {
                foreach (IDbDataParameter param in insertCmd.Parameters)
                {
                    object col = dr[param.SourceColumn];

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
            dr = null;

            if (_useTransaction == true)
            {
                // Compiler warning here, it won't get here though without the transaction being initialized
                destinationTransaction.Commit();
                destinationTransaction.Dispose();
            }

            // Cleanup
            if (_leaveConnectionsOpen == false)
            {
                destinationConnection.Close();
                destinationConnection.Dispose();
            }

            sw.Stop();
            _timeElapsed = sw.ElapsedMilliseconds;

        }

        private bool _leaveConnectionsOpen = false;
        /// <summary>
        /// Whether or not to close and dispose of the connections after the CopyTable command has executed.  This is false
        /// by default meaning that the database connections will be closed and disposed of.  If you set this to true, they
        /// will be left open and you will be responsible for closing and cleaning them up.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool LeaveConnectionsOpen
        {
            get { return _leaveConnectionsOpen; }
            set { _leaveConnectionsOpen = value; }
        }

        private bool _isDestinationSqlServer = false;
        /// <summary>
        /// Whether or not the destination connection is a Sql Server.  This allows us to have Sql Server specific checks in but
        /// not mess up other provider types
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsDestinationSqlServer
        {
            get { return _isDestinationSqlServer; }
            set { _isDestinationSqlServer = value; }
        }

        private string _variableSymbol = "@";
        /// <summary>
        /// The symbol to use in the prepare statement.  @ is the default in this class which works with SQL Server.  Other ADO.Net providers may require
        /// another symbol like a ?.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string VariableSymbol
        {
            get { return _variableSymbol; }
            set { _variableSymbol = value; }
        }

        /// <summary>
        /// Enumeration for how deletions should be handled.
        /// </summary>
        /// <remarks></remarks>
        public enum DeleteAction
        {
            DeleteTableData,
            TruncateTableData,
            NoAction
        }

        private DeleteAction _deleteDestinationTableAction = DeleteAction.NoAction;
        /// <summary>
        /// What action to take with the destination table before the load begins.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DeleteAction DeleteDestinationTableAction
        {
            get { return _deleteDestinationTableAction; }
            set { _deleteDestinationTableAction = value; }
        }

        private bool _useTransaction = false;
        /// <summary>
        /// Whether or not the code should use a database transaction.  The result of this will be that if there are any
        /// errors the entire load will be rolled back (including the delete statement).  The transaction insert performs
        /// much faster than individual inserts.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool UseTransaction
        {
            get { return _useTransaction; }
            set { _useTransaction = value; }
        }

        private double _timeElapsed = 0;
        /// <summary>
        /// The time elapsed in milleseconds for the last database copy.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double TimeElapsed
        {
            get { return _timeElapsed; }
        }

    }

}