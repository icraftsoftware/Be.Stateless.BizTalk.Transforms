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
using System.Xml;
using System.Xml.Xsl;

namespace Be.Stateless.BizTalk.Transform.ExtensionObjects
{
	/// <summary>
	/// XSLT extension object offering support for XML conversions.
	/// </summary>
	/// <seealso cref="XsltArgumentList.AddExtensionObject"/>
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "XSLT Extension Object.")]
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "XSLT Extension Object.")]
	public class XmlConversionFunctions
	{
		/// <summary>
		/// Converts the <paramref name="@string"/> String to a Boolean equivalent.
		/// </summary>
		/// <param name="string">
		/// The string to convert.
		/// </param>
		/// <returns>
		/// A <see cref="bool"/> value, that is <c>true</c> or <c>false</c>.
		/// </returns>
		/// <remarks>
		/// <para>
		/// According to XML Schema Definition Language <see href="https://www.w3.org/TR/xmlschema11-2/#boolean">boolean</see>, a
		/// boolean can have either of the following values: <c>'true'</c>, <c>'false'</c>, <c>'1'</c>, or <c>'0'</c>. Converting
		/// a boolean literal to a boolean value therefore requires to write the following expression in XPath 1.0:
		/// <c>&lt;xsl:value-of select='boolean(number(text())) = true() or text() = 'true'' /&gt;</c>.
		/// </para>
		/// <para>
		/// This function provides a shorter way of doing it: <c>&lt;xsl:value-of select='xc:ToBoolean(text())' /&gt;</c>,
		/// assuming <c>xc</c> denotes the XML namespace referencing the extension object <see cref="XmlConversionFunctions"/>.
		/// </para>
		/// </remarks>
		public bool ToBoolean(string @string)
		{
			return XmlConvert.ToBoolean(@string);
		}
	}
}
