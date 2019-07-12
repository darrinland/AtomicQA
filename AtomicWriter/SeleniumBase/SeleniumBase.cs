using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumBase
{
	public class SeleniumBase : IDisposable
	{
		protected BaseDriver _baseDriver;
		protected IWebDriver Driver;

		public SeleniumBase()
		{
			CreateChromeDriver();
		}

		public string CurrentUrl => Driver.Url;

		public void Dispose()
		{
			Driver.Dispose();
		}

		public void CreateChromeDriver(int timeoutSeconds = 30)
		{
			_baseDriver = new ChromeDriver(timeoutSeconds);
			Driver = _baseDriver.Driver;
			Driver.Manage().Window.Size = new System.Drawing.Size(1800, 1100);
			Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(30.0);
		}

		public void NavigateToUrl(string url)
		{
			_baseDriver.NavigateToUrl(url);
		}

		public void WaitToInput(By location, string text)
		{
			_baseDriver.WaitToInput(location, text);
		}

		private void Input(By location, string text)
		{
			_baseDriver.Input(location, text);
		}

		public void ClearText(By location)
		{
			_baseDriver.ClearText(location);
		}

		public void WaitToClick(By location)
		{
			_baseDriver.WaitToClick(location);
		}

		public string WaitToGetText(By location)
		{
			return _baseDriver.WaitToGetText(location);
		}

		private string GetText(By location)
		{
			return _baseDriver.GetText(location);
		}

		public string GetValue(By location)
		{
			return _baseDriver.GetValue(location);
		}

		public bool HasElement(By location)
		{
			return _baseDriver.HasElement(location);
		}

		public void WaitFor(By location)
		{
			_baseDriver.WaitFor(location);
		}

		public void Click(By location)
		{
			_baseDriver.Click(location);
		}

		public void ScrollTo(By location)
		{
			_baseDriver.ScrollTo(location);
		}

		public Screenshot GetScreenshot()
		{
			return ((ITakesScreenshot)Driver).GetScreenshot();
		}

		public void SendKeys(By location, string keyString)
		{
			_baseDriver.SendKeys(location, keyString);
		}
    }
}
