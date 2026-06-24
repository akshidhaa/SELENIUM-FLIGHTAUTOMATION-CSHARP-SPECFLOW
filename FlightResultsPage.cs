using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FlightBDD.Pages
{
    public class FlightResultsPage : BasePage
    {
        public FlightResultsPage(IWebDriver driver)
            : base(driver)
        {
        }

       

        public bool HasResults()
        {
            WaitForPageLoad();
            Thread.Sleep(3000);

            var resultSelectors = new[]
            {
                By.XPath("//div[contains(@class,'flight-item')]"),
                By.XPath("//div[contains(@class,'result-item')]"),
                By.XPath("//div[contains(@class,'flight-list')]//div[contains(@class,'item')]"),
                By.XPath("//button[contains(text(),'Book') or contains(text(),'Select')]")
            };

            foreach (var sel in resultSelectors)
            {
                try
                {
                    var items = Driver.FindElements(sel);

                    if (items.Count > 0 && items[0].Displayed)
                    {
                        Console.WriteLine($"Found {items.Count} flight result(s)");
                        return true;
                    }
                }
                catch { }
            }

            Console.WriteLine("No flight results found with standard selectors.");
            return false;
        }

        public bool IsNoResultsMessageShown()
        {
            var noResultSelectors = new[]
            {
                By.XPath("//*[contains(text(),'No flights') or contains(text(),'no flights')]"),
                By.XPath("//*[contains(text(),'No Result') or contains(text(),'not found')]"),
                By.CssSelector(".no-results, .empty-results, .no-flights")
            };

            foreach (var sel in noResultSelectors)
            {
                if (IsDisplayed(sel))
                    return true;
            }

            return false;
        }

       

        public SeatBookingPage SelectFirstFlight()
        {
            Thread.Sleep(2000);
            WaitForPageLoad();

            var bookSelectors = new[]
            {
                By.XPath("(//button[contains(text(),'Book')])[1]"),
                By.XPath("(//a[contains(text(),'Book')])[1]"),
                By.XPath("(//button[contains(text(),'Select')])[1]"),
                By.XPath("(//a[contains(@href,'book') or contains(@href,'flight')])[1]"),
                By.XPath("(//*[contains(@class,'flight')]//button)[1]")
            };

            foreach (var sel in bookSelectors)
            {
                try
                {
                    var btn = Driver.FindElement(sel);

                    if (btn.Displayed)
                    {
                        ScrollInto(btn);
                        Thread.Sleep(500);

                        btn.Click();

                        Console.WriteLine("Flight selected");

                        Thread.Sleep(3000);

                        return new SeatBookingPage(Driver);
                    }
                }
                catch { }
            }

            throw new Exception(
                "Could not find a Book/Select button on results page."
            );
        }
    }
}
