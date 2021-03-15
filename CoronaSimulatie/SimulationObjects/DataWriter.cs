using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace CoronaSimulatie.SimulationObjects
{
    public class DataWriter
    {
        Application file;
        _Workbook workbook;
        _Worksheet worksheet;
        int column;

        public DataWriter()
        {
            file = new Application();
            file.Visible = true;
            file.UserControl = false;

            workbook = file.Workbooks.Add("");
            worksheet = workbook.ActiveSheet;

            worksheet.Cells[1, 1] = "healthy";
            worksheet.Cells[1, 2] = "infected";
            worksheet.Cells[1, 3] = "recovered";

            column = 2;
        }

        public void Write(int healty, int infected, int recovered)
        {
            worksheet.Cells[column, 1] = healty;
            worksheet.Cells[column, 2] = infected;
            worksheet.Cells[column, 3] = recovered;
            column++;
        }

        ~DataWriter()
        {
            workbook.SaveAs("D:\\Users\\Lourens\\OneDrive\\Desktop\\test.xlsx", XlFileFormat.xlWorkbookDefault);
            workbook.Close();
            file.Quit();
        }
    }
}
