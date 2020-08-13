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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using Be.Stateless.BizTalk.Unit.Xml;
using Be.Stateless.BizTalk.Unit.Xml.Extensions;
using Be.Stateless.BizTalk.Unit.Xml.XPath.Extensions;
using Be.Stateless.IO.Extensions;
using Be.Stateless.Xml;
using Be.Stateless.Xml.XPath.Extensions;
using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.Unit.Transform
{
	internal class TransformFixtureXmlOutputSetup<TTransform> : ITransformFixtureXmlOutputSetup, ISystemUnderTestSetup<TransformFixtureXmlResult>
		where TTransform : TransformBase, new()
	{
		internal TransformFixtureXmlOutputSetup(System.IO.Stream xsltOutputStream, Action<ITransformFixtureXmlOutputSetup> xmlOutputSetupConfigurator)
		{
			if (xmlOutputSetupConfigurator == null) throw new ArgumentNullException(nameof(xmlOutputSetupConfigurator));
			XsltOutputStream = xsltOutputStream ?? throw new ArgumentNullException(nameof(xsltOutputStream));
			ContentProcessing = XmlSchemaContentProcessing.Strict;
			Schemas = new List<XmlSchema>();

			xmlOutputSetupConfigurator(this);
			ValidateSetup();
		}

		#region ISystemUnderTestSetup<TransformFixtureXmlResult> Members

		public TransformFixtureXmlResult Validate()
		{
			using (XsltOutputStream)
			using (var xsltResultReader = CreateValidationAwareReader(XsltOutputStream))
			{
				var navigator = new XPathDocument(xsltResultReader).CreateNavigator();
				var xmlResult = new TransformFixtureXmlResult(navigator, navigator.GetNamespaceManager().AddNamespaces<TTransform>());
				xmlResult.CheckValuedness(ValuednessValidationCallback);
				return xmlResult;
			}
		}

		#endregion

		#region ITransformFixtureXmlOutputSetup Members

		public ITransformFixtureXmlOutputSetup WithValuednessValidationCallback(ValuednessValidationCallback valuednessValidationCallback)
		{
			ValuednessValidationCallback = valuednessValidationCallback;
			return this;
		}

		public ITransformFixtureXmlOutputSetup ConformingTo<T>() where T : SchemaBase, new()
		{
			Schemas.Add(new T().CreateResolvedSchema());
			return this;
		}

		public ISystemUnderTestSetup<TransformFixtureXmlResult> WithConformanceLevel(XmlSchemaContentProcessing xmlSchemaContentProcessing)
		{
			ContentProcessing = xmlSchemaContentProcessing;
			return this;
		}

		public ISystemUnderTestSetup<TransformFixtureXmlResult> WithNoConformanceLevel()
		{
			return WithConformanceLevel(XmlSchemaContentProcessing.Skip);
		}

		public ISystemUnderTestSetup<TransformFixtureXmlResult> WithLaxConformanceLevel()
		{
			return WithConformanceLevel(XmlSchemaContentProcessing.Lax);
		}

		public ISystemUnderTestSetup<TransformFixtureXmlResult> WithStrictConformanceLevel()
		{
			return WithConformanceLevel(XmlSchemaContentProcessing.Strict);
		}

		#endregion

		private XmlSchemaContentProcessing ContentProcessing { get; set; }

		private List<XmlSchema> Schemas { get; }

		private ValuednessValidationCallback ValuednessValidationCallback { get; set; }

		private System.IO.Stream XsltOutputStream { get; }

		[SuppressMessage("ReSharper", "SwitchStatementHandlesSomeKnownEnumValuesWithDefault")]
		private void ValidateSetup()
		{
			using (var stringReader = new StringReader(new TTransform().XmlContent))
			using (var xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings { XmlResolver = null }))
			{
				var navigator = new XPathDocument(xmlReader).CreateNavigator();
				var output = navigator.SelectSingleNode("/xsl:stylesheet/xsl:output/@method", navigator.GetNamespaceManager().AddNamespaces<TTransform>());
				if (output != null && !output.Value.Equals("xml", StringComparison.OrdinalIgnoreCase))
					throw new InvalidOperationException($"Transform produces a {output.Value} output and not an XML one.");
			}

			switch (ContentProcessing)
			{
				case XmlSchemaContentProcessing.None:
				case XmlSchemaContentProcessing.Skip:
					break;
				case XmlSchemaContentProcessing.Lax:
				case XmlSchemaContentProcessing.Strict:
					if (!Schemas.Any()) throw new InvalidOperationException("At least one XML Schema to which the output must conform to must be setup.");
					break;
			}
		}

		private XmlReader CreateValidationAwareReader(System.IO.Stream transformStream)
		{
			var validatingXmlReader = XmlReader.Create(
				transformStream,
				ValidatingXmlReaderSettings.Create(
					ContentProcessing,
					(sender, args) => throw new XmlSchemaValidationException(
						$"Transform's output failed schema(s) validation for the following reason:{Environment.NewLine}{args.Severity}: {args.Message}{Environment.NewLine}{Environment.NewLine}The message's content is:{Environment.NewLine}{transformStream.ReadToEnd()}{Environment.NewLine}",
						args.Exception),
					Schemas.ToArray()));
			return validatingXmlReader;
		}
	}
}
