using AtomicReader.Objects;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace AtomicReader.DataAccess
{
	public static class DataWriter
	{
		public static void Save(string location, object saveObject)
		{
			WriteToFile(location, JsonConvert.SerializeObject(saveObject));
		}

		private static void WriteToFile(string location, string data)
		{
			using (StreamWriter sw = new StreamWriter(location, false, Encoding.ASCII))
			{
				sw.Write(data);
			}
		}
	}
}
