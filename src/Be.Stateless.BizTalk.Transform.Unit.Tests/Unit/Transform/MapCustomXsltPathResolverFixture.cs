#region Copyright & License

// Copyright © 2012 - 2020 François Chabot
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
using System.IO;
using System.Runtime.CompilerServices;
using Be.Stateless.BizTalk.Resources.Transform;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.BizTalk.Unit.Transform
{
	public class MapCustomXsltPathResolverFixture
	{
		[Fact]
		public void TryResolveBtmClassSourceFilePath()
		{
			typeof(TextTransform).TryResolveBtmClassSourceFilePath(out var path).Should().BeTrue();
			path.Should().BeEquivalentTo(Path.Combine(_projectFolder, @"Be.Stateless.BizTalk.Transform.Unit.Tests\Resources\Transform\TextTransform.cs"));
		}

		[Fact]
		public void TryResolveCustomXsltPath()
		{
			typeof(TextTransform).TryResolveCustomXsltPath(out var path).Should().BeTrue();
			path.Should().BeEquivalentTo(Path.Combine(_projectFolder, @"Be.Stateless.BizTalk.Transform.Unit.Tests\Resources\Transform\TextTransform.xslt"));
		}

		[Fact]
		public void TryResolveEmbeddedXsltResourceSourceFilePath()
		{
			typeof(TextTransform).TryResolveEmbeddedXsltResourceSourceFilePath("TextTransform.xslt", out var path).Should().BeTrue();
			path.Should().BeEquivalentTo(Path.Combine(_projectFolder, @"Be.Stateless.BizTalk.Transform.Unit.Tests\Resources\Transform\TextTransform.xslt"));
		}

		static MapCustomXsltPathResolverFixture()
		{
			_projectFolder = ComputeProjectFolder();
		}

		private static string ComputeProjectFolder([CallerFilePath] string sourceFilePath = "")
		{
			return sourceFilePath.Substring(0, sourceFilePath.IndexOf(@"\Be.Stateless.BizTalk.Transform.Unit.Tests", StringComparison.OrdinalIgnoreCase));
		}

		private static readonly string _projectFolder;
	}
}
