using System;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;

namespace Asap2
{
    [Serializable()]
    public class ParserErrorException : System.Exception
    {
        public ParserErrorException() : base() { }
        public ParserErrorException(string message) : base(message) { }
        public ParserErrorException(string message, System.Exception inner) : base(message, inner) { }
        public ParserErrorException(string format, params object[] args) : base(string.Format(format, args)) { }

        public override string ToString()
        {
            return base.Message;
        }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected ParserErrorException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }


    public class Parser
    {
        private IErrorReporter errorHandler;
        public Parser(string fileName, IErrorReporter errorHandler)
        {
            this.fileName = fileName;
            this.errorHandler = errorHandler;
        }

        public string fileName { get; private set; }

        /// <summary>
        /// Parse the provided A2L file.
        /// </summary>
        /// <returns>true if all succeded with no fatal errors</returns>
        public Asap2File DoParse()
        {
            bool status = false;
            Asap2Scanner scanner;
            Asap2Parser parser;
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                scanner = new Asap2Scanner(stream, this.errorHandler);
                parser = new Asap2Parser(scanner, this.errorHandler);
                try
                {
                    status = parser.Parse();
                }
                catch(ParserErrorException e)
                {
                    errorHandler.reportError(e.Message);
                    status = false;
                }
            }

            if (status)
            {
                return parser.Asap2File;
            }
            else
            {
                return null;
            }
        }

        public static class SortedFieldsCache
        {
            private static Dictionary<Type, FieldInfo[]> Value;
            static SortedFieldsCache()
            {
                Value = new Dictionary<Type, FieldInfo[]>();
            }

            public static FieldInfo[] Get(Type x)
            {

                FieldInfo[] v;
                if (Value.TryGetValue(x, out v))
                    return v;

                v = x.GetFields().OrderBy(f =>
                {
                    var data = AttributeCache<ElementAttribute, MemberInfo >.Get(f);
                    if (data == null)
                        return (uint)999999; /* sort it last */
                    else
                        return data.SortOrder;
                }).ToArray();
                Value.Add(x, v);
                return v;
            }
        }

        public static class AttributeCache<T, L>
            where T : class
            where L : MemberInfo
        {
            public static Dictionary<L, T> Value;
            static AttributeCache()
            {
                Value = new Dictionary<L, T>();
            }

            public static T Get(L x)
            {
                T v;
                if (Value.TryGetValue(x, out v))
                    return v;

                v = Attribute.GetCustomAttribute(x, typeof(T)) as T;
                Value.Add(x, v);
                return v;
            }
        }

        public bool Serialise(Asap2File tree, MemoryStream outStream)
        {
            StreamWriter stream = new StreamWriter(outStream, new UTF8Encoding(true));

            tree.elements.Sort((x, y) => x.OrderID.CompareTo(y.OrderID));
            foreach (var item in tree.elements)
            {
                if (item.GetType() == typeof(FileComment))
                {
                    stream.Write(item.ToString());
                    stream.Write(Environment.NewLine);
                }
                else
                {
                    foreach (SerialisedData data in SerialiseNode(item, 0))
                    {
                        WriteToStream(data, stream);
                    }
                }
            }
            stream.Flush();
            return false;
        }

        private void WriteToStream(SerialisedData node, StreamWriter stream, string indentType = "    ")
        {
            Indent(stream, node.indentLevel, indentType);
            stream.WriteAsync(node.data);
        }

        private void Indent(StreamWriter stream, ulong level, string indentType, ulong extraLevel = 0)
        {
            for (ulong i = 0; i < level + extraLevel; i++)
            {
                stream.WriteAsync(indentType);
            }
        }

