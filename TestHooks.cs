
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;
using System.IO;

namespace FlightBDD.Hooks
{
    public class TestHooks
    {
      

        public static readonly string DownloadPath =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.UserProfile),
                "Downloads",
                "FlightTickets");

        [BeforeScenario]
        public void Setup()
        {
            Directory.CreateDirectory(DownloadPath);

            var options = new ChromeOptions();

            options.AddArgument("--start-maximized");

            Driver = new ChromeDriver(options);

            Driver.Navigate()
                  .GoToUrl("https://phptravels.net/flights");

            Thread.Sleep(4000);

            ClosePopup();
        }

        private void ClosePopup()
        {
            try
            {
                var btn = Driver.FindElement(
                    By.XPath(
                    "//button[contains(text(),'Accept') " +
                    "or contains(text(),'OK') " +
                    "or contains(text(),'Close')]"));

                btn.Click();

                Thread.Sleep(1000);

                Console.WriteLine("Popup closed");
            }
            catch
            {
                Console.WriteLine("No popup present");
            }
        }

        [AfterScenario]
        public void TearDown()
        {
            try
            {
                Driver.Quit();
            }
            catch
            {
            }
        }
    }
}
