﻿#region Copyright & License

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
using System.Diagnostics.CodeAnalysis;
using Be.Stateless.BizTalk.Xml;

namespace Be.Stateless.BizTalk.Unit.Transform
{
	internal class DebuggerSupportingEmbeddedXmlResolver : XslMapUrlResolver
	{
		public DebuggerSupportingEmbeddedXmlResolver(Type transform) : base(transform) { }

		#region Base Class Member Overrides

		[SuppressMessage("ReSharper", "InvertIf")]
		[SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
		public override Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			var uri = new Uri(relativeUri, UriKind.RelativeOrAbsolute);
			if (uri.Scheme == MAP_SCHEME)
			{
				if (uri.Host == TYPE_HOST)
				{
					var typeName = Uri.UnescapeDataString(uri.Segments[1]);
					var type = Type.GetType(typeName, true);
					if (type.TryResolveCustomXsltPath(out var sourceXsltFilePath)) relativeUri = sourceXsltFilePath;
				}
				else if (uri.Host == RESOURCE_HOST)
				{
					var resourceName = uri.Segments[1];
					if (ReferenceType.TryResolveEmbeddedXsltResourceSourceFilePath(resourceName, out var sourceXsltFilePath)) relativeUri = sourceXsltFilePath;
				}
			}
			return base.ResolveUri(baseUri, relativeUri);
		}

		#endregion
	}
}