        private IEnumerable<SerialisedData> SerialiseNode(Object tree, ulong indentLevel, ElementAttribute nodeElemAtt = null)
        {
            BaseAttribute baseAtt = AttributeCache<BaseAttribute, MemberInfo>.Get(tree.GetType());

            string elementName = null;
            if (baseAtt != null)
            {
                var fI = SortedFieldsCache.Get(tree.GetType());

                for (int i = 0; i < fI.Length; i++)
                {
                    ElementAttribute elemAtt = AttributeCache<ElementAttribute, MemberInfo>.Get(fI[i]);
                    if (elemAtt != null)
                    {
                        if (elemAtt.IsName)
                        {
                            elementName = (string)fI[i].GetValue(tree);
                        }
                    }
                }

                {
                    if (elementName == null)
                    {
                        elementName = tree.GetType().Name.ToUpper();
                    }

                    if (baseAtt.IsSimple)
                    {
                        yield return new SerialisedData(indentLevel, elementName);
                    }
                    else
                    {
                        yield return new SerialisedData(indentLevel, "/begin " + elementName);
                    }
                }


                if (fI.Length > 0)
                {
                    foreach (SerialisedData resultData in SerialiseElement(tree, fI, indentLevel + 1))
                    {
                        yield return resultData;
                    }
                }

                if (!baseAtt.IsSimple)
                {
                    yield return new SerialisedData(indentLevel, "/end " + elementName);
                }
                yield return new SerialisedData(indentLevel, Environment.NewLine);
            }
            else if (nodeElemAtt != null)
            {
                // Pure data node
                string data = "";
                if (nodeElemAtt.ForceNewLine)
                {
                    data = Environment.NewLine + data;
                }

                if (nodeElemAtt.IsString)
                {
                    String value = tree.ToString();
                    value = Regex.Replace(value, "\r", @"\r");
                    value = Regex.Replace(value, "\n", @"\n");
                    value = Regex.Replace(value, "\t", @"\t");
                    value = "\"" + value + "\"";
                    data += value;
                    yield return new SerialisedData(indentLevel, data);
                }
                else
                {
                    if (tree.GetType().IsEnum)
                    {
                        String value = Enum.GetName(tree.GetType(), tree);
                        data += value;
                        yield return new SerialisedData(indentLevel, data);
                    }
                    else if (nodeElemAtt.CodeAsHex)
                    {
                        UInt64 tmp = (UInt64)tree;
                        String value = "0x" + tmp.ToString("X");
                        data += value;
                        yield return new SerialisedData(indentLevel, data);
                    }
                    else if (tree.GetType() == typeof(decimal))
                    {
                        decimal value = (decimal)tree;
                        data += value.ToString(CultureInfo.InvariantCulture);
                        yield return new SerialisedData(indentLevel, data);
                    }
                    else
                    {
                        data += tree.ToString();
                        yield return new SerialisedData(indentLevel, data);
                    }
                }
            }
        }

