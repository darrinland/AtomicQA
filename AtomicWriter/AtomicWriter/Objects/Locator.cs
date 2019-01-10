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
	}
}
