
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Infrastructure.Services
{
    public class PdfService:IPdfService
    {
        //Here download chromium if needed
        //    then launches a headless browwser
        //    then opens a page and set the html content
        //    convert the page to pdf with bg and margins
        //    return Pdf bytes
     public async Task<byte[]> GenerateProfilePdfAsync(string htmlContent)
        {
            //Download Chromium if not already done
            await new BrowserFetcher().DownloadAsync();

            //Launch a headless browser means run without showing GUI
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync(htmlContent);

            var pdfData = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                MarginOptions = new MarginOptions
                {
                    Top = "20px",
                    Bottom = "20px",
                    Left = "20px",
                    Right = "20px"
                }
            });
            return pdfData;
        }

    }
}
