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
    public class Parser
    {
        private IErrorReporter errorHandler;
        public Parser(string fileName, IErrorReporter errorHandler)
        {
            this.fileName = fileName;
            this.errorHandler = errorHandler;
        }

        public string fileName { get; private set; }

        public Asap2File DoParse()
        {
            bool status = false;
            Asap2Scanner scanner;
            Asap2Parser parser;
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                scanner = new Asap2Scanner(stream, this.errorHandler, this.fileName);
                parser = new Asap2Parser(scanner);
                status = parser.Parse();
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
        
        private class compareFieldInfo : IComparer
        {
            int IComparer.Compare(Object x, Object y)
            {
                FieldInfo fiX = x as FieldInfo;
                FieldInfo fiY = y as FieldInfo;
                ElementAttribute elemAttX = (ElementAttribute)Attribute.GetCustomAttribute(fiX, typeof(ElementAttribute));
                ElementAttribute elemAttY = (ElementAttribute)Attribute.GetCustomAttribute(fiX, typeof(ElementAttribute));
                if (elemAttX == null)
                {
                    return -1;
                }
                else if (elemAttY == null)
                {
                    return 1;
                }

                if (elemAttX.SortOrder > elemAttY.SortOrder)
                {
                    return -1;
                }
                else if (elemAttX.SortOrder < elemAttY.SortOrder)
                {
                    return 1;
                }

                return 0;
            }

        }

        public bool Serialise(Asap2File tree, MemoryStream outStream)
        {
            List<SerialisedData> resultTree = new List<SerialisedData>();

            tree.elements.Sort((x, y) => x.OrderID.CompareTo(y.OrderID));
            foreach (var item in tree.elements)
            {
                if (item.GetType() == typeof(FileComment))
                {
                    SerialisedData data = new SerialisedData();
                    data.name = item.ToString();
                    data.isNode = true;
                    data.isSimple = true;
                    resultTree.Add(data);
                }
                else
                {
                    SerialisedData data = SerialiseNode(item, 0);
                    if (data != null)
                    {
                        resultTree.Add(data);
                    }
                }
            }
            StreamWriter stream = new StreamWriter(outStream);
            WriteToStream(resultTree, stream);
            stream.Flush();
            return false;
        }

        private void WriteToStream(List<SerialisedData> tree, StreamWriter stream, string indentType = "    ")
        {
            tree.Sort();
            foreach(SerialisedData node in tree)
            {
                if (node.ForceNewLine)
                {
                    stream.WriteAsync(newLine);
                }

                if (node.comment != "")
                {
                    if (!node.ForceNewLine)
                    {
                        stream.WriteAsync(newLine);
                    }
                    Indent(stream, node.indentLevel, indentType);
                    stream.WriteAsync("/*");
                    stream.WriteAsync(node.comment);
                    stream.WriteAsync("*/");
                    if (node.data != "" && node.name != "")
                    {
                        stream.WriteAsync(newLine);
                    }
                    else
                    {
                        stream.WriteAsync(" ");
                    }
                }

                if (node.name != "")
                {
                    if (!node.ForceNewLine)
                    {
                        stream.WriteAsync(newLine);
                    }
                    Indent(stream, node.indentLevel, indentType);
                    if (node.isSimple)
                    {
                        stream.WriteAsync(node.name);
                    }
                    else if (node.isNode)
                    {
                        stream.WriteAsync("/begin " + node.name);
                    }
                }

                if (node.data != "")
                {
                    if (node.comment == "" && !node.ForceNewLine)
                    {
                        stream.WriteAsync(" ");
                    }
                    else if (node.comment == "" && node.ForceNewLine && node.name == "")
                    {
                        Indent(stream, node.indentLevel, indentType);
                    }
                    stream.WriteAsync(node.data);
                }

                if (node.subData != null)
                {
                    WriteToStream(node.subData, stream);
                }

                if (!node.isSimple && node.name != "" && node.isNode)
                {
                    stream.WriteAsync(newLine);
                    Indent(stream, node.indentLevel, indentType);
                    stream.WriteAsync("/end " + node.name);
                }
            }
        }

        private void Indent(StreamWriter stream, ulong level, string indentType, ulong extraLevel = 0)
        {
            for (ulong i = 0; i < level + extraLevel; i++)
            {
                stream.WriteAsync(indentType);
            }
        }

        private class IndentH
        {
            public IndentH(string indentType = "    ", int level = 0)
            {
                this.indentType = indentType;
                this.level = level;
            }

            public IndentH(IndentH iH)
            {
                this.indentType = iH.indentType;
                this.level = iH.level + 1;
            }

            public string indentType { private set; get; }
            public int level { private set; get; }
            public void Indent(StreamWriter stream, int extraLevel = 0)
            {
                for (int i = 0; i < level + extraLevel; i++)
                {
                    stream.WriteAsync(indentType);
                }
            }
        }

        private string newLine = "\r\n";
        private SerialisedData SerialiseNode(Object tree, ulong indentLevel)
        {
            SerialisedData resultTree = null;
            string elementName = null;
            BaseAttribute baseAtt = (BaseAttribute)Attribute.GetCustomAttribute(tree.GetType(), typeof(BaseAttribute));

            if (baseAtt != null)
            {
                FieldInfo[] fI = tree.GetType().GetFields();
                IComparer myComparer = new compareFieldInfo();
                Array.Sort(fI, myComparer);

                resultTree = new SerialisedData();
                resultTree.isSimple = baseAtt.IsSimple;
                resultTree.indentLevel = indentLevel;
                for (int i = 0; i < fI.Length; i++)
                {
                    ElementAttribute elemAtt = (ElementAttribute)Attribute.GetCustomAttribute(fI[i], typeof(ElementAttribute));
                    if (elemAtt != null)
                    {
                        if (elemAtt.IsName)
                        {
                            elementName = (string)fI[i].GetValue(tree);
                        }
                    }
                }

                if (elementName != null)
                {
                    resultTree.name = elementName;
                }
                else
                {
                    resultTree.name = tree.GetType().Name.ToUpper();
                }

                resultTree.isNode = true;
                if (!baseAtt.IsSimple)
                {
                    if (elementName != null)
                    {
                        resultTree.name = elementName;
                    }
                    else
                    {
                        resultTree.name = tree.GetType().Name.ToUpper();
                    }
                }

                if (fI.Length > 0)
                {
                    List<SerialisedData> resultData = SerialiseElement(tree, fI, indentLevel + 1);
                    if (resultData.Count > 0)
                    {
                        resultTree.subData = resultData;
                    }
                }
            }
            return resultTree;
        }

        private List<SerialisedData> SerialiseElement(Object tree, FieldInfo[] fI, ulong indentLevel)
        {
            List<SerialisedData> resultData = new List<SerialisedData>();
            for (int i = 0; i < fI.Length; i++)
            {
                ElementAttribute att = (ElementAttribute)Attribute.GetCustomAttribute(fI[i], typeof(ElementAttribute));

                if (att != null)
                {
                    if (fI[i].GetValue(tree) != null)
                    {
                        if (att.IsComment)
                        {
                            if (!att.IsPreComment && fI[i].GetValue(tree) != null)
                            {
                                SerialisedData resultElement = new SerialisedData();
                                resultElement.ForceNewLine = att.ForceNewLine;
                                resultElement.sortOrder = att.SortOrder;
                                resultElement.indentLevel = indentLevel;
                                resultElement.comment = fI[i].GetValue(tree).ToString();
                                resultData.Add(resultElement);
                            }
                        }
                        else if ((att.IsArgument || att.IsString) && !att.IsList)
                        {
                            SerialisedData resultElement = new SerialisedData();
                            resultElement.ForceNewLine = att.ForceNewLine;
                            resultElement.sortOrder = att.SortOrder;
                            resultElement.indentLevel = indentLevel;
                            if (att.Comment != null)
                            {
                                resultElement.comment = att.Comment;
                            }

                            if (att.Name != null && att.Name != "")
                            {
                                resultElement.name = att.Name;
                                resultElement.isSimple = true;
                            }

                            if (att.IsString)
                            {
                                String value = fI[i].GetValue(tree).ToString();
                                value = Regex.Replace(value, "\r", @"\r");
                                value = Regex.Replace(value, "\n", @"\n");
                                value = Regex.Replace(value, "\t", @"\t");
                                value = "\"" + value + "\"";
                                resultElement.data = value;
                            }
                            else
                            {
                                if (fI[i].FieldType.IsEnum)
                                {
                                    String value = Enum.GetName(fI[i].FieldType, fI[i].GetValue(tree));
                                    resultElement.data = value;
                                }
                                else if (att.CodeAsHex)
                                {
                                    UInt64 data = (UInt64)fI[i].GetValue(tree);
                                    String value = "0x" + data.ToString("X");
                                    resultElement.data = value;
                                }
                                else if (fI[i].FieldType == typeof(decimal))
                                {
                                    decimal data = (decimal)fI[i].GetValue(tree);
                                    resultElement.data = data.ToString(CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    resultElement.data = fI[i].GetValue(tree).ToString();
                                }
                            }
                            resultData.Add(resultElement);
                        }
                        else if (att.IsDictionary)
                        {
                            object obj = fI[i].GetValue(tree);
                            Dictionary<string, object> dict = ToDict<string, object>(obj);

                            if (dict.Count > 0)
                            {
                                SerialisedData resultElement = new SerialisedData();
                                resultElement.ForceNewLine = att.ForceNewLine;
                                resultElement.sortOrder = att.SortOrder;
                                resultElement.indentLevel = indentLevel;
                                if (att.Comment != null)
                                {
                                    resultElement.comment = att.Comment;
                                }

                                foreach (object elem in dict.Values)
                                {
                                    SerialisedData dicElement = SerialiseNode(elem, indentLevel);
                                    if (dicElement != null)
                                    {
                                        if (dicElement.subData != null)
                                        {
                                            dicElement.sortOrder = dicElement.subData.Last().sortOrder + 1;
                                        }
                                        else
                                        {
                                            dicElement.sortOrder = 0;
                                        }
                                        resultElement.addSubData(dicElement);
                                    }
                                }
                                resultData.Add(resultElement);
                            }
                        }
                        else if (att.IsList)
                        {
                            object obj = fI[i].GetValue(tree);
                            if (obj is IList)
                            {
                                var list = ((IList) obj);
                                SerialisedData resultElement = new SerialisedData();
                                resultElement.ForceNewLine = att.ForceNewLine;
                                resultElement.sortOrder = att.SortOrder;
                                resultElement.indentLevel = indentLevel;
                                if (att.Comment != null)
                                {
                                    resultElement.comment = att.Comment;
                                }
                                
                                if (list.Count > 0)
                                {
                                    foreach(var item in list)
                                    {
                                        SerialisedData dicElement = SerialiseNode(item, indentLevel);
                                        if (dicElement == null)
                                        {
                                            // Pure data element in the list. Add them to a new SerialisedData.
                                            dicElement = new SerialisedData();
                                            dicElement.indentLevel = indentLevel;
                                            dicElement.ForceNewLine = true;

                                            if (att.IsString)
                                            {
                                                String value = item.ToString();
                                                value = Regex.Replace(value, "\r", @"\r");
                                                value = Regex.Replace(value, "\n", @"\n");
                                                value = Regex.Replace(value, "\t", @"\t");
                                                value = "\"" + value + "\"";
                                                dicElement.data = value;
                                            }
                                            else
                                            {
                                                if (item.GetType().IsEnum)
                                                {
                                                    String value = Enum.GetName(item.GetType(), item);
                                                    dicElement.data = value;
                                                }
                                                else if (att.CodeAsHex)
                                                {
                                                    UInt64 data = (UInt64)item;
                                                    String value = "0x" + data.ToString("X");
                                                    dicElement.data = value;
                                                }
                                                else if (item.GetType() == typeof(decimal))
                                                {
                                                    decimal data = (decimal)item;
                                                    dicElement.data = data.ToString(CultureInfo.InvariantCulture);
                                                }
                                                else
                                                {
                                                    dicElement.data = item.ToString();
                                                }
                                            }
                                        }

                                        if (dicElement.subData != null)
                                        {
                                            dicElement.sortOrder = dicElement.subData.Last().sortOrder + 1;
                                        }
                                        else
                                        {
                                            dicElement.sortOrder = 0;
                                        }
                                        resultElement.addSubData(dicElement);
                                    }
                                }
                                resultData.Add(resultElement);
                            }
                        }
                        else
                        {
                            if (fI[i].GetValue(tree) != null)
                            {
                                SerialisedData resultElement = SerialiseNode(fI[i].GetValue(tree), indentLevel);
                                if (resultElement != null)
                                {
                                    resultElement.sortOrder = att.SortOrder;
                                    resultData.Add(resultElement);
                                }
                            }
                        }
                    }
                }
            }
            return resultData;
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

        private class SerialisedData : IComparable
        {
            public ulong indentLevel;
            public ulong sortOrder;
            public bool isSimple;
            public bool ForceNewLine;
            public bool isNode;
            public string comment;
            public string name;
            public string data;
            public List<SerialisedData> subData;

            public SerialisedData()
            {
                this.indentLevel  = 0;
                this.sortOrder    = 0;
                this.comment      = "";
                this.name         = "";
                this.data         = "";
                this.isNode       = false;
                this.isSimple     = false;
                this.ForceNewLine = false;
            }

            public void addSubData(SerialisedData data)
            {
                if (subData == null)
                {
                    subData = new List<SerialisedData>();
                }
                subData.Add(data);
            }

            public int CompareTo(object obj)
            {
                if (obj == null)
                {
                    return 1;
                }

                SerialisedData data = obj as SerialisedData;
                if (data != null)
                {
                    return this.sortOrder.CompareTo(data.sortOrder);
                }
                else
                {
                    throw new ArgumentException("Object is not of type SerialisedData");
                }
            }
        }
    }
}
