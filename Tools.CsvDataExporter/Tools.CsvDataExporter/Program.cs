using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.CsvDataExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            ExportToCsvTask task = new ExportToCsvTask();
            task.Execute();
        }
    }
}
