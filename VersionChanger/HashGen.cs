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

		public static string GetHash(string folder)
		{
			//Calculate a hash for each file
			string totalHash = "";
			foreach (string item in Directory.EnumerateFiles(folder))
			{
				totalHash += CalculateMD5File(item);
			}


			//Calculate hash of hashes to store a single value
			using (var md5 = MD5.Create())
			{
				var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(totalHash));
				return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
			}

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
