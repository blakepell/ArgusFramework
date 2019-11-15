using System;
using System.Data;

namespace Argus.Data
{
    /// <summary>
    /// A class to handle persisting a NameValueCollection to a database.  It should be noted that this class does not yet handle
    /// concurrent users, it was designed for easy storage of collections in a single user application.
    /// </summary>
    /// <remarks></remarks>
    public class DbNameValueCollection : System.Collections.Specialized.NameValueCollection
    {
        //*********************************************************************************************************************
        //
        //             Class:  DbNameValueCollection
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  10/07/2011
        //      Last Updated:  04/10/2014
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        #region "Contructors"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conn">The database connection to use.  The connection must be initialized but does not need to be open yet.  This class will not cleanup or close the connection when it's done with it.</param>
        /// <param name="tableName">The name of the table the NameValueCollection should be stored in.</param>
        /// <param name="fieldKeyName">The database field name for the key field.</param>
        /// <param name="fieldValueName">The database field name for the value field.</param>
        /// <remarks></remarks>
        public DbNameValueCollection(IDbConnection conn, string tableName, string fieldKeyName, string fieldValueName)
        {
            this.TableName = tableName;
            this.FieldKeyName = fieldKeyName;
            this.FieldValueName = fieldValueName;

            if (conn == null)
            {
                throw new NullReferenceException("The database connection cannot be null.");
            }

            _dbConnection = conn;

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            
            IDbCommand command = conn.CreateCommand();
            command.CommandText = $"SELECT {fieldKeyName.Replace("'", "''")}, {fieldValueName.Replace("'", "''")} FROM {tableName.Replace("'", "''")}";
            IDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                // A null key will be skipped
                if (Convert.IsDBNull(dr[fieldKeyName]) == true)
                {
                    continue;
                }

                // A null value will be converted to default of blank
                if (Convert.IsDBNull(dr[fieldValueName]) == true)
                {
                    this.Add(dr[fieldKeyName].ToString(), "");
                }
                else {
                    // The value is good, add it
                    this.Add(dr[fieldKeyName].ToString(), dr[fieldValueName].ToString());
                }
            }

            dr.Close();
            dr = null;
            command.Dispose();
            command = null;
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Clears and refreshes the contents of the collection from the database.
        /// </summary>
        /// <remarks></remarks>
        public void RefreshCollectionFromDatabase()
        {
            if (_dbConnection == null)
            {
                throw new NullReferenceException("The database connection cannot be null.");
            }

            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            IDbCommand command = _dbConnection.CreateCommand();
            command.CommandText = string.Format("SELECT {0}, {1} FROM {2}", FieldKeyName.Replace("'", "''"), FieldValueName.Replace("'", "''"), TableName.Replace("'", "''"));

            IDataReader dr = command.ExecuteReader();

            this.Clear();

            while (dr.Read())
            {
                // A null key will be skipped
                if (Convert.IsDBNull(dr[FieldKeyName]) == true)
                {
                    continue;
                }

                // A null value will be converted to default of blank
                if (Convert.IsDBNull(dr[FieldValueName]) == true)
                {
                    this.Add(dr[FieldKeyName].ToString(), "");
                }
                else {
                    // The value is good, add it
                    this.Add(dr[FieldKeyName].ToString(), dr[FieldValueName].ToString());
                }
            }

            dr.Close();
            dr = null;
            command.Dispose();
            command = null;

            _changed = false;
        }

        /// <summary>
        /// Commits the changes to the database only if any have taken place.
        /// </summary>
        /// <remarks>
        /// This method will use a database transaction, if any records fail to be inserted then the entire transaction
        /// will be rolled back and no changes will occured on the database side.  After the rollback occurs the exception
        /// will be thrown.
        /// </remarks>
        public void CommitChanges()
        {
            if (_changed == false)
            {
                return;
            }

            if (_dbConnection == null)
            {
                throw new NullReferenceException("The database connection cannot be null.");
            }

            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            IDbTransaction transaction = _dbConnection.BeginTransaction();
            IDbCommand command = _dbConnection.CreateCommand();

            try
            {
                command.Transaction = transaction;
                command.CommandText = string.Format("DELETE from {0}", this.TableName.Replace("'", "''"));
                command.ExecuteNonQuery();

                command.CommandText = string.Format("INSERT INTO {0} ({1}, {2}) VALUES ({3}keyName, {3}valueName)", this.TableName.Replace("'", "''"), this.FieldKeyName.Replace("'", "''"), this.FieldValueName.Replace("'", "''"), this.ParameterQualifier);

                System.Data.IDbDataParameter pKeyName = command.CreateParameter();
                pKeyName.ParameterName = string.Format("{0}keyName", this.ParameterQualifier);
                pKeyName.DbType = DbType.String;
                command.Parameters.Add(pKeyName);

                System.Data.IDbDataParameter pKeyValue = command.CreateParameter();
                pKeyValue.ParameterName = string.Format("{0}valueName", this.ParameterQualifier);
                pKeyValue.DbType = DbType.String;
                command.Parameters.Add(pKeyValue);

                foreach (string key in this.Keys)
                {                    
                    ((IDbDataParameter)command.Parameters[string.Format("{0}keyName", this.ParameterQualifier)]).Value = key;
                    ((IDbDataParameter)command.Parameters[string.Format("{0}valueName", this.ParameterQualifier)]).Value = this[key].ToString();
                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch 
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                command.Dispose();
                command = null;
                transaction.Dispose();
            }

            _changed = false;
        }

        /// <summary>
        /// Removes an item from the NameValueCollection
        /// </summary>
        /// <param name="name"></param>
        /// <remarks></remarks>
        public override void Remove(string name)
        {
            _changed = true;
            base.Remove(name);
        }

        /// <summary>
        /// Adds an item to the NameValueCollection.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <remarks></remarks>
        public override void Add(string name, string value)
        {
            _changed = true;
            base.Add(name, value);
        }

        /// <summary>
        /// Clears the NameValueCollection
        /// </summary>
        /// <remarks></remarks>
        public override void Clear()
        {
            _changed = true;
            base.Clear();
        }

        /// <summary>
        /// Sets a value based on it's provided key.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <remarks></remarks>
        public override void Set(string name, string value)
        {
            _changed = true;
            base.Set(name, value);
        }

        private string _parameterQualifier = "@";
        /// <summary>
        /// The paramater qualifier that should be used.  SQL Server = @, MySql = ? and Oracle = :
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ParameterQualifier
        {
            get { return _parameterQualifier; }
            set { _parameterQualifier = value; }
        }

        #endregion

        #region "Properties"

        private bool _changed = false;
        /// <summary>
        /// Whether or not the NameValueCollection has changed since it was last committed to the database.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Changed
        {
            get { return _changed; }
        }

        private string _tableName = "";
        /// <summary>
        /// The name of the table in the database that holds the NameValueCollection.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>
        /// This table will need to have 2 character fields, one for the key and one for the associated value.
        /// </remarks>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        private string _fieldKeyName = "";
        /// <summary>
        /// The name of the database field which stores the key.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string FieldKeyName
        {
            get { return _fieldKeyName; }
            set { _fieldKeyName = value; }
        }

        private string _fieldValueName = "";
        /// <summary>
        /// The name of the database field that stores the value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string FieldValueName
        {
            get { return _fieldValueName; }
            set { _fieldValueName = value; }
        }

        private IDbConnection _dbConnection;
        /// <summary>
        /// The underlaying database connection.  
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public IDbConnection DbConnection
        {
            get { return _dbConnection; }
        }

        #endregion

    }

}