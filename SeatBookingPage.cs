using OpenQA.Selenium;
using System;
using System.Threading;

namespace FlightBDD.Pages
{
    public class SeatBookingPage : BasePage
    {
        public SeatBookingPage(IWebDriver driver) : base(driver) { }

        public bool HasSeatMap()
        {
            WaitForPageLoad();
            Thread.Sleep(2000);

            var seatSelectors = new[]
            {
                By.XPath("//*[contains(@class,'seat') and not(contains(@class,'disabled'))]"),
                By.XPath("//div[contains(@class,'seatmap')]"),
                By.XPath("//div[contains(@id,'seat')]"),
                By.XPath("//table[contains(@class,'seat')]")
            };

            foreach (var sel in seatSelectors)
            {
                if (IsDisplayed(sel))
                {
                    Console.WriteLine("Seat map visible");
                    return true;
                }
            }

            Console.WriteLine("No seat map - proceeding to passenger/booking details.");
            return false;
        }

        public bool IsNoSeatsAvailable()
        {
            return IsDisplayed(By.XPath("//*[contains(text(),'No seats') or contains(text(),'Sold out') or contains(text(),'fully booked')]"));
        }

        public string GetNoSeatsMessage()
        {
            try
            {
                return Driver.FindElement(By.XPath("//*[contains(text(),'No seats') or contains(text(),'Sold out')]")).Text;
            }
            catch
            {
                return "No seats available message";
            }
        }

        public void SelectFirstAvailableSeat()
        {
            var seatSelectors = new[]
            {
                By.XPath("(//*[contains(@class,'seat available') or contains(@class,'available seat')])[1]"),
                By.XPath("(//*[contains(@class,'seat') and not(contains(@class,'occupied')) and not(contains(@class,'disabled'))])[1]"),
                By.XPath("(//span[contains(@class,'seat')])[1]"),
                By.XPath("(//td[contains(@class,'seat')])[1]")
            };

            foreach (var sel in seatSelectors)
            {
                try
                {
                    var seat = Driver.FindElement(sel);
                    if (seat.Displayed)
                    {
                        JSClick(seat);
                        Console.WriteLine("Seat selected");
                        Thread.Sleep(1000);
                        return;
                    }
                }
                catch { }
            }

            Console.WriteLine("No seat map to interact with - skipping seat selection.");
        }

        public void FillPassengerDetails(string firstName = "Test", string lastName = "User", string email = "test@test.com", string phone = "9876543210")
        {
            WaitForPageLoad();
            Thread.Sleep(1500);

            TryFill(By.XPath("//input[contains(@name,'first_name') or contains(@placeholder,'First')]"), firstName);
            TryFill(By.XPath("//input[contains(@name,'last_name') or contains(@placeholder,'Last')]"), lastName);
            TryFill(By.XPath("//input[contains(@name,'email') or contains(@type,'email')]"), email);
            TryFill(By.XPath("//input[contains(@name,'phone') or contains(@name,'mobile') or contains(@placeholder,'Phone')]"), phone);

            Console.WriteLine($"Passenger: {firstName} {lastName} | {email} | {phone}");
        }

        private void TryFill(By by, string value)
        {
            try
            {
                var el = Driver.FindElement(by);
                if (el.Displayed && el.Enabled)
                {
                    el.Clear();
                    el.SendKeys(value);
                }
            }
            catch { }
        }

        public PaymentPage ProceedToPayment()
        {
            var continueSelectors = new[]
            {
                By.XPath("//button[contains(text(),'Continue') or contains(text(),'Proceed')]"),
                By.XPath("//button[contains(text(),'Book') or contains(text(),'Pay')]"),
                By.XPath("//input[@type='submit']"),
                By.CssSelector("button.btn-primary, button.btn-continue")
            };

            foreach (var sel in continueSelectors)
            {
                try
                {
                    var btn = Driver.FindElement(sel);
                    if (btn.Displayed && btn.Enabled)
                    {
                        ScrollTo(sel);
                        btn.Click();
                        Console.WriteLine("Proceeding to payment");
                        Thread.Sleep(3000);
                        return new PaymentPage(Driver);
                    }
                }
                catch { }
            }

            throw new Exception("Could not find Continue/Proceed button on booking page.");
        }
    }
}
