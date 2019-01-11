using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace SeleniumBase
{
	public abstract class BaseDriver
	{
		public IWebDriver Driver;
		private WebDriverWait _wait;
		private readonly int _timeoutSeconds;
		public WebDriverWait Wait
		{
			get
			{
				if (_wait == null)
				{
					_wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(_timeoutSeconds));
				}

				return _wait;
			}
		}

		protected BaseDriver(int timeoutSeconds)
		{
			_timeoutSeconds = timeoutSeconds;
		}

		protected abstract IWebDriver StartDriver();

		public void NavigateToUrl(string url)
		{
			Driver.Navigate().GoToUrl(url);
		}

		public void WaitToInput(By location, string text)
		{
			WaitFor(location);
			Input(location, text);
        }

		public void Input(By location, string text)
		{
			Driver.FindElement(location).SendKeys(text);
		}

		public void ClearText(By location)
		{
			Driver.FindElement(location).Clear();
		}

		public void WaitToClick(By location)
		{
			Wait.Until(ExpectedConditions.ElementToBeClickable(location));
			System.Threading.Thread.Sleep(50);
			Click(location);
		}

		public string WaitToGetText(By location)
		{
			WaitFor(location);
			return GetText(location);
		}

		public string GetText(By location)
		{
			return Driver.FindElement(location).Text;
		}

		public string GetValue(By location)
		{
			return Driver.FindElement(location).GetAttribute("value");
		}

		public bool HasElement(By location)
		{
			return Driver.FindElements(location).Any();
		}

		public void WaitFor(By location)
		{
			Wait.Until(ExpectedConditions.ElementIsVisible(location));
		}

		public virtual void Click(By location)
		{
			Driver.FindElement(location).Click();
		}

		public void ScrollTo(By location)
		{
			var element = Driver.FindElement(location);
			((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
		}

        public void SendKeys(By location, string keyString)
        {
            var key = Keys.Null;
            if (keyString == "Return")
            {
                key = Keys.Return;
            }
            else if (keyString == "Tab")
            {
                key = Keys.Tab;
            }

            Driver.FindElement(location).SendKeys(key);
        }
	}
}