using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace FlightBDD.Pages
{
    public class BasePage
    {
        protected IWebDriver Driver;
        protected WebDriverWait Wait;
        protected WebDriverWait LongWait;

        public BasePage(IWebDriver driver)
        {
            Driver = driver;

            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            LongWait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }

        protected IWebElement WaitVisible(By by)
            => Wait.Until(
                ExpectedConditions.ElementIsVisible(by));

        protected IWebElement WaitClickable(By by)
            => Wait.Until(
                ExpectedConditions.ElementToBeClickable(by));

        protected void Click(By by)
            => WaitClickable(by).Click();

        protected void Type(By by, string text)
        {
            var el = WaitClickable(by);

            el.Clear();

            el.SendKeys(text);
        }

        protected string GetText(By by)
            => WaitVisible(by).Text.Trim();

        protected bool IsPresent(By by)
        {
            try
            {
                Driver.FindElement(by);

                return true;
            }
            catch
            {
                return false;
            }
        }

        protected bool IsDisplayed(By by)
        {
            try
            {
                return Driver.FindElement(by).Displayed;
            }
            catch
            {
                return false;
            }
        }

        protected void ScrollTo(By by)
        {
            var el = Driver.FindElement(by);

            ((IJavaScriptExecutor)Driver)
                .ExecuteScript(
                "arguments[0].scrollIntoView(true);",
                el);

            Thread.Sleep(300);
        }

        protected void JSClick(By by)
        {
            var el = Driver.FindElement(by);

            ((IJavaScriptExecutor)Driver)
                .ExecuteScript(
                "arguments[0].click();",
                el);
        }

        protected void WaitPageReady()
        {
            Wait.Until(x =>
                ((IJavaScriptExecutor)x)
                .ExecuteScript(
                "return document.readyState")
                .ToString() == "complete");
        }

        // Handles alert if present

        protected void DismissAlertIfPresent()
        {
            try
            {
                var alert = Driver.SwitchTo().Alert();

                alert.Dismiss();
            }
            catch { } 
        }
    }
}
