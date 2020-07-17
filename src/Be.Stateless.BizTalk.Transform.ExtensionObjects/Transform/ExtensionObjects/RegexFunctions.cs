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
using System.Text.RegularExpressions;
using System.Xml.Xsl;

namespace Be.Stateless.BizTalk.Transform.ExtensionObjects
{
	/// <summary>
	/// XSLT extension object offering support for regular expressions.
	/// </summary>
	/// <seealso cref="XsltArgumentList.AddExtensionObject"/>
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "XSLT Extension Object.")]
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "XSLT Extension Object.")]
	public class RegexFunctions
	{
		/// <summary>
		/// Indicates whether the specified regular expression finds a match in the specified input string.
		/// </summary>
		/// <param name="input">
		/// The string to search for a match.
		/// </param>
		/// <param name="pattern">
		/// The regular expression pattern to match.
		/// </param>
		/// <returns>
		/// <c>true</c> if the regular expression finds a match; <c>false</c> otherwise.
		/// </returns>
		/// <seealso cref="Regex.IsMatch(string,string)"/>
		public bool IsMatch(string input, string pattern)
		{
			return Regex.IsMatch(input, pattern);
		}

		/// <summary>
		/// Indicates whether the specified regular expression finds a match in the <b>whole</b> specified input string.
		/// </summary>
		/// <param name="input">
		/// The string to search for a match.
		/// </param>
		/// <param name="pattern">
		/// The regular expression pattern to match.
		/// </param>
		/// <returns>
		/// <c>true</c> if the regular expression finds a match; <c>false</c> otherwise.
		/// </returns>
		/// <seealso cref="Regex.IsMatch(string,string)"/>
		public bool IsOneOf(string input, string pattern)
		{
			return Regex.IsMatch(input, "^(" + pattern + ")$");
		}

		/// <summary>
		/// In a specified input string, replaces all strings that match a specified regular expression with a specified
		/// replacement string.
		/// </summary>
		/// <param name="input">
		/// The string to search for a match.
		/// </param>
		/// <param name="pattern">
		/// The regular expression pattern to match.
		/// </param>
		/// <param name="replacement">
		/// The replacement string.
		/// </param>
		/// <returns>
		/// A new string that is identical to the input string, except that the replacement string takes the place of each matched
		/// string. If pattern is not matched in the current instance, the method returns the current instance unchanged.
		/// </returns>
		/// <seealso cref="Regex.Replace(string,string,string)"/>
		public string Replace(string input, string pattern, string replacement)
		{
			return Regex.Replace(input, pattern, replacement);
		}
	}
}
