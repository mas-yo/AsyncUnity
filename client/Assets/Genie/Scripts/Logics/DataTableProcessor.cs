using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Genie.MasterData;

namespace Genie.Logics
{
    public static class DataTableProcessor
    {
        public static IEnumerable<(string tableName, IReadOnlyDictionary<string, string> dict)> ConvertRowsToDictionary(
            IEnumerable<string[]> rows)
        {
            using var rowEnumerator = rows.GetEnumerator();

            while (SearchTable(rowEnumerator, out var tableName))
            {
                if (rowEnumerator.MoveNext() == false) continue;
                var columnNames = rowEnumerator.Current.ToArray();

                foreach (var dict in EnumerateDictionary(rowEnumerator, columnNames).Select(x => (tableName, x)))
                {
                    yield return dict;
                }
            }

        }
        private static IEnumerable<IReadOnlyDictionary<string, string>> EnumerateDictionary(IEnumerator<string[]> rows, string[] columnNames)
        {
            while (rows.MoveNext())
            {
                var values = rows.Current;
                if (values.Length < 1) break;
                if (string.IsNullOrEmpty(values[0])) break;
                
                var dict = new Dictionary<string, string>();
                foreach (var kv in columnNames.Zip(values, (k, v) => new { k, v }))
                {
                    dict.TryAdd(kv.k, kv.v);
                }
                yield return dict;
            }
        }
        
        private static bool SearchTable(IEnumerator<string[]> rows, out string name)
        {
            while (rows.MoveNext())
            {
                var rowData = rows.Current;
                if (string.IsNullOrEmpty(rowData[0])) continue;
                if (string.Equals("MASTERDATA", rowData[0]))
                {
                    name = rowData[1];
                    return true;
                }
            }

            name = null;
            return false;
        }
        
        

    }
}