        private IEnumerable<SerialisedData> SerialiseElement(Object tree, FieldInfo[] fI, ulong indentLevel)
        {
            for (int i = 0; i < fI.Length; i++)
            {
                ElementAttribute att = AttributeCache<ElementAttribute, MemberInfo>.Get(fI[i]);

                if (att != null)
                {
                    if (fI[i].GetValue(tree) != null)
                    {
                        if (att.IsComment)
                        {
                            string data = Environment.NewLine + "/*" + fI[i].GetValue(tree).ToString() + "*/" + Environment.NewLine;
                            if (att.ForceNewLine)
                            {
                                data = Environment.NewLine + data;
                            }
                            yield return new SerialisedData(indentLevel, data);
                        }
                        else if ((att.IsArgument || att.IsString) && !att.IsList)
                        {
                            string data = "";
                            if (att.ForceNewLine)
                            {
                                data += Environment.NewLine;
                            }

                            if (att.Name != null && att.Name != "")
                            {
                                data += att.Name + " ";
                            }

                            if (att.IsString)
                            {
                                String value = fI[i].GetValue(tree).ToString();
                                value = Regex.Replace(value, "\r", @"\r");
                                value = Regex.Replace(value, "\n", @"\n");
                                value = Regex.Replace(value, "\t", @"\t");
                                value = "\"" + value + "\"";
                                data += value;
                                yield return new SerialisedData(indentLevel, data);
                            }
                            else
                            {
                                if (fI[i].FieldType.IsEnum)
                                {
                                    String value = Enum.GetName(fI[i].FieldType, fI[i].GetValue(tree));
                                    data = value;
                                    yield return new SerialisedData(indentLevel, data);
                                }
                                else if (att.CodeAsHex)
                                {
                                    UInt64 tmp = (UInt64)fI[i].GetValue(tree);
                                    String value = "0x" + tmp.ToString("X");
                                    data += value;
                                    yield return new SerialisedData(indentLevel, data);
                                }
                                else if (fI[i].FieldType == typeof(decimal))
                                {
                                    decimal tmp = (decimal)fI[i].GetValue(tree);
                                    data += tmp.ToString(CultureInfo.InvariantCulture);
                                    yield return new SerialisedData(indentLevel, data);
                                }
                                else
                                {
                                    yield return new SerialisedData(indentLevel, data + fI[i].GetValue(tree).ToString());
                                }
                            }
                        }
                        else if (att.IsDictionary)
                        {
                            object obj = fI[i].GetValue(tree);
                            Dictionary<string, object> dict = ToDict<string, object>(obj);

                            if (dict.Count > 0)
                            {
                                if (att.Comment != null)
                                {
                                    yield return new SerialisedData(indentLevel, Environment.NewLine + "/*" + att.Comment + "*/" + Environment.NewLine);
                                }
                                else if (att.ForceNewLine)
                                {
                                    yield return new SerialisedData(indentLevel, Environment.NewLine);
                                }

                                foreach (object elem in dict.Values)
                                {
                                    foreach (SerialisedData dicElement in SerialiseNode(elem, indentLevel))
                                    {
                                        yield return dicElement;
                                    }
                                }
                            }
                        }
                        else if (att.IsList)
                        {
                            object obj = fI[i].GetValue(tree);
                            if (obj is IList)
                            {
                                var list = ((IList) obj);
                                if (list.Count > 0)
                                {
                                    if (att.Comment != null)
                                    {
                                        yield return new SerialisedData(indentLevel, Environment.NewLine + "/*" + att.Comment + "*/" + Environment.NewLine);
                                    }
                                    else if (att.ForceNewLine)
                                    {
                                        yield return new SerialisedData(indentLevel, Environment.NewLine);
                                    }

                                    foreach (var item in list)
                                    {
                                        foreach (SerialisedData listElement in SerialiseNode(item, indentLevel))
                                        {
                                            yield return listElement;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (fI[i].GetValue(tree) != null)
                            {
                                foreach (SerialisedData dataNode in SerialiseNode(fI[i].GetValue(tree), indentLevel))
                                {
                                    yield return dataNode;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static Dictionary<TKey, TValue> ToDict<TKey, TValue>(object obj)
        {
            var stringDictionary = obj as Dictionary<TKey, TValue>;

            if (stringDictionary != null)
            {
                return stringDictionary;
            }
            var baseDictionary = obj as IDictionary;

            if (baseDictionary != null)
            {
                var dictionary = new Dictionary<TKey, TValue>();
                foreach (DictionaryEntry keyValue in baseDictionary)
                {
                    if (!(keyValue.Value is TValue))
                    {
                        // value is not TKey. perhaps throw an exception
                        return null;
                    }
                    if (!(keyValue.Key is TKey))
                    {
                        // value is not TValue. perhaps throw an exception
                        return null;
                    }

                    dictionary.Add((TKey)keyValue.Key, (TValue)keyValue.Value);
                }
                return dictionary;
            }
            // object is not a dictionary. perhaps throw an exception
            return null;

        }

        private class SerialisedData
        {
            public ulong indentLevel;
            public string data;

            public SerialisedData(ulong indentLevel, string data)
            {
                this.indentLevel  = indentLevel;
                this.data         = data;
            }
        }
    }
}
