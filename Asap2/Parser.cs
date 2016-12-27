using System;
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
        public Parser(string fileName)
        {
            this.fileName = fileName;
        }

        public string fileName {get; private set;}

        public Asap2File DoParse()
        {
            bool status = false;
            Asap2Scanner scanner;
            Asap2Parser parser;
            using (var file = new FileStream(fileName, FileMode.Open))
            {
                scanner = new Asap2Scanner(file);
                parser = new Asap2Parser(scanner);
                status = parser.Parse();
            }

            if (status)
            {
                return parser.Asap2File;
            }
            else
            {
                Console.WriteLine("Line {0} {1}", scanner.line_num, scanner.chars_num);
                return null;
            }
        }

        public bool Serialise(Asap2File tree, MemoryStream outStream)
        {
            StreamWriter outputFile = new StreamWriter(outStream);
            {
                SerialiseNode(tree, outputFile, new IndentH(level: -1));
            }
            outputFile.Flush();
            return false;
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
        private void SerialiseNode(Object tree, StreamWriter stream, IndentH iH)
        {
            string elementName = null;
            BaseAttribute baseAtt = (BaseAttribute)Attribute.GetCustomAttribute(tree.GetType(), typeof(BaseAttribute));

            FieldInfo[] fI = tree.GetType().GetFields();

            for (int i = 0; i < fI.Length; i++)
            {
                ElementAttribute elemAtt = (ElementAttribute)Attribute.GetCustomAttribute(fI[i], typeof(ElementAttribute));
                if (elemAtt != null)
                {
                    if (elemAtt.IsComment && elemAtt.IsPreComment)
                    {
                        if (fI[i].GetValue(tree) != null)
                        {
                            if (baseAtt != null)
                            {
                                stream.WriteAsync(newLine);
                                iH.Indent(stream);
                            }
                            stream.WriteAsync("/*");
                            stream.WriteAsync(fI[i].GetValue(tree).ToString());
                            stream.WriteAsync("*/");
                        }
                    }
                    else if (elemAtt.IsName)
                    {
                        elementName = (string)fI[i].GetValue(tree);
                    }
                }
            }

            if (baseAtt != null)
            {
                if (!baseAtt.IsSimple)
                {
                    stream.WriteAsync(newLine);
                    iH.Indent(stream);
                    stream.WriteAsync("/begin ");
                    if (elementName != null)
                    {
                        stream.WriteAsync(elementName);
                    }
                    else
                    {
                        stream.WriteAsync(tree.GetType().Name.ToUpper());
                    }
                }
                else
                {
                    stream.WriteAsync(newLine);
                    iH.Indent(stream);
                    if (elementName != null)
                    {
                        stream.WriteAsync(elementName);
                    }
                    else
                    {
                        stream.WriteAsync(tree.GetType().Name.ToUpper());
                    }
                }
            }
            
            if (fI.Length > 0)
            {
                SerialiseElement(tree, stream, fI, new IndentH(iH));
            }

            if (baseAtt != null)
            {
                if (!baseAtt.IsSimple)
                {
                    stream.WriteAsync(newLine);
                    iH.Indent(stream);
                    stream.WriteAsync("/end ");
                    if (elementName != null)
                    {
                        stream.WriteAsync(elementName);
                    }
                    else
                    {
                        stream.WriteAsync(tree.GetType().Name.ToUpper());
                    }
                }
            }
        }

        private void SerialiseElement(Object tree, StreamWriter stream, FieldInfo[] fI, IndentH iH)
        {
            for (int i = 0; i < fI.Length; i++)
            {
                ElementAttribute att = (ElementAttribute)Attribute.GetCustomAttribute(fI[i], typeof(ElementAttribute));

                if (att != null)
                {
                    if (fI[i].GetValue(tree) != null)
                    {
                        if (att.ForceNewLine)
                        {
                            stream.WriteAsync(newLine);
                        }

                        if (att.IsComment)
                        {
                            if (!att.IsPreComment && fI[i].GetValue(tree) != null)
                            {
                                stream.WriteAsync(newLine);
                                iH.Indent(stream);
                                stream.WriteAsync("/*");
                                stream.WriteAsync(fI[i].GetValue(tree).ToString());
                                stream.WriteAsync("*/");
                            }
                        }
                        else if (att.IsArgument || att.IsString)
                        {
                            if (att.Comment != null)
                            {
                                stream.WriteAsync(newLine);
                                iH.Indent(stream);
                                stream.WriteAsync("/*");
                                stream.WriteAsync(att.Comment);
                                stream.WriteAsync("*/ ");
                            }
                            else
                            {
                                if (att.ForceNewLine)
                                {
                                    iH.Indent(stream);
                                }
                                else if (att.Name == null || att.Name == "")
                                {
                                    stream.WriteAsync(" ");
                                }
                            }
                            
                            if (att.Name != null && att.Name != "")
                            {
                                if (!att.ForceNewLine ||  (att.Comment == null))
                                {
                                    stream.WriteAsync(newLine);
                                    iH.Indent(stream);
                                } 
                                stream.WriteAsync(att.Name + " ");
                            }

                            if (att.IsString)
                            {
                                stream.WriteAsync("\"");
                                String value = fI[i].GetValue(tree).ToString();
                                value = Regex.Replace(value, "\r", @"\r");
                                value = Regex.Replace(value, "\n", @"\n");
                                value = Regex.Replace(value, "\t", @"\t");
                                stream.WriteAsync(value);
                                stream.WriteAsync("\"");
                            }
                            else
                            {
                                if (fI[i].FieldType.IsEnum)
                                {
                                    stream.WriteAsync(Enum.GetName(fI[i].FieldType, fI[i].GetValue(tree)));
                                }
                                else if (fI[i].FieldType.IsPrimitive && att.CodeAsHex)
                                {
                                    UInt64 data = (UInt64)fI[i].GetValue(tree);
                                    stream.WriteAsync("0x" + data.ToString("X"));
                                }
                                else
                                {
                                    stream.WriteAsync(fI[i].GetValue(tree).ToString());
                                }
                            }
                        }
                        else if(att.IsDictionary)
                        {
                            if (att.Comment != null)
                            {
                                stream.WriteAsync(newLine);
                                iH.Indent(stream);
                                stream.WriteAsync("/*");
                                stream.WriteAsync(att.Comment);
                                stream.WriteAsync("*/");
                            }
                            object obj = fI[i].GetValue(tree);
                            Dictionary<string, object> dict = ToDict<string, object>(obj);
                            
                            foreach (object elem in dict.Values)
                            {
                                SerialiseNode(elem, stream, iH);
                            }
                        }
                        else
                        {
                            if (fI[i].GetValue(tree) != null)
                            {
                                SerialiseNode(fI[i].GetValue(tree), stream, iH);
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
    }
}
