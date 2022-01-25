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

using System;
using Be.Stateless.BizTalk.Stream.Extensions;
using Be.Stateless.BizTalk.Xml.Xsl;
using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.Unit.Transform
{
	internal class DebuggerSupportingTransformer : Transformer
	{
		public DebuggerSupportingTransformer(System.IO.Stream[] streams, IMapCustomXsltPathResolver customXsltPathResolver) : base(streams)
		{
			_customXsltPathResolver = customXsltPathResolver;
		}

		#region Base Class Member Overrides

		/// <summary>
		/// Returns a <see cref="XslCompiledTransformDescriptor"/>-derived instance that enable XSLT debugging if the source XSLT
		/// file of the <see cref="TransformBase"/>-derived <paramref name="transform"/> is found. Returns a regular <see
		/// cref="XslCompiledTransformDescriptor"/> instance otherwise.
		/// </summary>
		/// <param name="transform">
		/// The <see cref="TransformBase"/>-derived type whose <see cref="XslCompiledTransformDescriptor"/> is looked up.
		/// </param>
		/// <returns>
		/// A <see cref="XslCompiledTransformDescriptor"/> that enable XSLT debugging if the source XSLT file has been found.
		/// </returns>
		protected override XslCompiledTransformDescriptor LookupTransformDescriptor(Type transform)
		{
			return _customXsltPathResolver.TryResolveXsltPath(out var sourceXsltFilePath)
				? new(new DebuggerSupportingXslCompiledTransformDescriptorBuilder(transform, sourceXsltFilePath))
				: base.LookupTransformDescriptor(transform);
		}

		#endregion

		private readonly IMapCustomXsltPathResolver _customXsltPathResolver;
	}
}
