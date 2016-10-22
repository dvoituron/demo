using Apps72.Dev.Data;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KendoUIMVC5.ReferenceTablesEditor
{
    public class TableDefinition
    {
        internal string ConnectionString { get; set; }

        public string Name { get; set; }

        public string SelectCommand { get; set; }

        public IEnumerable<string> Identifiers { get; set; }

        public IEnumerable<string> InsertCommands { get; set; }

        public IEnumerable<string> UpdateCommands { get; set; }

        public IEnumerable<string> DeleteCommands { get; set; }

        internal DataTable GetData()
        {
            using (var cmd = new SqlDatabaseCommand(this.ConnectionString))
            {
                // Read data
                cmd.CommandText.Append(this.SelectCommand);
                DataTable data = cmd.ExecuteTable();

                // Assign PrimaryKeys
                List<DataColumn> primaryKeys = new List<DataColumn>();
                foreach (DataColumn col in data.Columns)
                {
                    if (this.Identifiers.Contains(col.ColumnName))
                    {
                        primaryKeys.Add(col);
                    }
                }
                data.PrimaryKey = primaryKeys.ToArray();

                return data;
            }
        }

        internal void Update(IEnumerable<KeyValuePair<string, string>> items)
        {
            ThrowSqlExceptionForInvalidIdentifiers(items);

            foreach (var sql in this.UpdateCommands)
            {
                using (var cmd = new SqlDatabaseCommand(this.ConnectionString))
                {
                    cmd.CommandText.Append(sql);
                    foreach (var param in items)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal void Delete(IEnumerable<KeyValuePair<string, string>> items)
        {
            ThrowSqlExceptionForInvalidIdentifiers(items);

            foreach (var sql in this.DeleteCommands)
            {
                using (var cmd = new SqlDatabaseCommand(this.ConnectionString))
                {
                    cmd.CommandText.Append(sql);
                    foreach (var param in items)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal void Add(IEnumerable<KeyValuePair<string, string>> items)
        {
            ThrowSqlExceptionForInvalidIdentifiers(items);

            foreach (var sql in this.InsertCommands)
            {
                using (var cmd = new SqlDatabaseCommand(this.ConnectionString))
                {
                    cmd.CommandText.Append(sql);
                    foreach (var param in items)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ThrowSqlExceptionForInvalidIdentifiers(IEnumerable<KeyValuePair<string, string>> items)
        {
            foreach (var id in this.Identifiers)
            {
                if (!items.Any(i => i.Key == id) || String.IsNullOrEmpty(items.First(i => i.Key == id).Value))
                    throw new ArgumentNullException($"The field '{id}' must be set.");
            }
        }
    }
}