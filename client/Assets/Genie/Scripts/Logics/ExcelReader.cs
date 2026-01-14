using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelDataReader;

namespace Genie.Logics
{
    public static class ExcelReader
    {
        public static IEnumerable<string[]> EnumerateRows(IExcelDataReader reader)
        {
            try
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
            finally
            {
                reader.Dispose();
            }
        }

        public static IEnumerable<IExcelDataReader> EnumerateExcelReaders(string directoryPath, string temporaryPath)
        {
            return EnumerateExcelFiles(directoryPath).Select(path =>
            {
                var tempPath = Path.Combine(temporaryPath, Path.GetFileName(path));
                File.Copy(path, tempPath, true );
                var stream = File.Open(tempPath, FileMode.Open, FileAccess.Read);
                var reader = ExcelReaderFactory.CreateReader(stream);
                return reader;
            });
        }
        
        public static IEnumerable<string> EnumerateExcelFiles(string directoryPath)
        {
            return System.IO.Directory.EnumerateFiles(directoryPath, "*.xlsx", System.IO.SearchOption.TopDirectoryOnly)
                .Where(x => !Path.GetFileName(x).StartsWith("~"));
        }
    }
}