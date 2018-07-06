using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SignalCollectorPro.DataObjects;

namespace SignalCollectorPro
{
    class DataBaseManager
    {



        static OleDbDataAdapter dataAdapter;
        static DataTable LocalDataTable = new DataTable();
        static OleDbConnection dbconnect = new OleDbConnection();
        static int rowPosition = 0;
        static int rowNumber = 0;


        public static void AddToDatabase(SaveMeasure data)
        {
            double measure = data._measurement;
            double temperature = data._temperature;
            double realmeasure = data._realmeasurement;
            double realtemp = data._realtemperature;
            DateTime t = data._time;
            dbconnect.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Measure.mdb";
            dbconnect.Open();
            string sql = "insert into [tab]([Measure],[Temperature],[RealMeasure],[RealTemp],[Date]) values(" + measure + "," + temperature + "," + realmeasure + "," + realtemp + "," + t.ToOADate() + ");";
            dataAdapter = new OleDbDataAdapter(sql, dbconnect);
            dataAdapter.Fill(LocalDataTable);

            if (LocalDataTable.Rows.Count != 0)
            {
                rowPosition = LocalDataTable.Rows.Count;
            }

            dbconnect.Close();
        }

        public static void CleanDatabase()
        {
            dbconnect.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Measure.mdb";
            dbconnect.Open();
            string sql = "delete * from [tab]";
            dataAdapter = new OleDbDataAdapter(sql, dbconnect);
            dataAdapter.Fill(LocalDataTable);

            if (LocalDataTable.Rows.Count != 0)
            {
                rowPosition = LocalDataTable.Rows.Count;
            }

            dbconnect.Close();
        }

        public static DataTable LoadDateFromFile(string fileName, DateTime from, DateTime to)
        {
            DataTable buf = new DataTable("Measure");

            // For convenience, the DataSet is identified by the name of the loaded file (without extension).

            // Compute the ConnectionString (using the OLEDB v12.0 driver compatible with ACCDB and MDB files)
            fileName = Path.GetFullPath(fileName);
            string connString = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Measure.mdb", fileName);

            // Opening the Access connection
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                conn.Open();

                // Getting all user tables present in the Access file (Msys* tables are system thus useless for us)
                DataTable dt = conn.GetSchema("Tables");
                List<string> tablesName = dt.AsEnumerable().Select(dr => dr.Field<string>("TABLE_NAME")).Where(dr => !dr.StartsWith("MSys")).ToList();

                // Getting the data for every user tables

                using (OleDbCommand cmd = new OleDbCommand(string.Format("SELECT * FROM [Tab] where Date between " + from.ToOADate() + " and " + to.ToOADate() + ";"), conn))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        // Saving all tables in our result DataSet.
                        buf = new DataTable("Measure");
                        adapter.Fill(buf);
                    } // adapter
                } // cmd
            } // tableName


            // Return the filled DataSet
            return buf;
        }

        public static DataTable LoadFromFile(string fileName)
        {
            DataTable buf = new DataTable("Measure");

            // For convenience, the DataSet is identified by the name of the loaded file (without extension).

            // Compute the ConnectionString (using the OLEDB v12.0 driver compatible with ACCDB and MDB files)
            fileName = Path.GetFullPath(fileName);
            string connString = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Measure.mdb", fileName);

            // Opening the Access connection
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                conn.Open();

                // Getting all user tables present in the Access file (Msys* tables are system thus useless for us)
                DataTable dt = conn.GetSchema("Tables");
                List<string> tablesName = dt.AsEnumerable().Select(dr => dr.Field<string>("TABLE_NAME")).Where(dr => !dr.StartsWith("MSys")).ToList();

                // Getting the data for every user tables

                using (OleDbCommand cmd = new OleDbCommand(string.Format("SELECT * FROM [Tab]"), conn))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmd))
                    {
                        // Saving all tables in our result DataSet.
                        buf = new DataTable("Measure");
                        adapter.Fill(buf);
                    } // adapter
                } // cmd
            } // tableName


            // Return the filled DataSet
            return buf;
        }

        public static void GetXLS(List<List<string>> list)
        {
            string tmpstr = "XLS";
            string fileName = "数据.xls";
            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
            {
                IWorkbook workBook;

                if (tmpstr == "XLS")
                {
                    workBook = new HSSFWorkbook();
                }
                else if (tmpstr == "XLSX")
                {
                    workBook = new XSSFWorkbook();
                }
                else
                    throw new Exception("不支持的文件格式");
                var sheet = workBook.CreateSheet("测量");
                //表头
                var row = sheet.CreateRow(0);
                string[] colName = new string[] { "ID", "测数", "温度", "实际测数", "环境温度", "日期" };
                for (int i = 0; i < colName.Length; i++)
                {
                    ICell cell = row.CreateCell(i);
                    cell.SetCellType(CellType.String);
                    cell.SetCellValue(colName[i]);
                }
                row = sheet.CreateRow(1);

                for (int i = 0; i < list.Count; i++)
                {
                    row = sheet.CreateRow(i + 1);
                    var cell0 = row.CreateCell(0);
                    var cell1 = row.CreateCell(1);
                    var cell2 = row.CreateCell(2);
                    var cell3 = row.CreateCell(3);
                    var cell4 = row.CreateCell(4);
                    var cell5 = row.CreateCell(5);
                    cell0.SetCellValue(list[i][0]);
                    cell1.SetCellValue(list[i][1]);
                    cell2.SetCellValue(list[i][2]);
                    cell3.SetCellValue(list[i][3]);
                    cell4.SetCellValue(list[i][4]);
                    cell5.SetCellValue(list[i][5]);

                }

                workBook.Write(stream);
                workBook.Close();
            }
        }
    }
}
