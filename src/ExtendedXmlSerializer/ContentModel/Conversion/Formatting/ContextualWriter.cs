// MIT License
//
// Copyright (c) 2016 Wojciech Nag�rski
//                    Michael DeMond
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using ExtendedXmlSerializer.ContentModel.Xml;
using ExtendedXmlSerializer.ContentModel.Xml.Namespacing;
using Attribute = ExtendedXmlSerializer.ContentModel.Xml.Attribute;

namespace ExtendedXmlSerializer.ContentModel.Conversion.Formatting
{
	sealed class ContextualWriter<T> : IWriter<T>
	{
		readonly Func<IXmlWriter, T, string> _formatter;
		readonly IIdentity _identity;

		public ContextualWriter(Func<IXmlWriter, T, string> formatter, IIdentity identity)
		{
			_formatter = formatter;
			_identity = identity;
		}

		public void Write(IXmlWriter writer, T instance)
		{
			var identifier = _identity.Identifier;
			var name = !string.IsNullOrEmpty(identifier) ? writer.Get(identifier) : null;
			var ns = !string.IsNullOrEmpty(name) ? new Namespace(name, identifier) : (Namespace?) null;
			var format = _formatter(writer, instance);
			var attribute = new Attribute(_identity.Name, format, ns);
			writer.Attribute(attribute);
		}
	}
}