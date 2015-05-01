using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;

namespace VersionUpdater {
	class MainClass {
		public static void Main(string[] args) {
			if (args == null || args.Length < 2) {
				Console.WriteLine(@"Sample usage: ");
				Console.WriteLine(@"VersionUpdater c:\path\\to\\assembly.dll c:\path\\to\\AndroidManifest.xml name[optional]");
				return;
			}
			if (!Check(args[0], "Assembly") || !Check(args[1], "Manifest")) {
				return;
			}
			var version = GetVersion(args[0]);

			string prefix = args.Length >= 3 ? args[2] : "";

			SetVersion(args[1], version, prefix);
		}

		static bool Check(string file, string fileName) {
			if (File.Exists(Path.GetFullPath(file)))
				return true;
			Console.WriteLine(fileName + " not found");
			return false;
		}

		static Version GetVersion(string file) {
			var assembly = Assembly.LoadFile(Path.GetFullPath(file));
			return assembly.GetName().Version;
		}

		private static readonly Regex VersionCodeRegex = new Regex("android:versionCode=\"(?<version>.*?)\"", RegexOptions.Compiled);
		private static readonly Regex VersionNameRegex = new Regex("android:versionName=\"(?<version>.*?)\"", RegexOptions.Compiled);


		static void SetVersion(string file, Version version, string prefix) {
			var manifest = File.ReadAllText(Path.GetFullPath(file), Encoding.UTF8);
			File.WriteAllText(Path.GetFullPath(file + ".bkp"), manifest, Encoding.UTF8);
			string name = string.Format("android:versionName=\"{0}{1}\"", prefix, version.ToString());
			Console.WriteLine(name);
			manifest = VersionNameRegex.Replace(manifest, name, 1);
            string code = string.Format("android:versionCode=\"{0}\"", version.Build * 100000 + version.Revision);
			Console.WriteLine(code);
			manifest = VersionCodeRegex.Replace(manifest, code, 1);
			File.WriteAllText(file, manifest, Encoding.UTF8);
		}

	}
}
