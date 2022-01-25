﻿#region Copyright & License

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
using System.Xml.XPath;

namespace Be.Stateless.BizTalk.Unit.Transform
{
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
	[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "Public API.")]
	public interface ITransformFixtureXmlResult : IFluentInterface
	{
		/// <summary>
		/// Namespace resolver initialized with each of the namespaces and their prefixes declared in the XSLT.
		/// </summary>
		XmlNamespaceManager NamespaceManager { get; }

		/// <summary>
		/// The whole result of the transform as an XML string.
		/// </summary>
		string XmlContent { get; }

		/// <summary>
		/// Evaluates the specified XPath expression and returns the typed result, implicitly using the <see
		/// cref="NamespaceManager" /> to resolve namespace prefixes in the XPath expression.
		/// </summary>
		/// <param name="xpath">
		/// A <see cref="string" /> representing an XPath expression that can be evaluated.
		/// </param>
		/// <returns>
		/// The result of the expression, either a Boolean, a number, a string, or a node set. This maps to <see cref="bool" />,
		/// <see cref="double" />, <see cref="string" />, or <see cref="System.Xml.XPath.XPathNodeIterator" /> objects
		/// respectively.
		/// </returns>
		object Evaluate(string xpath);

		/// <summary>
		/// Determines whether the current node matches the specified XPath expression implicitly using the <see
		/// cref="NamespaceManager" /> to resolve namespace prefixes.
		/// </summary>
		/// <param name="xpath">
		/// A <see cref="string" /> representing an XPath expression.
		/// </param>
		/// <returns>
		/// <see langword="true" /> if the current node matches the specified XPath expression; otherwise, <see langword="false"
		/// />.
		/// </returns>
		bool Matches(string xpath);

		/// <summary>
		/// Selects a node set using the specified XPath expression implicitly using the <see cref="NamespaceManager" /> to
		/// resolve namespace prefixes in the XPath expression.
		/// </summary>
		/// <param name="xpath">
		/// A <see cref="string" /> representing an XPath expression.
		/// </param>
		/// <returns>
		/// An <see cref="XPathNodeIterator" /> pointing to the selected node set.
		/// </returns>
		XPathNodeIterator Select(string xpath);

		/// <summary>
		/// Selects a single node in the <see cref="XPathNavigator" /> object using the specified XPath query and the implicit
		/// <see cref="NamespaceManager" /> to resolve namespace prefixes.
		/// </summary>
		/// <param name="xpath">
		/// A <see cref="string" /> representing an XPath expression.
		/// </param>
		/// <returns>
		/// An <see cref="XPathNavigator" /> object that contains the first matching node for the XPath query specified;
		/// otherwise <see langword="null" /> if there are no query results.
		/// </returns>
		XPathNavigator SelectSingleNode(string xpath);

		/// <summary>
		/// Concatenates all the text values of the nodes selected using the specified XPath expression and uses the specified
		/// separator between each value.
		/// </summary>
		/// <param name="xpath">
		/// A <see cref="string"/> representing an XPath expression.
		/// </param>
		/// <param name="separator">
		/// The <see cref="char"/> to use as a separator; it defaults to '#'.
		/// </param>
		/// <returns>
		/// A string that consists of the selected nodes' values delimited by the separator <see cref="char"/>.
		/// </returns>
		/// <remarks>
		/// <see cref="StringJoin(string,char)"/> automatically benefits of an <see cref="IXmlNamespaceResolver"/>
		/// resolver that declares all of the namespaces that the XSLT transform of <see cref="TransformFixture{TTransform}"/> have
		/// declared.
		/// </remarks>
		/// <seealso href="http://www.w3.org/TR/2002/WD-xquery-operators-20021115/#func-string-join"/>
		string StringJoin(string xpath, char separator = '#');
	}
}
