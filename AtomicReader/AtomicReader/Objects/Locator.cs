using OpenQA.Selenium;

namespace AtomicReader.Objects
{
	public class Locator
	{
		public LocatorTypes LocatorType { get; set; }
		public string Path { get; set; }

		public enum LocatorTypes
		{
			XPath,
			Id,
			ClassName,
			CssSelector,
			LinkText,
			Name,
			PartialLinkText,
			TagName,
		}

		public static By GetByLocator(LocatorTypes type, string path)
		{
			return (By)typeof(By).GetMethod(type.ToString()).Invoke(null, new object[] { path });
		}
	}
}
