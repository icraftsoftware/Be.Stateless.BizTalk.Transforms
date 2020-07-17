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

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using Be.Stateless.BizTalk.Dummies.Transform;
using Be.Stateless.BizTalk.Message;
using Be.Stateless.BizTalk.Schema;
using Be.Stateless.IO.Extensions;
using Be.Stateless.Resources;
using Be.Stateless.Xml.Extensions;
using BTF2Schemas;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.BizTalk.Unit.Transform
{
	public class TransformFixtureXmlResultFixture : TransformFixture<IdentityTransform>
	{
		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		public void ScalarAssertion()
		{
			using (var stream = _document.AsStream())
			{
				var setup = Given(input => input.Message(stream))
					.Transform
					.OutputsXml(output => output.ConformingTo<btf2_services_header>().WithStrictConformanceLevel());

				var result = setup.Validate();

				result.NamespaceManager.AddNamespace("tns", SchemaMetadata.For<btf2_services_header>().TargetNamespace);
				result.SelectSingleNode("//*[1]/tns:sendBy/text()").Value.Should().Be("2012-04-12T12:13:14");
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void StringJoinAssertion()
		{
			using (var stream = _document.AsStream())
			{
				var setup = Given(input => input.Message(stream))
					.Transform
					.OutputsXml(output => output.ConformingTo<btf2_services_header>().WithStrictConformanceLevel());

				var result = setup.Validate();

				result.NamespaceManager.AddNamespace("tns", SchemaMetadata.For<btf2_services_header>().TargetNamespace);
				result.StringJoin("//tns:sendBy").Should().Be("2012-04-12T12:13:14#2012-04-12T23:22:21");
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void XPathAssertion()
		{
			using (var stream = _document.AsStream())
			{
				var setup = Given(input => input.Message(stream))
					.Transform
					.OutputsXml(output => output.ConformingTo<btf2_services_header>().WithStrictConformanceLevel());

				var result = setup.Validate();

				result.NamespaceManager.AddNamespace("tns", SchemaMetadata.For<btf2_services_header>().TargetNamespace);
				result.Select("//tns:sendBy").Cast<XPathNavigator>().Should().HaveCount(2);
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		public void XPathNavigatorResultingOfSelectCanBeReusedToSimplifyXPathExpression()
		{
			using (var stream = _document.AsStream())
			{
				var setup = Given(input => input.Message(stream))
					.Transform
					.OutputsXml(
						output => output
							.ConformingTo<btf2_services_header>()
							.WithStrictConformanceLevel());

				var result = setup.Validate();

				result.NamespaceManager.AddNamespace("ns", SchemaMetadata.For<btf2_services_header>().TargetNamespace);
				var deliveryReceiptRequest = result.SelectSingleNode("/ns:services/ns:deliveryReceiptRequest");
				deliveryReceiptRequest.Should().NotBeNull();
				deliveryReceiptRequest.SelectSingleNode("ns:sendBy").Value.Should().Be("2012-04-12T12:13:14");
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void XPathNavigatorResultingOfSelectIsNullWhenNodeIsNotFound()
		{
			using (var stream = _document.AsStream())
			{
				var setup = Given(input => input.Message(stream))
					.Transform
					.OutputsXml(output => output.ConformingTo<btf2_services_header>().WithStrictConformanceLevel());

				var result = setup.Validate();

				result.NamespaceManager.AddNamespace("tns", SchemaMetadata.For<btf2_services_header>().TargetNamespace);
				result.SelectSingleNode("//*[1]/tns:unknown").Should().BeNull();
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void XPathNodeIteratorResultingOfSelectIsEmptyWhenNodeIsNotFound()
		{
			using (var stream = _document.AsStream())
			{
				var setup = Given(input => input.Message(stream))
					.Transform
					.OutputsXml(output => output.ConformingTo<btf2_services_header>().WithStrictConformanceLevel());

				var result = setup.Validate();

				result.NamespaceManager.AddNamespace("tns", SchemaMetadata.For<btf2_services_header>().TargetNamespace);
				result.Select("//*[1]/tns:unknown").Cast<XmlNode>().Should().BeEmpty();
			}
		}

		private readonly XmlDocument _document = MessageBodyFactory.Create<btf2_services_header>(
			ResourceManager.Load(
				Assembly.GetExecutingAssembly(),
				"Be.Stateless.BizTalk.Resources.Message.Sample.xml",
				s => s.ReadToEnd()));
	}
}
