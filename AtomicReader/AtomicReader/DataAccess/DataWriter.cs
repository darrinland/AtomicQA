using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace AtomicReader.DataAccess
{
	public class DataWriter
	{
		public void Save(string location, SaveObject saveObject)
		{
			WriteToFile(location, JsonConvert.SerializeObject(saveObject));
		}

		private void WriteToFile(string location, string data)
		{
			using (StreamWriter sw = new StreamWriter(location, false, Encoding.ASCII))
			{
				sw.Write(data);
			}
		}
	}
}
