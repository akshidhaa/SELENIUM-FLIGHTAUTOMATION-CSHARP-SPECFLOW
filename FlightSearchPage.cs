using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace FlightBDD.Pages
{
    
   
    public class FlightSearchPage : BasePage
    {
        public FlightSearchPage(IWebDriver driver) : base(driver) { }

       

        public void SelectFromCity(string city)
        {
         
            var fromContainer = Wait.Until(d => 
                d.FindElement(By.XPath("(//span[contains(@class,'select2-selection')])[1]")));
            fromContainer.Click();
            Thread.Sleep(1000);

           
            var searchBox = Wait.Until(d => 
                d.FindElement(By.XPath("//input[@class='select2-search__field']")));
            searchBox.Clear();
            searchBox.SendKeys(city);
            Thread.Sleep(2000);

            var option = Wait.Until(d => 
                d.FindElement(By.XPath("//li[contains(@class,'select2-results__option')][1]"))); 
            option.Click();
            Thread.Sleep(500);
            Console.WriteLine($"FROM: {city}");
        }

        

        public void SelectToCity(string city)
        {
           
            var toContainer = Wait.Until(d => 
                d.FindElement(By.XPath("(//span[contains(@class,'select2-selection')])[2]")));
            toContainer.Click();
            Thread.Sleep(1000);

            var searchBox = Wait.Until(d => 
                d.FindElement(By.XPath("//input[@class='select2-search__field']")));
            searchBox.Clear();
            searchBox.SendKeys(city);
            Thread.Sleep(2000);

            var option = Wait.Until(d => 
                d.FindElement(By.XPath("//li[contains(@class,'select2-results__option') and not(contains(@class,'loading'))]")));
            option.Click();
            Thread.Sleep(500);
            Console.WriteLine($"TO: {city}");
        }

      

        public void SelectTravelDate(int daysFromToday = 7)
        {
            var targetDate = DateTime.Today.AddDays(daysFromToday);

           
            var dateInput = Wait.Until(d => 
                d.FindElement(By.XPath("//input[contains(@placeholder,'Departure') or contains(@id,'departure_date') or contains(@name,'departure_date')]")));
            dateInput.Click();
            Thread.Sleep(1500);

           
            NavigateToMonth(targetDate);

           
            ClickDay(targetDate.Day);
            Thread.Sleep(500);
            Console.WriteLine($"DATE: {targetDate:dd MMM yyyy}");
        }

        private void NavigateToMonth(DateTime target)
        {
            for (int i = 0; i < 12; i++)
            {
                string header = string.Empty;
                try
                {
                    header = Driver.FindElement(By.XPath("//th[contains(@class,'datepicker-switch')]")).Text;
                }
                catch { return; }

                if (header.Contains(target.ToString("MMMM")) && header.Contains(target.Year.ToString()))
                {
                    return;
                }

              
                try
                {
                    Driver.FindElement(By.XPath("//th[contains(@class,'next')]")).Click();
                    Thread.Sleep(400);
                }
                catch { return; }
            }
        }

        private void ClickDay(int day)
        {
            var days = Driver.FindElements(By.XPath("//td[contains(@class,'day') and not(contains(@class,'old')) and not(contains(@class,'disabled'))]"));
            foreach (var d in days)
            {
                if (d.Text.Trim() == day.ToString())
                {
                    d.Click();
                    return;
                }
            }

          
            if (days.Count > 0) days[0].Click();
        }

        

        public void SelectReturnDate(int daysFromToday = 14)
        {
            var targetDate = DateTime.Today.AddDays(daysFromToday);
            try
            {
                var returnInput = Driver.FindElement(By.XPath("//input[contains(@placeholder,'Return') or contains(@id,'return_date')]"));
                returnInput.Click();
                Thread.Sleep(1500);
                NavigateToMonth(targetDate);
                ClickDay(targetDate.Day);
                Thread.Sleep(500);
            }
            catch
            {
                Console.WriteLine("No return date field found (one-way flight).");
            }
        }

        

        public void SetPassengers(int adults = 1)
        {
            
            Console.WriteLine($"PASSENGERS: {adults} adult(s)");
        }

   

        public void ClickSearch()
        {
           
            var searchSelectors = new By[]
            {
                By.XPath("//button[contains(@class,'main_search')]"),
                By.XPath("//button[contains(text(),'Search')]"),
                By.XPath("//input[@type='submit']"),
                By.CssSelector("button.btn-search, button.search-btn, .flights-search button[type='submit']")
            };

            foreach (var selector in searchSelectors)
            {
                try
                {
                    var btn = Driver.FindElement(selector);
                    if (btn.Displayed && btn.Enabled)
                    {
                        ScrollTo(selector);
                        btn.Click();
                        Console.WriteLine("SEARCH clicked");
                        Thread.Sleep(5000); 
                        return;
                    }
                }
                catch { }
            }

            throw new Exception("Search button not found. Check the selector.");
        }
    }
}
