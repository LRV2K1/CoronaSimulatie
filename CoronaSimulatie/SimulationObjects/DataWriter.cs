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

        float vs, ve, vj, vr;
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

            worksheet.Cells[1, 17] = "calculated-beta";
            worksheet.Cells[1, 18] = "calculated-alpha";
            worksheet.Cells[1, 19] = "calculated-gamma";

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

            vs = 0;
            ve = 0;
            vj = 0;
            vr = 0;
        }

        public void Write()
        {
            worksheet.Cells[column, 1] = SaveData.Susceptible;
            worksheet.Cells[column, 2] = SaveData.Exposed;
            worksheet.Cells[column, 3] = SaveData.Infectious;
            worksheet.Cells[column, 4] = SaveData.Recovered;

            float s = SaveData.Susceptible;
            float e = SaveData.Exposed;
            float j = SaveData.Infectious;
            float r = SaveData.Recovered;
            float t = Globals.totalpopulation;

            float ds = (s - vs) / t;
            float noemer = (s / t) * (j / t);
            float beta = 0;
            if (noemer != 0)
                beta = (-ds) / noemer;

            float de = (e - ve) / t;
            float alpha = 0;
            if (e != 0)
                alpha = ((-de) + beta * noemer) / (e / t);

            float di = (j - vj) / t;
            float gamma = 0;
            if (j != 0)
                gamma = ((-di) + alpha * (e / t)) / (j / t);

            worksheet.Cells[column, 17] = beta;
            worksheet.Cells[column, 18] = alpha;
            worksheet.Cells[column, 19] = gamma;

            vs = SaveData.Susceptible;
            ve = SaveData.Exposed;
            vj = SaveData.Infectious;
            vr = SaveData.Recovered;

            column++;
        }

        public void Log(float f)
        {
            worksheet.Cells[column, 1] = f;
            column++;
        }

        public void End()
        {
            worksheet.Cells[2, 8] = "=AVERAGE(Q3: Q" + column + ")";
            worksheet.Cells[2, 9] = "=AVERAGE(R3: R" + column + ")";
            worksheet.Cells[2, 10] = "=AVERAGE(S3: S" + column + ")";
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
