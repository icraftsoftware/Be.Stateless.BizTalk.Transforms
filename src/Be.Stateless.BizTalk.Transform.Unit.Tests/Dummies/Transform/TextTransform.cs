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

using System.Reflection;
using Be.Stateless.IO.Extensions;
using Be.Stateless.Resources;
using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.Dummies.Transform
{
	[SchemaReference("Microsoft.XLANGs.BaseTypes.Any", typeof(Any))]
	public class TextTransform : TransformBase
	{
		static TextTransform()
		{
			_xmlContent = ResourceManager.Load(Assembly.GetExecutingAssembly(), "Be.Stateless.BizTalk.Dummies.Transform.TextTransform.xslt", s => s.ReadToEnd());
		}

		#region Base Class Member Overrides

		public override string[] SourceSchemas => new[] { typeof(Any).FullName };

		public override string[] TargetSchemas => new[] { typeof(Any).FullName };

		public override string XmlContent => _xmlContent;

		public override string XsltArgumentListContent => @"<ExtensionObjects />";

		#endregion

		private static readonly string _xmlContent;
	}
}
