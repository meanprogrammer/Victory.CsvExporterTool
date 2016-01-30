using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Tools.CsvDataExporter
{
    public class ExportToCsvTask
    {
        private Database db;
        //static private string DBUser = "kidschurch";
        //static private string DBPass = "1nt3gr1ty@ENLI";


        public ExportToCsvTask()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        public void Execute()
        {
            IDataReader reader = db.ExecuteReader(CommandType.Text, "SELECT name FROM sys.Tables");
            using (reader)
            {
                while (reader.Read())
                {
                    string tableName = reader.GetString(0);
                    Console.WriteLine("Exporting {0}...", tableName);
                    ExportTableToCSV(tableName);
                    Console.WriteLine("Done Exporting {0}...", tableName);
                }
            }
        }

        public void ExportTableToCSV(string tableName)
        {
            string fileName = string.Format("{0}.csv", tableName);
            DataSet ds = db.ExecuteDataSet(System.Data.CommandType.Text, string.Format("SELECT * FROM {0}", tableName));
            string csvData = ConvertToCSV(ds);
            File.WriteAllText(fileName, csvData);
            ds.Dispose();
        }

        private string ConvertToCSV(DataSet objDataSet)
        {
            StringBuilder content = new StringBuilder();

            if (objDataSet.Tables.Count >= 1)
            {
                DataTable table = objDataSet.Tables[0];

                if (table.Rows.Count > 0)
                {
                    DataRow dr1 = (DataRow)table.Rows[0];
                    int intColumnCount = dr1.Table.Columns.Count;
                    int index = 1;

                    //add column names
                    foreach (DataColumn item in dr1.Table.Columns)
                    {
                        content.Append(String.Format("\"{0}\"", item.ColumnName));
                        if (index < intColumnCount)
                            content.Append(",");
                        else
                            content.Append("\r\n");
                        index++;
                    }

                    //add column data
                    foreach (DataRow currentRow in table.Rows)
                    {
                        string strRow = string.Empty;
                        for (int y = 0; y <= intColumnCount - 1; y++)
                        {
                            strRow += "\"" + currentRow[y].ToString() + "\"";

                            if (y < intColumnCount - 1 && y >= 0)
                                strRow += ",";
                        }
                        content.Append(strRow + "\r\n");
                    }
                }
            }

            return content.ToString();
        }
    }
}
