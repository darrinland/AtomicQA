using OpenQA.Selenium;
using System.Collections.Generic;

namespace AtomicWriter.Objects
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

		public static List<LocatorTypes> GetLocatorTypes()
		{
			return new List<LocatorTypes>
			{
				LocatorTypes.XPath,
				LocatorTypes.Id,
				LocatorTypes.ClassName,
				LocatorTypes.CssSelector,
				LocatorTypes.LinkText,
				LocatorTypes.Name,
				LocatorTypes.PartialLinkText,
				LocatorTypes.TagName
			};
		}
		
		public static By GetByLocator(LocatorTypes type, string path)
		{
			return (By)typeof(By).GetMethod(type.ToString()).Invoke(null, new object[] { path });
		}
	}
}
