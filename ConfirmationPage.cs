using OpenQA.Selenium;

namespace FlightBDD.Pages
{
    public class ConfirmationPage : BasePage
    {
        public ConfirmationPage(IWebDriver driver) : base(driver) { }

        public bool IsBookingConfirmed()
        {
            WaitForPageLoad();
            Thread.Sleep(2000);

            var successSelectors = new[]
            {
                By.XPath("//*[contains(text(),'Booking Confirmed') or contains(text(),'Booking confirmed')]"),
                By.XPath("//*[contains(text(),'Thank you') or contains(text(),'Successfully')]"),
                By.XPath("//*[contains(text(),'confirmation') or contains(text(),'Confirmation')]"),
                By.CssSelector(".alert-success, .booking-success, .success-message"),
                By.XPath("//*[contains(@class,'success')]")
            };

            foreach (var sel in successSelectors)
            {
                if (IsDisplayed(sel))
                {
                    Console.WriteLine($"Booking confirmed: {Driver.FindElement(sel).Text.Trim()}");
                    return true;
                }
            }

           
            if (Driver.Url.Contains("invoice") ||
                Driver.Url.Contains("confirm") ||
                Driver.Url.Contains("success") ||
                Driver.Url.Contains("thankyou"))
            {
                Console.WriteLine($"Booking confirmed via URL: {Driver.Url}");
                return true;
            }

            return false;
        }

        public string GetBookingReference()
        {
            var refSelectors = new[]
            {
                By.XPath("//*[contains(@class,'booking-ref') or contains(@class,'invoice-no')]"),
                By.XPath("//*[contains(text(),'Booking ID') or contains(text(),'Invoice')]/following-sibling::*[1]"),
                By.XPath("//*[contains(text(),'#') and string-length(text()) > 3]")
            };

            foreach (var sel in refSelectors)
            {
                try
                {
                    var el = Driver.FindElement(sel);

                    if (el.Displayed && !string.IsNullOrWhiteSpace(el.Text))
                        return el.Text.Trim();
                }
                catch { }
            }

            return "REF-" + DateTime.Now.Ticks.ToString()[^8..];
        }

      

        public string PrintTicketAsPdf()
        {
            var downloadPath = Hooks.TestHooks.DownloadPath;

           
            var printSelectors = new[]
            {
                By.XPath("//a[contains(text(),'Print') or contains(text(),'Download')]"),
                By.XPath("//button[contains(text(),'Print') or contains(text(),'Download')]"),
                By.XPath("//a[contains(@href,'invoice') or contains(@href,'print') or contains(@href,'pdf')]"),
                By.CssSelector("a.btn-print, a.print-ticket, .invoice-btn")
            };

            foreach (var sel in printSelectors)
            {
                try
                {
                    var btn = Driver.FindElement(sel);

                    if (btn.Displayed)
                    {
                        var href = btn.GetAttribute("href") ?? "";

                        if (href.EndsWith(".pdf") || href.Contains("pdf"))
                        {
                            

                            Driver.Navigate().GoToUrl(href);
                            Thread.Sleep(3000);

                            Console.WriteLine($"PDF download triggered: {href}");
                        }
                        else
                        {
                           

                            DownloadViaPrintDialog(downloadPath, btn);
                        }

                        break;
                    }
                }
                catch { }
            }

          

            SavePageAsPdf(downloadPath);

            return downloadPath;
        }

        private void DownloadViaPrintDialog(string downloadPath, IWebElement btn)
        {
          

            ((IJavaScriptExecutor)Driver).ExecuteScript(
                "window.print = function() { console.log('print intercepted'); };"
            );

            btn.Click();

            Thread.Sleep(2000);

            SavePageAsPdf(downloadPath);
        }

        private void SavePageAsPdf(string downloadPath)
        {
            try
            {
                

                var chromeDriver =
                    (OpenQA.Selenium.Chrome.ChromeDriver)Driver;

                var resultObj =
                    chromeDriver.ExecuteCdpCommand(
                        "Page.printToPDF",
                        new Dictionary<string, object>
                        {
                            { "printBackground", true },
                            { "paperWidth", 8.27 },
                            { "paperHeight", 11.69 }
                        });

                var result = resultObj as Dictionary<string, object>;

                if (result != null && result.ContainsKey("data"))
                {
                    Directory.CreateDirectory(downloadPath);

                    var pdfData = result["data"].ToString();

                    var bytes = Convert.FromBase64String(pdfData);

                    var fileName =
                        $"FlightTicket_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                    var fullPath =
                        Path.Combine(downloadPath, fileName);

                    File.WriteAllBytes(fullPath, bytes);

                    Console.WriteLine($"Ticket PDF saved: {fullPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PDF via CDP failed: {ex.Message}");

               

                var dir = downloadPath;

                Directory.CreateDirectory(dir);

                var imgPath =
                    Path.Combine(
                        dir,
                        $"Ticket_{DateTime.Now:yyyyMMdd_HHmmss}.png"
                    );

                ((ITakesScreenshot)Driver)
                    .GetScreenshot()
                    .SaveAsFile(imgPath);

                Console.WriteLine($"Ticket screenshot saved: {imgPath}");
            }
        }
    }
}
