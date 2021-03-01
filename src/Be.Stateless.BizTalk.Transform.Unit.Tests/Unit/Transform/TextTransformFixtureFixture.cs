#region Copyright & License

// Copyright © 2012 - 2021 François Chabot
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
using System.Reflection;
using System.Xml;
using Be.Stateless.BizTalk.Dummies.Transform;
using Be.Stateless.BizTalk.Message;
using Be.Stateless.IO.Extensions;
using Be.Stateless.Resources;
using Be.Stateless.Xml.Extensions;
using BTF2Schemas;
using FluentAssertions;
using Xunit;
using static FluentAssertions.FluentActions;

namespace Be.Stateless.BizTalk.Unit.Transform
{
	public class TextTransformFixtureFixture : TransformFixture<TextTransform>
	{
		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void SetupTextTransform()
		{
			using (var stream = _document.AsStream())
			{
				var setup = Given(input => input.Message(stream))
					.Transform
					.OutputsText();

				var result = setup.Validate();

				result.TextContent.Should().Be("services, deliveryReceiptRequest, sendTo, address, sendBy, commitmentReceiptRequest, sendTo, address, sendBy, ");
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void SetupXmlTransformThrowsIfXsltOutputIsText()
		{
			using (var stream = _document.AsStream())
			{
				Invoking(
						() => Given(input => input.Message(stream))
							.Transform
							.OutputsXml(output => output.WithNoConformanceLevel()))
					.Should().Throw<InvalidOperationException>()
					.WithMessage("Transform produces a text output and not an XML one.");
			}
		}

		private readonly XmlDocument _document = MessageBodyFactory.Create<btf2_services_header>(
			ResourceManager.Load(
				Assembly.GetExecutingAssembly(),
				"Be.Stateless.BizTalk.Resources.Message.Sample.xml",
				s => s.ReadToEnd()));
	}
}
