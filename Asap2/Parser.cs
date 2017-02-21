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
            this.indentType = "    ";
        }

        public string fileName { get; private set; }
        public string indentType { get; set; }

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

        private static class SortedFieldsCache
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

        private static class SortedPropertyCache
        {
            private static Dictionary<Type, PropertyInfo[]> Value;
            static SortedPropertyCache()
            {
                Value = new Dictionary<Type, PropertyInfo[]>();
            }

            public static PropertyInfo[] Get(Type x)
            {

                PropertyInfo[] v;
                if (Value.TryGetValue(x, out v))
                    return v;

                v = x.GetProperties().OrderBy(f =>
                {
                    var data = AttributeCache<ElementAttribute, MemberInfo>.Get(f);
                    if (data == null)
                        return (uint)999999; /* sort it last */
                    else
                        return data.SortOrder;
                }).ToArray();
                Value.Add(x, v);
                return v;
            }
        }

        private static class AttributeCache<T, L>
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

        public bool Serialise(Asap2File tree, StreamWriter stream)
        {
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
                    foreach (string data in SerialiseNode(item, 0))
                    {
                        stream.WriteAsync(data);
                    }
                }
            }
            stream.Flush();
            return false;
        }

        private StringBuilder Indent(uint level)
        {
            StringBuilder tmp = new StringBuilder((int)(indentType.Length * level));
            for (ulong i = 0; i < level; i++)
            {
                tmp.Append(indentType);
            }
            return tmp;
        }

        private IEnumerable<string> SerialiseNode(Object tree, uint indentLevel, ElementAttribute nodeElemAtt = null)
        {
            BaseAttribute baseAtt = AttributeCache<BaseAttribute, MemberInfo>.Get(tree.GetType());

            string elementName = null;
            if (baseAtt != null)
            {
                var pI = SortedPropertyCache.Get(tree.GetType());
                var fI = SortedFieldsCache.Get(tree.GetType());

                for (int i = 0; i < pI.Length; i++)
                {
                    ElementAttribute elemAtt = AttributeCache<ElementAttribute, MemberInfo>.Get(pI[i]);
                    if (elemAtt != null)
                    {
                        if (elemAtt.IsName)
                        {
                            elementName = (string)pI[i].GetValue(tree);
                        }
                    }
                }

                if (elementName == null)
                {
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
                }
                {
                    yield return Environment.NewLine;
                    if (elementName == null)
                    {
                        elementName = tree.GetType().Name.ToUpper();
                    }

                    if (baseAtt.IsSimple)
                    {
                        yield return Indent(indentLevel).Append(elementName).ToString();
                    }
                    else
                    {
                        yield return Indent(indentLevel).Append("/begin " + elementName).ToString();
                    }
                }


                if (fI.Length > 0)
                {
                    foreach (string resultData in SerialiseElement(tree, fI, pI, indentLevel + 1))
                    {
                        yield return resultData;
                    }
                }

                if (!baseAtt.IsSimple)
                {
                    yield return Environment.NewLine;
                    yield return Indent(indentLevel).Append("/end " + elementName).ToString();
                }
            }
            else if (nodeElemAtt != null)
            {
                // Pure data node
                string data = data = Environment.NewLine + Indent(indentLevel).ToString();

                if (nodeElemAtt.IsString)
                {
                    String value = tree.ToString();
                    data += "\"" + value + "\"";
                    yield return Indent(indentLevel).Append(data).ToString();
                }
                else
                {
                    if (tree.GetType().IsEnum)
                    {
                        String value = Enum.GetName(tree.GetType(), tree);
                        data += value;
                        yield return data;
                    }
                    else if (nodeElemAtt.CodeAsHex)
                    {
                        UInt64 tmp = (UInt64)tree;
                        String value = "0x" + tmp.ToString("X");
                        data += value;
                        yield return data;
                    }
                    else if (tree.GetType() == typeof(decimal))
                    {
                        decimal value = (decimal)tree;
                        data += value.ToString(CultureInfo.InvariantCulture);
                        yield return data;
                    }
                    else
                    {
                        data += tree.ToString();
                        yield return data;
                    }
                }
            }
        }

        private IEnumerable<string> SerialiseElement(Object tree, FieldInfo[] fI, PropertyInfo[] pI, uint indentLevel)
        {
            foreach (var info in pI)
            {
                ElementAttribute att = AttributeCache<ElementAttribute, MemberInfo>.Get(info);
                if (att != null)
                {
                    object objData = info.GetValue(tree);
                    if (objData != null)
                    {
                        foreach (var serialiseAttribute in SerialiseAttributeData(objData, info.PropertyType, att, indentLevel))
                        {
                            yield return serialiseAttribute;
                        }
                    }
                }
            }
            foreach (var info in fI)
            {
                ElementAttribute att = AttributeCache<ElementAttribute, MemberInfo>.Get(info);
                if (att != null)
                {
                    object objData = info.GetValue(tree);
                    if (objData != null)
                    {
                        foreach (var serialiseAttribute in SerialiseAttributeData(objData, info.FieldType, att, indentLevel))
                        {
                            yield return serialiseAttribute;
                        }
                    }
                }
            }
        }
        private IEnumerable<string> SerialiseAttributeData(Object objData, Type objType, ElementAttribute att, uint indentLevel)
        {
            if (att.IsComment)
            {
                yield return Environment.NewLine;
                StringBuilder tmp = Indent(indentLevel);
                tmp.Append("/*");
                tmp.Append(objData.ToString());
                tmp.Append("*/");
                tmp.Append(Environment.NewLine);
                yield return tmp.ToString();
            }
            else if ((att.IsArgument || att.IsString) && !att.IsList)
            {
                string data = "";
                if (att.Comment != null)
                {
                    yield return Environment.NewLine;
                    StringBuilder tmp = Indent(indentLevel);
                    tmp.Append("/*");
                    tmp.Append(att.Comment);
                    tmp.Append("*/ ");
                    yield return tmp.ToString();
                }
                if (att.Name != null && att.Name != "")
                {
                    data += Environment.NewLine;
                    data += Indent(indentLevel).Append(att.Name).Append(" ").ToString();
                }
                else if (att.ForceNewLine)
                {
                    data += Environment.NewLine;
                    data += Indent(indentLevel).ToString();
                }
                else if (att.Comment == null)
                {
                    data = " ";
                }

                if (att.IsString)
                {
                    String value = objData.ToString();
                    data += "\"" + value + "\"";
                    yield return data;
                }
                else
                {
                    if (objType.IsEnum)
                    {
                        String value = Enum.GetName(objType, objData);
                        data += value;
                        yield return data;
                    }
                    else if (att.CodeAsHex)
                    {
                        UInt64 tmp = (UInt64)objData;
                        String value = "0x" + tmp.ToString("X");
                        data += value;
                        yield return data;
                    }
                    else if (objType == typeof(decimal))
                    {
                        decimal tmp = (decimal)objData;
                        data += tmp.ToString(CultureInfo.InvariantCulture);
                        yield return data;
                    }
                    else
                    {
                        yield return data + objData.ToString();
                    }
                }
            }
            else if (att.IsDictionary)
            {
                Dictionary<string, object> dict = ToDict<string, object>(objData);

                if (dict.Count > 0)
                {
                    if (att.Comment != null)
                    {
                        yield return Environment.NewLine;
                        StringBuilder tmp = Indent(indentLevel);
                        tmp.Append("/*");
                        tmp.Append(att.Comment);
                        tmp.Append("*/");
                        tmp.Append(Environment.NewLine);
                        yield return tmp.ToString();
                    }
                    else if (att.ForceNewLine)
                    {
                        yield return Environment.NewLine;
                    }

                    foreach (object elem in dict.Values)
                    {
                        foreach (string dicElement in SerialiseNode(elem, indentLevel))
                        {
                            yield return dicElement;
                        }
                    }
                }
            }
            else if (att.IsList)
            {
                if (objData is IList)
                {
                    var list = ((IList)objData);
                    if (list.Count > 0)
                    {
                        if (att.Comment != null)
                        {
                            yield return Environment.NewLine;
                            StringBuilder tmp = Indent(indentLevel);
                            tmp.Append("/*");
                            tmp.Append(att.Comment);
                            tmp.Append("*/");
                            yield return tmp.ToString();
                        }
                        else if (att.ForceNewLine)
                        {
                            yield return Environment.NewLine;
                        }

                        if (list[0].GetType().BaseType == typeof(Asap2Base))
                        {
                            /* If the is list is List<Asap2Base> sort the list and then iterate over the sorted list. */
                            IEnumerable<Asap2Base> tmp =
                                from Asap2Base item in list
                                orderby item.OrderID
                                select item;

                            foreach (var item in tmp)
                            {
                                foreach (string listElement in SerialiseNode(item, indentLevel, att))
                                {
                                    yield return listElement;
                                }
                            }
                        }
                        else
                        {
                            /* Generic data elements. */
                            foreach (var item in list)
                            {
                                foreach (string listElement in SerialiseNode(item, indentLevel, att))
                                {
                                    yield return listElement;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (objData != null)
                {
                    foreach (string dataNode in SerialiseNode(objData, indentLevel))
                    {
                        yield return dataNode;
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

    }
}
