using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using HtmlAgilityPack;
using System;
using System.Windows.Media;
using System.Windows;
using System.Globalization;

namespace Hyperwave.ViewModel
{
    public static class EveMarkupLanguage
    {
        static Regex mRegEx_Color = new Regex(@"^#(?<A>[0-9a-f]{2})(?<R>[0-9a-f]{2})(?<G>[0-9a-f]{2})(?<B>[0-9a-f]{2})$");
               
        public static FlowDocument ConvertToFlowDocument(string evml, out Hyperlink[] links, FontFamily font,Brush background,double base_font_size = 12)
        {
            HtmlDocument doc = new HtmlDocument();
            
            FlowDocument ret = new FlowDocument();
            List<Hyperlink> linklist = new List<Hyperlink>();

            ret.FontFamily = font;
            ret.Background = background;

            doc.LoadHtml("<html><body>"+ evml + "</body></html>");

            var body = doc.DocumentNode.SelectSingleNode("//body");

            Paragraph paragraph = new Paragraph();

            ret.Blocks.Add(paragraph);

            double fontsizemul = base_font_size / 12.0;

            StyleOptions base_style = new StyleOptions()
            {
                Color = Colors.White,
                FontWeight = FontWeights.Normal,
                FontStyle = FontStyles.Normal,
                TextDecoration = null,
                FontSize = 12,
                FontSizeMultiplier = fontsizemul
            };

            base_style.Apply(paragraph);

            AddFlowNode(body, doc, paragraph.Inlines, linklist, base_style);

            links = linklist.ToArray();

            return ret;

        }

        private static void AddFlowNode(HtmlNode node, HtmlDocument doc, InlineCollection items, List<Hyperlink> linklist, StyleOptions styles)
        {
            foreach (HtmlNode i in node.ChildNodes)
            {
                switch (i.NodeType)
                {
                    case HtmlNodeType.Text:
                        items.Add((Inline)styles.Apply(new Run(i.InnerHtml)));
                        continue;
                    case HtmlNodeType.Element:
                        break;
                    default:
                        continue;
                }

                switch (i.Name)
                {
                    case "font":
                        AddFlowNode(i, doc, items, linklist, new StyleOptions(styles)
                        {
                            Color = ParseColorValue(i.GetAttributeValue("color", ""), styles.Color),
                            FontSize = i.GetAttributeValue("size", 12)
                        });
                        break;
                    case "b":
                        AddFlowNode(i, doc, items, linklist, new StyleOptions(styles)
                        {
                            FontWeight = FontWeights.Bold
                        });
                        break;
                    case "u":
                        AddFlowNode(i, doc, items, linklist, new StyleOptions(styles)
                        {
                            TextDecoration = TextDecorations.Underline
                        });
                        break;
                    case "i":
                        AddFlowNode(i, doc, items, linklist, new StyleOptions(styles)
                        {
                            FontStyle = FontStyles.Italic
                        });
                        break;
                    case "br":
                        items.Add(new LineBreak());
                        break;
                    case "a":
                        {
                            var linkstyle = new StyleOptions(styles)
                            {
                                TextDecoration = TextDecorations.Underline
                            };

                            Hyperlink link = (Hyperlink)linkstyle.Apply(new Hyperlink());
                            link.NavigateUri = new Uri(i.GetAttributeValue("href", ""), UriKind.RelativeOrAbsolute);

                            items.Add(link);
                            linklist.Add(link);
                            AddFlowNode(i, doc, link.Inlines, linklist, linkstyle);
                            break;
                        }
                    default:
                        AddFlowNode(i, doc, items, linklist, styles);
                        break;
                }
            }
        }

        private static Color ParseColorValue(string colortext, Color defvalue)
        {
            Match match = mRegEx_Color.Match(colortext);

            if (!match.Success)
                return defvalue;

            Color color = Color.FromArgb(255,
                byte.Parse(match.Groups["R"].Value, NumberStyles.HexNumber),
                byte.Parse(match.Groups["G"].Value, NumberStyles.HexNumber),
                byte.Parse(match.Groups["B"].Value, NumberStyles.HexNumber));

            return color;
        }

        public static string ConvertFromFlowDocument(FlowDocument source, double base_font_size = 12)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml("<html><body></body></html>");

            var body = doc.DocumentNode.SelectSingleNode("//body");

            double fontsizemul = base_font_size / 12.0;

            var style = new StyleOptions()
            {
                Color = Colors.White,
                FontWeight = FontWeights.Normal,
                FontStyle = FontStyles.Normal,
                TextDecoration = null,
                FontSize = 12,
                FontSizeMultiplier = fontsizemul
            };

            foreach(Paragraph i in source.Blocks)
            {
                ConvertParagraph(i, doc, body, style);
            }

            return body.InnerHtml;
        }

        private static void ConvertParagraph(Paragraph paragraph, HtmlDocument doc, HtmlNode body, StyleOptions base_style)
        {
            StyleOptions current_style = new StyleOptions(base_style);
            HtmlNode working_node = CreateStyleTags(paragraph, doc,body,body,ref current_style);

            foreach (var i in paragraph.Inlines)
            {
                ConvertInline(i, doc, body, base_style,ref working_node,ref current_style);
            }
            body.AppendChild(doc.CreateElement("br"));
        }

