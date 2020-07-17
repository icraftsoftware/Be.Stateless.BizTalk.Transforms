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
using System.Reflection;
using System.Xml.Linq;
using Be.Stateless.BizTalk.Dummies.Transform;
using Be.Stateless.BizTalk.Unit.Transform;
using Be.Stateless.Resources;
using FluentAssertions;
using Microsoft.XLANGs.BaseTypes;
using Xunit;

namespace Be.Stateless.BizTalk.Transform.ExtensionObjects
{
	public class XmlConversionFunctionsFixture : TransformFixture<BoolTransform>
	{
		[SuppressMessage("ReSharper", "AccessToDisposedClosure")]
		[Fact]
		public void ToBoolean()
		{
			using (var stream = ResourceManager.Load(Assembly.GetExecutingAssembly(), "Be.Stateless.BizTalk.Resources.Message.Bool.xml"))
			{
				var result = Given(input => input.Message(stream))
					.Transform
					.OutputsXml(output => output.ConformingTo<Any>().WithLaxConformanceLevel())
					.Validate();

				XDocument.Parse(result.XmlContent)
					.Should().BeEquivalentTo(
						XDocument.Parse(
							@"<s0:Message xmlns:s0='urn:resources:message:bool'>
  <s0:false_int a='false'>false</s0:false_int>
  <s0:false_value a='false'>false</s0:false_value>
  <s0:true_int a='true'>true</s0:true_int>
  <s0:true_value a='true'>true</s0:true_value>
</s0:Message>"));
			}
		}
	}
}
