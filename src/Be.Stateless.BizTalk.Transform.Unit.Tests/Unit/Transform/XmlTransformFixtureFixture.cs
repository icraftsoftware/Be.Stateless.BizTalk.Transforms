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
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using Be.Stateless.BizTalk.Dummies.Transform;
using Be.Stateless.BizTalk.Message;
using Be.Stateless.IO.Extensions;
using Be.Stateless.Resources;
using Be.Stateless.Xml.Extensions;
using BTF2Schemas;
using BTS;
using FluentAssertions;
using Xunit;
using static FluentAssertions.FluentActions;

namespace Be.Stateless.BizTalk.Unit.Transform
{
	public class XmlTransformFixtureFixture : TransformFixture<IdentityTransform>
	{
		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void InvalidInputMessageThrowsInputRelatedXmlSchemaValidationExceptionWithEmbeddedXmlContent()
		{
			using (var stream1 = _document.AsStream())
			using (var stream2 = _document.AsStream())
			{
				Invoking(
						() => Given(
								input => input
									.Arguments(new())
									.Context(new MessageContextMock().Object)
									.Message<soap_envelope_1__2.Envelope>(stream1)
									.Message(stream2))
							.Transform
							.OutputsXml(
								output => output
									.WithValuednessValidationCallback((_, args) => args.Severity = XmlSeverityType.Warning)
									.WithNoConformanceLevel()))
					.Should().Throw<XmlSchemaValidationException>()
					.WithMessage("Transform's input message #1 failed 'Envelope' schema validation for the following reason:*");
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void InvalidTransformOutputThrowsOutputRelatedXmlSchemaValidationExceptionWithEmbeddedXmlContent()
		{
			using (var stream = _document.AsStream())
			{
				var setup = Given(input => input.Message(stream))
					.Transform
					.OutputsXml(
						output => output
							.ConformingTo<btf2_receipt_header>()
							.ConformingTo<soap_envelope_1__2.Fault>()
							.WithStrictConformanceLevel());

				Invoking(() => setup.Validate())
					.Should().Throw<XmlSchemaValidationException>()
					.WithMessage("Transform's output failed schema(s) validation for the following reason:*");
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void InvalidTransformResultThrows()
		{
			using (var stream = MessageBodyFactory.Create<btf2_services_header>().AsStream())
			{
				var setup = Given(input => input.Message(stream))
					.Transform
					.OutputsXml(output => output.ConformingTo<btf2_services_header>().WithStrictConformanceLevel());

				Invoking(() => setup.Validate()).Should().Throw<XmlSchemaValidationException>();
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void SetupTextTransformThrowsIfXsltOutputIsXml()
		{
			using (var stream = _document.AsStream())
			{
				Invoking(
						() => Given(input => input.Message(stream))
							.Transform
							.OutputsText())
					.Should().Throw<InvalidOperationException>()
					.WithMessage("Transform produces an XML output and not a text one.");
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void SetupValuednessValidationCallbackConfirmingSeverity()
		{
			using (var stream = MessageBodyFactory.Create<btf2_services_header>().AsStream())
			{
				var setup = Given(input => input.Message(stream))
					.Transform
					.OutputsXml(
						output => output
							.WithValuednessValidationCallback((_, args) => args.Severity = XmlSeverityType.Error)
							.ConformingTo<btf2_services_header>().WithNoConformanceLevel());

				Invoking(() => setup.Validate())
					.Should().Throw<XmlException>()
					.Where(
						exception => exception.Message.Contains("/ns0:services/ns0:deliveryReceiptRequest/ns0:sendTo/ns0:address")
							&& exception.Message.Contains("/ns0:services/ns0:deliveryReceiptRequest/ns0:sendBy")
							&& exception.Message.Contains("/ns0:services/ns0:commitmentReceiptRequest/ns0:sendBy"));
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void SetupValuednessValidationCallbackDemotingSeverity()
		{
			using (var stream = MessageBodyFactory.Create<btf2_services_header>().AsStream())
			{
				var setup = Given(input => input.Message(stream))
					.Transform
					.OutputsXml(
						output => output
							.WithValuednessValidationCallback((_, args) => args.Severity = XmlSeverityType.Warning)
							.ConformingTo<btf2_services_header>().WithNoConformanceLevel()
					);

				Invoking(() => setup.Validate()).Should().NotThrow();
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void SetupXmlTransformWithMultipleInputs()
		{
			using (var stream1 = _document.AsStream())
			using (var stream2 = _document.AsStream())
			{
				var setup = Given(
						input => input
							.Arguments(new())
							.Context(new MessageContextMock().Object)
							.Message<btf2_services_header>(stream1)
							.Message(stream2))
					.Transform
					.OutputsXml(
						output => output
							.ConformingTo<btf2_services_header>()
							.WithLaxConformanceLevel());

				Invoking(() => setup.Validate()).Should().NotThrow();
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void SetupXmlTransformWithoutConformanceLevel()
		{
			using (var stream = _document.AsStream())
			{
				var setup = Given(input => input.Message(stream))
					.Transform
					.OutputsXml(output => output.ConformingTo<soap_envelope_1__2.Envelope>().WithNoConformanceLevel());

				Invoking(() => setup.Validate()).Should().NotThrow();
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void SetupXmlTransformWithoutConformingSchemaAndConformanceLevel()
		{
			using (var stream = MessageBodyFactory.Create<btf2_services_header>().AsStream())
			{
				var setup = Given(input => input.Message(stream))
					.Transform
					.OutputsXml(
						output => output
							.WithValuednessValidationCallback((_, args) => args.Severity = XmlSeverityType.Warning)
							.WithNoConformanceLevel());

				Invoking(() => setup.Validate()).Should().NotThrow();
			}
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void SetupXmlTransformWithoutConformingSchemaButWithStrictConformanceLevel()
		{
			using (var stream = _document.AsStream())
			{
				Invoking(
						() => Given(input => input.Message(stream))
							.Transform
							.OutputsXml(output => output.WithStrictConformanceLevel()))
					.Should().Throw<InvalidOperationException>()
					.WithMessage("At least one XML Schema to which the output must conform to must be setup.");
			}
		}

		[Fact]
		public void SetupXmlTransformWithoutInputMessage()
		{
			Invoking(
					() => Given(_ => { })
						.Transform
						.OutputsXml(output => output.WithStrictConformanceLevel()))
				.Should().Throw<InvalidOperationException>()
				.WithMessage("At least one input message must be setup.");
		}

		[Fact]
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		public void ValidTransformResultDoesNotThrow()
		{
			using (var stream = _document.AsStream())
			{
				var setup = Given(input => input.Message(stream))
					.Transform
					.OutputsXml(output => output.ConformingTo<btf2_services_header>().WithStrictConformanceLevel());

				Invoking(() => setup.Validate()).Should().NotThrow();
			}
		}

		private readonly XmlDocument _document = MessageBodyFactory.Create<btf2_services_header>(
			ResourceManager.Load(Assembly.GetExecutingAssembly(), "Be.Stateless.BizTalk.Resources.Message.Sample.xml").ReadToEnd());
	}
}
