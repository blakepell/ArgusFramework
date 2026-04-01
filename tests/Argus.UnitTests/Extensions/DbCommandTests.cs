/*
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using Argus.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Xunit;

namespace Argus.UnitTests
{
    public class DbCommandTests
    {
        [Fact]
        public void AddWithValue_AddsParameterWithNameAndValue()
        {
            using var conn = new TestDbConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Test WHERE Id = @Id";

            cmd.AddWithValue("@Id", 42);

            Assert.Equal(1, cmd.Parameters.Count);
        }

        [Fact]
        public void AddWithValue_WithDbType_AddsParameterCorrectly()
        {
            using var conn = new TestDbConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Test WHERE Name = @Name";

            cmd.AddWithValue("@Name", "test", DbType.String);

            Assert.Equal(1, cmd.Parameters.Count);
        }

        [Fact]
        public void AddWithValue_WithDbTypeAndSize_AddsParameterCorrectly()
        {
            using var conn = new TestDbConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Test WHERE Name = @Name";

            cmd.AddWithValue("@Name", "test", DbType.String, 50);

            Assert.Equal(1, cmd.Parameters.Count);
        }

        [Fact]
        public void ActualCommandText_ReplacesStringParameter()
        {
            using var conn = new TestDbConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Test WHERE Name = @Name";
            cmd.AddWithValue("@Name", "John", DbType.String);

            var result = cmd.ActualCommandText();

            Assert.Contains("'John'", result);
            Assert.DoesNotContain("@Name", result);
        }

        [Fact]
        public void ActualCommandText_ReplacesIntParameter()
        {
            using var conn = new TestDbConnection();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Test WHERE Id = @Id";
            cmd.AddWithValue("@Id", 42, DbType.Int32);

            var result = cmd.ActualCommandText();

            Assert.Contains("42", result);
            Assert.DoesNotContain("@Id", result);
        }

        /// <summary>
        /// Minimal IDbConnection/IDbCommand implementation for testing purposes.
        /// </summary>
        private class TestDbConnection : IDbConnection
        {
            public string ConnectionString { get; set; } = "";
            public int ConnectionTimeout => 30;
            public string Database => "";
            public ConnectionState State => ConnectionState.Closed;
            public IDbTransaction BeginTransaction() => null!;
            public IDbTransaction BeginTransaction(IsolationLevel il) => null!;
            public void ChangeDatabase(string databaseName) { }
            public void Close() { }
            public IDbCommand CreateCommand() => new TestDbCommand();
            public void Open() { }
            public void Dispose() { }
        }

        private class TestDbCommand : IDbCommand
        {
            private readonly TestParameterCollection _parameters = new();

            public string CommandText { get; set; } = "";
            public int CommandTimeout { get; set; } = 30;
            public CommandType CommandType { get; set; } = CommandType.Text;
            public IDbConnection? Connection { get; set; }
            public IDataParameterCollection Parameters => _parameters;
            public IDbTransaction? Transaction { get; set; }
            public UpdateRowSource UpdatedRowSource { get; set; }

            public void Cancel() { }
            public IDbDataParameter CreateParameter() => new TestParameter();
            public void Dispose() { }
            public int ExecuteNonQuery() => 0;
            public IDataReader ExecuteReader() => null!;
            public IDataReader ExecuteReader(CommandBehavior behavior) => null!;
            public object? ExecuteScalar() => null;
            public void Prepare() { }
        }

        private class TestParameter : IDbDataParameter
        {
            public byte Precision { get; set; }
            public byte Scale { get; set; }
            public int Size { get; set; }
            public DbType DbType { get; set; }
            public ParameterDirection Direction { get; set; }
            public bool IsNullable => false;
            public string ParameterName { get; set; } = "";
            public string SourceColumn { get; set; } = "";
            public DataRowVersion SourceVersion { get; set; }
            public object? Value { get; set; }
        }

        private class TestParameterCollection : IDataParameterCollection
        {
            private readonly System.Collections.Generic.List<object> _list = new();

            public object this[string parameterName]
            {
                get => _list.First(p => ((IDbDataParameter)p).ParameterName == parameterName);
                set { }
            }

            public object? this[int index]
            {
                get => _list[index];
                set { if (value != null) _list[index] = value; }
            }

            public bool IsFixedSize => false;
            public bool IsReadOnly => false;
            public bool IsSynchronized => false;
            public int Count => _list.Count;
            public object SyncRoot => _list;

            public int Add(object? value) { if (value != null) _list.Add(value); return _list.Count - 1; }
            public void Clear() => _list.Clear();
            public bool Contains(object? value) => _list.Contains(value!);
            public bool Contains(string parameterName) => _list.Any(p => ((IDbDataParameter)p).ParameterName == parameterName);
            public void CopyTo(System.Array array, int index) { }
            public System.Collections.IEnumerator GetEnumerator() => _list.GetEnumerator();
            public int IndexOf(object? value) => _list.IndexOf(value!);
            public int IndexOf(string parameterName) => _list.FindIndex(p => ((IDbDataParameter)p).ParameterName == parameterName);
            public void Insert(int index, object? value) { if (value != null) _list.Insert(index, value); }
            public void Remove(object? value) => _list.Remove(value!);
            public void RemoveAt(int index) => _list.RemoveAt(index);
            public void RemoveAt(string parameterName) => _list.RemoveAll(p => ((IDbDataParameter)p).ParameterName == parameterName);
        }
    }
}
