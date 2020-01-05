using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace devio.Powershell
{
	// this code was originally posted at
	// http://stackoverflow.com/questions/2664028/how-can-i-get-powershell-added-types-to-use-added-types/2669782#2669782
	// I added the AddAssemblyPath method and the assembly lookup added paths
	// published under license for StackOverflow/StackExchange
	// CC BY-SA http://creativecommons.org/licenses/by-sa/3.0/

	public class AssemblyResolver
	{
		private static Dictionary<string, string> _assemblies;
		private static List<string> paths;

		static AssemblyResolver()
		{
			var comparer = StringComparer.CurrentCultureIgnoreCase;
			_assemblies = new Dictionary<string, string>(comparer);
			paths = new List<string>();
			AppDomain.CurrentDomain.AssemblyResolve += ResolveHandler;
		}

		public static void AddAssemblyLocation(string path)
		{
			// This should be made threadsafe for production use
			string name = Path.GetFileNameWithoutExtension(path);
			_assemblies.Add(name, path);
		}

		public static void AddAssemblyPath(string path)
		{
			paths.Add(path);
		}

		private static Assembly ResolveHandler(object sender, ResolveEventArgs args)
		{
			var assemblyName = new AssemblyName(args.Name);
			if (_assemblies.ContainsKey(assemblyName.Name) && File.Exists(_assemblies[assemblyName.Name]))
			{
				return Assembly.LoadFrom(_assemblies[assemblyName.Name]);
			}
			foreach (var path in paths)
			{
				var filename = Path.Combine(path, assemblyName.Name) + ".dll";
				if (File.Exists(filename))
					return Assembly.LoadFrom(filename);
			}
			return null;
		}
	}
}
