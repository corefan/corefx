// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using StringBuilder = System.Text.StringBuilder;

namespace System.Xml.Linq
{
    /// <summary>
    /// Represents a text node.
    /// </summary>
    public class XText : XNode
    {
        internal string text;

        /// <summary>
        /// Initializes a new instance of the XText class.
        /// </summary>
        /// <param name="value">The string that contains the value of the text node.</param>
        public XText(string value)
        {
            if (value == null) throw new ArgumentNullException("value");
            text = value;
        }

        /// <summary>
        /// Initializes a new instance of the XText class from another XText object.
        /// </summary>
        /// <param name="other">The text node to copy from.</param>
        public XText(XText other)
        {
            if (other == null) throw new ArgumentNullException("other");
            text = other.text;
        }

        internal XText(XmlReader r)
        {
            text = r.Value;
            r.Read();
        }

        /// <summary>
        /// Gets the node type for this node.
        /// </summary>
        /// <remarks>
        /// This property will always return XmlNodeType.Text.
        /// </remarks>
        public override XmlNodeType NodeType
        {
            get
            {
                return XmlNodeType.Text;
            }
        }

        /// <summary>
        /// Gets or sets the value of this node.
        /// </summary>
        public string Value
        {
            get
            {
                return text;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                bool notify = NotifyChanging(this, XObjectChangeEventArgs.Value);
                text = value;
                if (notify) NotifyChanged(this, XObjectChangeEventArgs.Value);
            }
        }

        /// <summary>
        /// Write this <see cref="XText"/> to the given <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="XmlWriter"/> to write this <see cref="XText"/> to.
        /// </param>
        public override void WriteTo(XmlWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            if (parent is XDocument)
            {
                writer.WriteWhitespace(text);
            }
            else
            {
                writer.WriteString(text);
            }
        }

        /// <summary>
        /// Write this <see cref="XText"/> to the given <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="XmlWriter"/> to write this <see cref="XText"/> to.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token.
        /// </param>
        public override Task WriteToAsync(XmlWriter writer, CancellationToken cancellationToken)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);

            return parent is XDocument ?
                writer.WriteWhitespaceAsync(text) :
                writer.WriteStringAsync(text);
        }

        internal override void AppendText(StringBuilder sb)
        {
            sb.Append(text);
        }

        internal override XNode CloneNode()
        {
            return new XText(this);
        }

        internal override bool DeepEquals(XNode node)
        {
            return node != null && NodeType == node.NodeType && text == ((XText)node).text;
        }

        internal override int GetDeepHashCode()
        {
            return text.GetHashCode();
        }
    }
}
