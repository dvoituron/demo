using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace KendoUIMVC5.ReferenceTablesEditor
{
    public class ReferenceTables
    {
        private const string XML_QUERIES_FILENAME = "ReferenceTablesEditor.ReferenceTablesQueries.xml";

        public ReferenceTables()
        {
            this.ReadXmlQueries();
        }

        public string ConnectionString { get; set; }

        public IEnumerable<TableDefinition> Table { get; set; }

        public DataTable Read(string name)
        {
            return GetTableDefinition(name).GetData();
        }

        public void Update(string name, NameValueCollection items)
        {
            var collection = items.Cast<string>().Select(key => new KeyValuePair<string, string>(key, items[key]));
            this.Update(name, collection);
        }

        public void Update(string name, IEnumerable<KeyValuePair<string, string>> items)
        {
            GetTableDefinition(name).Update(items);
        }

        public void Delete(string name, NameValueCollection items)
        {
            var collection = items.Cast<string>().Select(key => new KeyValuePair<string, string>(key, items[key]));
            this.Delete(name, collection);
        }

        public void Delete(string name, IEnumerable<KeyValuePair<string, string>> items)
        {
            GetTableDefinition(name).Delete(items);
        }

        public void Add(string name, NameValueCollection items)
        {
            var collection = items.Cast<string>().Select(key => new KeyValuePair<string, string>(key, items[key]));
            this.Add(name, collection);
        }

        public void Add(string name, IEnumerable<KeyValuePair<string, string>> items)
        {
            GetTableDefinition(name).Add(items);
        }

        private TableDefinition GetTableDefinition(string name)
        {
            var table = this.Table.First(t => t.Name == name);
            table.ConnectionString = this.ConnectionString;
            return table;
        }

        private void ReadXmlQueries()
        {
            // Read resource file
            XDocument xml;
            string resourceName = this.GetType().Assembly.GetManifestResourceNames().First(i => i.EndsWith(XML_QUERIES_FILENAME));
            using (Stream stream = this.GetType().Assembly.GetManifestResourceStream(resourceName))
            {
                xml = XDocument.Load(stream);
            }

            // Read queries
            List<TableDefinition> tables = new List<TableDefinition>();
            foreach (var table in xml.Root.Elements("table"))
            {
                tables.Add(new TableDefinition()
                {
                    Name = table.Attribute("name").Value,
                    SelectCommand = table.Element("select").Value,
                    Identifiers = table.Elements("identifier").Select(e => e.Value),
                    InsertCommands = table.Elements("insert").Select(e => e.Value),
                    UpdateCommands = table.Elements("update").Select(e => e.Value),
                    DeleteCommands = table.Elements("delete").Select(e => e.Value),
                });
            }
            this.Table = tables;
        }        
    }
}