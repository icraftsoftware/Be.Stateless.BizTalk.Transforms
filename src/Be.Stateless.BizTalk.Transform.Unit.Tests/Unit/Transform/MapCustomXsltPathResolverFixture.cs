#region Copyright & License

// Copyright © 2012 - 2022 François Chabot
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Be.Stateless.BizTalk.Dummies.Transform;
using FluentAssertions;
using Microsoft.VisualStudio.Dia;
using Xunit;

namespace Be.Stateless.BizTalk.Unit.Transform
{
	public class MapCustomXsltPathResolverFixture
	{
		#region Setup/Teardown

		static MapCustomXsltPathResolverFixture()
		{
			_projectFolder = ComputeProjectFolder();
		}

		#endregion

		[SkippableFact]
		public void TryResolveBtmClassSourceFilePath()
		{
			Skip.IfNot(IsDebugInterfaceAccessComClassRegistered());
			// does not pass when run in shell via dotnet test
			Skip.IfNot(typeof(TextTransform).TryResolvePdbFilePath(out _));
			typeof(TextTransform).TryResolveBtmClassSourceFilePath(out var path).Should().BeTrue();
			path.Should().BeEquivalentTo(Path.Combine(_projectFolder, @"Dummies\Transform\TextTransform.cs"));
		}

		[SkippableFact]
		public void TryResolveCustomXsltPath()
		{
			Skip.IfNot(IsDebugInterfaceAccessComClassRegistered());
			typeof(TextTransform).TryResolveCustomXsltPath(out var path).Should().BeTrue();
			path.Should().BeEquivalentTo(Path.Combine(_projectFolder, @"Dummies\Transform\TextTransform.xslt"));
		}

		[SkippableFact]
		public void TryResolveEmbeddedXsltResourceSourceFilePath()
		{
			Skip.IfNot(IsDebugInterfaceAccessComClassRegistered());
			typeof(TextTransform).TryResolveEmbeddedXsltResourceSourceFilePath("TextTransform.xslt", out var path).Should().BeTrue();
			path.Should().BeEquivalentTo(Path.Combine(_projectFolder, @"Dummies\Transform\TextTransform.xslt"));
		}

		private static string ComputeProjectFolder([CallerFilePath] string sourceFilePath = "")
		{
			var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
			return sourceFilePath.Substring(0, sourceFilePath.IndexOf($@"\{assemblyName}", StringComparison.OrdinalIgnoreCase) + assemblyName.Length + 1);
		}

		[SuppressMessage("ReSharper", "UnusedVariable")]
		private bool IsDebugInterfaceAccessComClassRegistered()
		{
			try
			{
				var source = (IDiaDataSource) new DiaSourceClass();
				return true;
			}
			catch (COMException)
			{
				return false;
			}
		}

		private static readonly string _projectFolder;
	}
}
