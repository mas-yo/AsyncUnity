using System.Collections.Generic;
using System.Linq;
using ExcelDataReader;

namespace Genie.Logics
{
    public static class ExcelReader
    {
        public static IEnumerable<string[]> EnumerateRows(IExcelDataReader reader)
        {
            do
            {
                while (reader.Read())
                {
                    var row = new string[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[i] = reader.GetValue(i)?.ToString() ?? string.Empty;
                    }
                    yield return row;
                }
            } while (reader.NextResult());
        }

        public static IEnumerable<IExcelDataReader> EnumerateExcelReaders(string directoryPath)
        {
            return EnumerateExcelFiles(directoryPath).Select(x =>
            {
                var stream = System.IO.File.Open(x, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                var reader = ExcelReaderFactory.CreateReader(stream);
                return reader;
            });
        }
        
        public static IEnumerable<string> EnumerateExcelFiles(string directoryPath)
        {
            return System.IO.Directory.EnumerateFiles(directoryPath, "*.xlsx", System.IO.SearchOption.AllDirectories);
        }
    }
}