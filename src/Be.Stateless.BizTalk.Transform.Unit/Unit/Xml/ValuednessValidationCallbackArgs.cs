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
using System.Xml.Schema;
using System.Xml.XPath;

namespace Be.Stateless.BizTalk.Unit.Xml
{
	/// <summary>
	/// Returns detailed information related to the <see cref="ValuednessValidationCallback" />.
	/// </summary>
	public class ValuednessValidationCallbackArgs
	{
		internal ValuednessValidationCallbackArgs(XPathNavigator navigator, XmlSeverityType severity)
		{
			Navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
			Severity = severity;
		}

		/// <summary>
		/// <see cref="XPathNavigator"/> denoting the node which is not valued.
		/// </summary>
		public XPathNavigator Navigator { get; }

		/// <summary>
		/// Gets the severity of the validation event.
		/// </summary>
		/// <returns>
		/// An <see cref="XmlSeverityType" /> value representing the severity of the validation event.
		/// </returns>
		public XmlSeverityType Severity { get; set; }
	}
}
