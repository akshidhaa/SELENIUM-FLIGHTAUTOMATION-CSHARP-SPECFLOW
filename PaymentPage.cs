using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace FlightBDD.Pages
{
    public class PaymentPage : BasePage
    {
        public PaymentPage(IWebDriver driver) : base(driver) { }

        public bool IsOnPaymentPage()
        {
            WaitForPageLoad();
            Thread.Sleep(1500);

            return IsDisplayed(By.XPath(
                "//*[contains(text(), 'Payment') or contains(text(), 'payment') or contains(text(), 'Credit Card')]"))
                || Driver.Url.Contains("payment")
                || Driver.Url.Contains("checkout")
                || IsDisplayed(By.XPath("//input[contains(@name, 'card') or contains(@placeholder, 'Card')]"));
        }



        public void FillCardPayment(string holderName, string cardNumber, string expMonth, string expYear, string cvv)
        {
            WaitForPageLoad();
            Thread.Sleep(1000);

            
            TryFill(By.XPath("//input[contains(@name, 'holder') or contains(@placeholder, 'Name on Card') or contains(@name, 'name')]"), holderName);

            
            TryFill(By.XPath("//input[contains(@name, 'card_number') or contains(@placeholder, 'Card Number') or contains(@name, 'cardnumber')]"), cardNumber);

            
            TrySelect(By.XPath("//select[contains(@name, 'month') or contains(@name, 'expiry_month')]"), expMonth);

            
            TrySelect(By.XPath("//select[contains(@name, 'year') or contains(@name, 'expiry_year')]"), expYear);

            
            TryFill(By.XPath("//input[contains(@name, 'cvv') or contains(@name, 'cvc') or contains(@placeholder, 'CVV')]"), cvv);

            Console.WriteLine($"Card filled: {holderName} | {cardNumber[..4]}xxxx | CVV: {cvv}");
        }

      

        public void SelectCashOnDelivery()
        {
            try
            {
                var cod = Driver.FindElement(By.XPath(
                    "//input[@value='cod' or @value='cash']/../label | //label[contains(text(), 'Cash')]"));
                cod.Click();
                Console.WriteLine("Payment method: Cash on Delivery");
            }
            catch
            {
                Console.WriteLine("COD option not available.");
            }
        }

        public void SelectCardPayment()
        {
            try
            {
                var cardOpt = Driver.FindElement(By.XPath(
                    "//input[@value='stripe' or @value='card' or @value='credit_card']/../label | //label[contains(text(), 'Credit Card')]"));
                cardOpt.Click();
                Thread.Sleep(1000);
                Console.WriteLine("Payment method: Credit Card");
            }
            catch
            {
                Console.WriteLine("Card option not available or already selected.");
            }
        }

       

        public ConfirmationPage ConfirmPayment()
        {
            var confirmSelectors = new By[]
            {
                By.XPath("//button[contains(text(), 'Confirm') or contains(text(), 'Pay Now')]"),
                By.XPath("//button[contains(text(), 'Complete') or contains(text(), 'Place Order')]"),
                By.XPath("//input[@type='submit']"),
                By.CssSelector("button.btn-success, button.btn-confirm, button.pay-btn")
            };

            foreach (var sel in confirmSelectors)
            {
                try
                {
                    var btn = Driver.FindElement(sel);
                    if (btn.Displayed && btn.Enabled)
                    {
                        ScrollTo(sel);
                        btn.Click();
                        Console.WriteLine("Payment confirmed");
                        Thread.Sleep(5000);
                        return new ConfirmationPage(Driver);
                    }
                }
                catch { }
            }

            throw new Exception("Could not find Confirm/Pay Now button on payment page.");
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

        private void TrySelect(By by, string value)
        {
            try
            {
                var el = Driver.FindElement(by);
                var select = new OpenQA.Selenium.Support.UI.SelectElement(el);
                try { select.SelectByValue(value); }
                catch { select.SelectByText(value); }
            }
            catch { }
        }
    }
}
