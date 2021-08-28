using CoreFramework.ObjectGraphBatchValidation.Models;
using CsvHelper.Configuration.Attributes;
using System.Data;
using System.Linq;

namespace CoreFramework.ObjectGraphBatchValidation
{
    public static class DataTablesExtensions
    {
        public static bool HasValueFor(this DataRow theDataRow, string column) =>
            theDataRow.Table.Columns.Contains(column) &&
            !string.IsNullOrWhiteSpace(theDataRow[column].ToString());

        public static bool HasValuesFor(this DataRow theDataRow, params string[] columns) =>
            columns.All(theDataRow.HasValueFor);

        public static DataRow[] RegressToRedundantColumnsForBackwardCodeCompatibility(this DataRow[] dataRows)
        {
            var inputDataModelKeys = typeof(InputData).GetProperties()
                .SelectMany(propertyInfo =>
                {
                    return propertyInfo.GetCustomAttributes(false).OfType<NameAttribute>()
                    .SelectMany(name => name.Names).ToList();
                }).ToList();

            var dataRowKeys = dataRows.First().Table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            var allKeys = inputDataModelKeys.Union(dataRowKeys).Distinct().ToList();

            var dataTable = new DataTable();
            dataTable.Columns.AddRange(allKeys.Select(key => new DataColumn(key, typeof(string))).ToArray());
            dataRows.ToList().ForEach(item =>
            {
                var row = dataTable.NewRow();
                item.Table.Columns
                    .Cast<DataColumn>()
                    .Select(dataColumn => dataColumn.ColumnName)
                    .ToList()
                    .ForEach(key => row[key] = item[key]);
                dataTable.Rows.Add(row);
            });

            return dataTable.Rows.Cast<DataRow>().ToArray();
        }

        public static DataRow[] ToDataTableRows(this Batch theBatch)
        {
            var evenBatch = theBatch.ToEvenBatch();
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(evenBatch.First().Keys.Select(key => new DataColumn(key, typeof(string))).ToArray());
            theBatch.ForEach(item =>
            {
                var row = dataTable.NewRow();
                item.Keys.ToList().ForEach(key =>
                    row[key] = item[key]);
                dataTable.Rows.Add(row);
            });
            return dataTable.Rows.Cast<DataRow>().ToArray().RegressToRedundantColumnsForBackwardCodeCompatibility();
        }
    }
}
