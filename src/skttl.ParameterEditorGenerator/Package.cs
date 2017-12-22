using System;
using Semver;

namespace skttl.ParameterEditorGenerator
{
	public static class Package
	{
		public const string Alias = "skttl.ParameterEditorGenerator";

		public const string Name = "Parameter Editor Generator";

		public static readonly Version Version = typeof(Package).Assembly.GetName().Version;

		public static readonly SemVersion SemVersion = new SemVersion(Version.Major, Version.Minor, Version.Build);
	}
}
