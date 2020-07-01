using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;

namespace BUILDER
{
    public class HtmlElement
    {
        public string _name, _text;
        public List<HtmlElement> Elements = new List<HtmlElement>();
        private const int indentSize = 2;

        public HtmlElement()
        {
        }

        public HtmlElement(string name, string text)
        {
            _name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            _text = text ?? throw new ArgumentNullException(paramName: nameof(name));
        }

        private string ToStringImpl(int indent)
        {
            var sb = new StringBuilder();
            var i = new string(' ', indentSize * indent);
            sb.AppendLine($"{i}<{_name}>");

            if (!string.IsNullOrWhiteSpace(_text))
            {
                // Append returns a stringBuilder so we can chain this method
                sb.Append(new string(' ', indentSize * (indent + 1)));
                sb.AppendLine(_text);
            }

            foreach (var htmlElement in Elements)
            {
                sb.Append(htmlElement.ToStringImpl(indent + 1));
            }
            sb.AppendLine($"{i}</{_name}>");

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringImpl(0);
        }
    }

    public class HtmlBuilder
    {
        private readonly string _rootName;
        HtmlElement root = new HtmlElement();

        public HtmlBuilder(string rootName)
        {
            _rootName = rootName;
            root._name = rootName;
        }

        public HtmlBuilder AddChild(string childName, string childText)
        {
            var e = new HtmlElement(childName, childText);
            root.Elements.Add(e);

            return this;
        }

        public override string ToString()
        {
            return root.ToString();
        }

        public void Clear()
        {
            root = new HtmlElement { _name = _rootName };
        }
    }

    public static class BuilderPattern
    {
        public static void Builder()
        {
            var builder = new HtmlBuilder("ul");

            // Fluent interface, an interface that allows you to chain several calls by returning a reference to the object I am working with 
            builder.AddChild("li", "hello").AddChild("li", "world");
            WriteLine(builder.ToString());
        }
    }
}
