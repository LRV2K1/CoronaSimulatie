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

        int previous_susceptible;
        int nummer;

        DataWriter writer;

        public DataWriter(int nummer, DataWriter writer = null)
        {
            this.nummer = nummer;
            this.writer = writer;

            file = new Application();
            file.UserControl = false;

            workbook = file.Workbooks.Add("");
            worksheet = workbook.ActiveSheet;

            column = 2;

            if (writer != null)
                DataSetup();
            else
                RDataSetup();
        }

        private void RDataSetup()
        {
            file.Visible = true;
            worksheet.Cells[1, 1] = "R0";
        }

        private void DataSetup()
        {
            worksheet.Cells[1, 1] = "Susceptible";
            worksheet.Cells[1, 2] = "Exposed";
            worksheet.Cells[1, 3] = "Infectious";
            worksheet.Cells[1, 4] = "Recovered";

            worksheet.Cells[1, 6] = "calculated-beta";

            worksheet.Cells[1, 8] = "beta";
            worksheet.Cells[1, 9] = "alpha";
            worksheet.Cells[1, 10] = "gamma";
            worksheet.Cells[1, 11] = "R0";
            worksheet.Cells[2, 9] = 1f / Globals.infectiondays;
            worksheet.Cells[2, 10] = 1f / Globals.a_contingiousnessdays;

            worksheet.Cells[1, 13] = "Total Population";
            worksheet.Cells[1, 14] = "Start Infected Population";
            worksheet.Cells[1, 15] = "World Size";
            worksheet.Cells[2, 13] = Globals.totalpopulation;
            worksheet.Cells[2, 14] = Globals.illpopulation;
            worksheet.Cells[2, 15] = Globals.worldsize;

            previous_susceptible = 0;
        }

        public void Write()
        {
            worksheet.Cells[column, 1] = SaveData.Susceptible;
            worksheet.Cells[column, 2] = SaveData.Exposed;
            worksheet.Cells[column, 3] = SaveData.Infectious;
            worksheet.Cells[column, 4] = SaveData.Recovered;

            float ds = ((float)(SaveData.Susceptible - previous_susceptible))/((float)(Globals.totalpopulation));
            float noemer = ((float)((((float)SaveData.Susceptible) / ((float)Globals.totalpopulation)) * (((float)SaveData.Infectious) / ((float)Globals.totalpopulation))));
            if (noemer != 0)
            {
                float beta = -ds / ((float)((((float)SaveData.Susceptible) / ((float)Globals.totalpopulation)) * (((float)SaveData.Infectious) / ((float)Globals.totalpopulation))));
                worksheet.Cells[column, 6] = beta;
            }
            else
            {
                worksheet.Cells[column, 6] = 0;
            }
            previous_susceptible = SaveData.Susceptible;

            float alpha = 1 / Globals.infectiondays;
            //float gamma = 1 / Globals.a_contingiousnessdays;
            //float r0 = (beta * alpha) / (alpha * gamma);
            //worksheet.Cells[column, 7] = r0;

            column++;
        }

        public void Log(float f)
        {
            worksheet.Cells[column, 1] = f;
            column++;
        }

        public void End()
        {
            worksheet.Cells[2, 8] = "=AVERAGE(F3:F" + column + ")";
            worksheet.Cells[2, 11] = "=H2/J2";
            Range r = worksheet.Cells[2, 11];
            double s = r.Value2;
            Console.WriteLine(s);
            writer.Log((float)s);
        }

        ~DataWriter()
        {
            file.Visible = false;
            if (writer != null)
                workbook.SaveAs("\\data\\data" + nummer + ".xlsx", XlFileFormat.xlWorkbookDefault);
            else
                workbook.SaveAs("\\data\\Rdata.xlsx", XlFileFormat.xlWorkbookDefault);
            workbook.Close();
            file.Quit();
        }
    }
}
