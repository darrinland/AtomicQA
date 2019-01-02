using AtomicWriter.Objects;
using Newtonsoft.Json;
using System.IO;

namespace AtomicWriter.DataAccess
{
	public static class DataReader
	{
		public static bool FileExists(string location)
		{
			return File.Exists(location);
		}

		public static string ReadFromFile(string location)
		{
			using (StreamReader sr = new StreamReader(location))
			{
				string line = string.Empty;
				string result = string.Empty;

				do
				{
					result += line;
					line = sr.ReadLine();
				} while (!string.IsNullOrEmpty(line));

				return result;
			}
		}

		public static SaveObject LoadObject(string location)
		{
			return JsonConvert.DeserializeObject<SaveObject>(ReadFromFile(location));
		}
	}
}