        private static void ConvertInline(Inline inline, HtmlDocument doc, HtmlNode parent, StyleOptions base_style, ref HtmlNode working_node, ref StyleOptions current_style)
        {
            
            Hyperlink link = inline as Hyperlink;

            if (link != null)
            {
                working_node = CreateStyleTags(inline, doc, parent, parent, ref current_style,true);

                HtmlNode linkbase = working_node;

                if (link.NavigateUri.Scheme == "http" || link.NavigateUri.Scheme == "https")
                    linkbase = linkbase.AppendChild(doc.CreateElement("loc"));

                HtmlNode linktag = linkbase.AppendChild(doc.CreateElement("a"));
                linktag.SetAttributeValue("href", link.NavigateUri.ToString());
                StyleOptions link_current_style = current_style;
                var link_working_node = linktag;

                foreach (var i in link.Inlines)
                {
                    ConvertInline(i, doc, linktag, base_style,ref link_working_node , ref link_current_style);
                }
                return;
            }

            var run = inline as Run;
            if (run != null)
            {
                working_node = CreateStyleTags(inline, doc, parent, parent, ref current_style);

                working_node.InnerHtml += HtmlEntity.Entitize(run.Text).Replace("&nbsp;"," ");
                return;
            }

            if(inline is LineBreak)
            {
                parent.AppendChild(doc.CreateElement("br"));
                return;
            }

        }

        private static HtmlNode CreateStyleTags(TextElement text, HtmlDocument doc, HtmlNode parent, HtmlNode working_node, ref StyleOptions current_style,bool forlink = false)
        {
            StyleOptions new_style = new StyleOptions(text, current_style.FontSizeMultiplier);
            if (new_style == current_style)
                return working_node;

            working_node = parent;

            if (new_style.Color != Colors.White || new_style.FontSize != 12)
            {
                working_node = working_node.AppendChild(doc.CreateElement("font"));
                working_node.SetAttributeValue("size", new_style.FontSize.ToString());
                working_node.SetAttributeValue("color", FormatEveColor(new_style.Color));
            }

            if(new_style.FontWeight != FontWeights.Normal)
                working_node = working_node.AppendChild(doc.CreateElement("b"));

            if (new_style.FontStyle == FontStyles.Italic)
                working_node = working_node.AppendChild(doc.CreateElement("i"));

            if (!forlink && new_style.TextDecoration == TextDecorations.Underline)
                working_node = working_node.AppendChild(doc.CreateElement("u"));

            current_style = new_style;
            return working_node;
        }

        static IEnumerable<double> GetPredefinedSizes()
        {
            yield return 8.0;
            yield return 9.0;
            yield return 10.0;
            yield return 11.0;
            yield return 12.0;
            yield return 14.0;
            yield return 18.0;
            yield return 24.0;
            yield return 30.0;
        }

        static double GetNearestSize(double size)
        {
            double ret = 8.0;
            double nearest = double.MaxValue;
            foreach(double i in GetPredefinedSizes())
            {
                double dist = Math.Abs(i - size);
                if (dist < nearest)
                {
                    ret = i;
                    nearest = dist;
                }
            }

            return ret;
        }

        

        private static string FormatEveColor(SolidColorBrush brush)
        {
            return FormatEveColor(brush.Color);
        }

        private static string FormatEveColor(Color color)
        {
            return string.Format("#bf{0:x02}{1:x02}{2:x02}", color.R, color.G, color.B);
        }

        struct StyleOptions
        {
            public StyleOptions(StyleOptions copy)
                :this()
            {
                Color = copy.Color;
                FontSize = copy.FontSize;
                FontSizeMultiplier = copy.FontSizeMultiplier;
                FontWeight = copy.FontWeight;
                FontStyle = copy.FontStyle;
                TextDecoration = copy.TextDecoration;
            }

            public StyleOptions(TextElement element,double font_size_multiplier)
                : this()
            {
                SolidColorBrush brush = element.Foreground as SolidColorBrush;
                Color = brush.Color;
                FontSize = GetNearestSize(element.FontSize / font_size_multiplier);
                FontSizeMultiplier = font_size_multiplier;
                FontWeight = element.FontWeight;
                FontStyle = element.FontStyle;

                Inline inline = element as Inline;

                if(inline != null)
                    TextDecoration = inline.TextDecorations;
            }
            
            public Color Color;
            public double FontSize;
            public double FontSizeMultiplier;
            public FontWeight FontWeight;
            public FontStyle FontStyle;
            public TextDecorationCollection TextDecoration;

            public TextElement Apply(TextElement element)
            {
                element.FontSize = FontSize * FontSizeMultiplier;
                element.FontWeight = FontWeight;
                element.FontStyle = FontStyle;
                element.Foreground = new SolidColorBrush(Color);

                Inline inline = element as Inline;

                if (inline != null)
                    inline.TextDecorations = TextDecoration;

                return element;
            }

            public static bool operator ==(StyleOptions lhs, StyleOptions rhs)
            {
                return lhs.Equals(rhs);
            }

            public static bool operator !=(StyleOptions lhs, StyleOptions rhs)
            {
                return !lhs.Equals(rhs);
            }

            public override bool Equals(object obj)
            {
                if (StyleOptions.ReferenceEquals(obj, null) || !(obj is StyleOptions))
                    return false;
                StyleOptions cmp = (StyleOptions)obj;
                return 
                    cmp.Color == Color &&
                    cmp.FontSize == FontSize &&
                    cmp.FontWeight == FontWeight &&
                    cmp.FontStyle == FontStyle &&
                    Compare(cmp.TextDecoration,TextDecoration);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            static bool Compare(TextDecorationCollection cmp1, TextDecorationCollection cmp2)
            {
                if (cmp1 == null && cmp2 == null)
                    return true;
                else if (cmp1 == null || cmp2 == null)
                    return false;
                else if (cmp1.Count != cmp2.Count)
                    return false;

                foreach(var i in cmp1)
                {
                    if (!cmp2.Contains(i))
                        return false;
                }

                return true;
            }

        }
    }
}
