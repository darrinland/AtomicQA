using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumBase
{
	class ChromeDriver : BaseDriver
	{
		private ChromeOptions _options;

		public ChromeDriver(int timeout = 60, ChromeOptions options = null)
		: base(timeout)
		{
			_options = options;
			Driver = StartDriver();
		}

		protected override IWebDriver StartDriver()
		{
			if (_options == null)
			{
				_options = new ChromeOptions();
				_options.AddArgument("--incognito");
				_options.AddArgument("--start-maximized");
			}

			return new OpenQA.Selenium.Chrome.ChromeDriver(_options);
		}
	}
}
