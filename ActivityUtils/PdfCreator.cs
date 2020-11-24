using HouseholdUserApplication.Models;

using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TheArtOfDev.HtmlRenderer.PdfSharp;


namespace HouseholdUserApplication.ActivityUtils
{
    public class PdfCreator
    {
        public static byte[] createPDF(Report report)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(@"wwwroot/Template.xls");
            Worksheet sheet1 = workbook.Worksheets[0];
            sheet1.Range["A4"].Value += $"{report.User.FirstName} {report.User.LastName}";
            string openDate = report.Balance.OpenDate.ToString("yyyթ MMMM d", CultureInfo.CreateSpecificCulture("hy"));
            sheet1.Range["D2:E2"].Value = @$"Հաշիվ-ֆակտուրա
                                            No {report.Billing.Id}
                                               {openDate}

            ";
            sheet1.Range["D3:E3"].Value = @$"Invoice N {report.Billing.Id}
                                                       {report.Balance.OpenDate}
            ";
            sheet1.Range["D13:D14"].Value = $"{report.Balance.Services}";
            sheet1.Range["A19"].Value = sheet1.Range["A19"].DisplayedText.Replace("{0}", report.Balance.TotalBalance.ToString());
            sheet1.Range["A7"].Value += $"\t{report.User.FirstName} {report.User.LastName}";
            sheet1.Range["E23"].Value = $"{report.Balance.Utilities}";
            int closeYear = report.Balance.CloseDate.Year;
            string closeDate = report.Balance.CloseDate.ToString("MMMM", CultureInfo.CreateSpecificCulture("en"));
            int closeDay = report.Balance.CloseDate.Day;
            string closeDateArm = report.Balance.CloseDate.ToString("yyyթ. MMMM d-ը", CultureInfo.CreateSpecificCulture("hy"));
            string openDateMonth = report.Balance.OpenDate.ToString("yyy թ. MMMM", CultureInfo.CreateSpecificCulture("hy"));

            sheet1.Range["B26:C26"].Value = sheet1.Range["B26:C26"].DisplayedText.Replace("25 March 2020", $" {closeDay} {closeDate} {closeYear}");
            sheet1.Range["E22"].Value = report.Billing.Payed.ToString();
            sheet1.Range["E23"].Value = report.Billing.TotalBilling.ToString();

            sheet1.Range["E24"].Value = report.Billing.Remain.ToString();
            sheet1.Range["A26"].Value = sheet1.Range["A26"].DisplayedText.Replace("2020թ. Մարտի 25-ը", $"{closeDateArm}");
            sheet1.Range["D31"].Value = $"{report.User.FirstName} {report.User.LastName}";
           
            Worksheet sheet2 = workbook.Worksheets[1];

            sheet2.Range["C19"].Value = $"{report.Balance.Utilities}";
            sheet2.Range["A2"].Value = sheet2.Range["A2"].DisplayedText.Replace("{0} թ. {1}", openDateMonth);
            sheet2.Range["A5"].Value = sheet2.Range["A5"].DisplayedText.Replace("{0} թ. {1}", openDateMonth);
            sheet2.Range["A8"].Value = sheet2.Range["A8"].DisplayedText.Replace("{0} թ. {1}", openDateMonth);
            sheet2.Range["A9"].Value = sheet2.Range["A9"].DisplayedText.Replace("{0} թ. {1}", openDateMonth);
            sheet2.Range["A14"].Value = sheet2.Range["A14"].DisplayedText.Replace("{0} թ. {1}", openDateMonth);

            MemoryStream stream = new MemoryStream();
            workbook.SaveToStream(stream, Spire.Xls.FileFormat.PDF);
            byte[] bytes = stream.ToArray();
            return bytes;
            
        }
     
    }
}
