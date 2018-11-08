using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VersionChanger
{
	class HashGen
	{

		public static IEnumerable<string> GetHashes(string folder)
		{
			List<string> hashes = new List<string>();
			//Calculate a hash for each file
			foreach (string item in Directory.EnumerateFiles(Path.Combine(folder, "Launcher")))
			{
				hashes.Add(CalculateMD5File(item));
			}

			hashes.Sort();

			List<string> tempHashList = new List<string>();

			foreach (string item in Directory.EnumerateFiles(@folder))
			{
				tempHashList.Add(CalculateMD5File(item));
			}

			tempHashList.Sort();

			hashes.AddRange(tempHashList);

			return hashes;
		}


		private static string CalculateMD5File(string filename)
		{
			using (var md5 = MD5.Create())
			{
				using (var stream = File.OpenRead(@filename))
				{
					var hash = md5.ComputeHash(stream);
					return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
				}
			}
		}
	}
}
