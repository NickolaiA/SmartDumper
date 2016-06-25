////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: DumperTest.cs
//FileType: Visual C# Source file
//Author : Nickolai Aleksandrov
//Description : Main Dumper class
////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Web.SessionState;

namespace SmartSolutions.SmartDumper
{
    public class Dumper
    {
        protected int objectCounter;

        public static StringBuilder Dump(string name, object obj, int indent)
        {
            StringBuilder sb = new StringBuilder();
            Dictionary<object, object> processed = new Dictionary<object, object>();
            Dumper dumper = new Dumper();
            dumper.objectCounter = 1;
            dumper.DumpRecursively(name, obj, indent, sb, processed);

            return sb;
        }

        private void DumpRecursively(string name, object obj, int indent, StringBuilder sb,
                                     Dictionary<object, object> processed)
        {

            string indentString = new string(' ', indent);
            sb.Append(indentString).Append(name ?? "null").Append(": ");

            if (obj == null)
            {
                sb.Append("null");
            }
            else
            {
                Type type = obj.GetType();


                //if (obj is bool || obj is byte || obj is char || obj is decimal || obj is double || obj is float || obj is int || obj is long || obj is sbyte || obj is short ||
                //    obj is uint || obj is ulong || obj is ushort)
                if (type.IsValueType)
                {
                    if (!(!type.IsPrimitive && type.Namespace != null && !type.Namespace.StartsWith("System.")) || obj is DateTime || type.FullName == typeof(decimal).FullName)
                    {
                        sb.Append(obj);
                    }

                    if (type.FullName == "System." + type.Name)
                    {
                        return;
                    }
                }

                // Do not dump string
                if (obj is string)
                {
                    sb.Append((string)obj);
                    return;
                }

                if (type.IsEnum)
                {
                    sb.Append(Enum.GetName(type, obj));
                    return;
                }

                if (processed.ContainsKey(obj))
                {
                    sb.Append("[see obj #").Append(processed[obj]).Append("]");
                    return;
                }

                processed.Add(obj, objectCounter);

                Exception exception = obj as Exception;

                if (exception != null)
                {
                    sb.Append(exception.GetType().FullName).Append(": ").Append(exception.Message);
                    return;
                }

                if (type.FullName == "System.Reflection." + type.Name || obj is System.Security.Principal.SecurityIdentifier)
                {
                    return;
                }

                //struct
                //enum

                sb.Append(type.FullName).Append(" [obj #").Append(objectCounter).AppendLine("]");

                if (!type.FullName.StartsWith("System."))
                {
                    PropertyInfo[] properties =
                        (from property in
                             type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                         where property.GetIndexParameters().Length == 0 && property.CanRead
                         select property).ToArray();

                    FieldInfo[] fields =
                        type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToArray();

                    objectCounter++;

                    if (properties.Length == 0 && fields.Length == 0)
                    {
                        sb.AppendLine();
                        return;
                    }

                    indentString = new string(' ', indent + 2);

                    if (properties.Length > 0)
                    {
                        sb.Append(indentString).Append("Properties {");

                        foreach (var propertyInfo in properties)
                        {
                            sb.AppendLine();

                            try
                            {
                                object propertyValue = propertyInfo.GetValue(obj, null);
                                DumpRecursively(propertyInfo.Name, propertyValue, indent + 4, sb, processed);
                            }
                            catch (TargetInvocationException ex)
                            {
                                DumpRecursively(propertyInfo.Name, ex.InnerException ?? ex, indent + 4, sb, processed);
                            }
                            catch (ArgumentException ex)
                            {
                                DumpRecursively(propertyInfo.Name, ex.InnerException ?? ex, indent + 4, sb, processed);
                            }
                            catch (RemotingException ex)
                            {
                                DumpRecursively(propertyInfo.Name, ex.InnerException ?? ex, indent + 4, sb, processed);
                            }
                        }

                        if (sb[sb.Length - 1] != '\n')
                        {
                            sb.AppendLine();
                        }

                        sb.Append(indentString).AppendLine("}");
                    }

                    if (fields.Length > 0)
                    {
                        bool isNotAutogeneratedExist = false;

                        foreach (FieldInfo field in fields)
                        {
                            if (!field.Name.Contains("k__"))
                            {
                                isNotAutogeneratedExist = true;
                                break;
                            }
                        }

                        if (isNotAutogeneratedExist)
                        {
                            sb.Append(indentString).Append("Fields {");

                            foreach (FieldInfo field in fields)
                            {
                                if (!field.Name.Contains("k__"))
                                {
                                    sb.AppendLine();

                                    try
                                    {
                                        object fieldValue = field.GetValue(obj);
                                        DumpRecursively(field.Name, fieldValue, indent + 4, sb, processed);
                                    }
                                    catch (TargetInvocationException ex)
                                    {
                                        DumpRecursively(field.Name, ex.InnerException ?? ex, indent + 4, sb, processed);
                                    }
                                }
                            }

                            if (sb[sb.Length - 1] != '\n')
                            {
                                sb.AppendLine();
                            }

                            sb.Append(indentString).AppendLine("}");
                        }
                    }
                }

                if (obj is IDictionary)
                {
                    IDictionary dictionary = obj as IDictionary;
                    sb.Append(indentString).Append("IDictionary[").Append(dictionary.Count).Append("] {");
                    int counter = 0;

                    foreach (DictionaryEntry dictionaryEntry in dictionary)
                    {
                        try
                        {
                            sb.Append(indentString).AppendLine("[");
                            DumpRecursively("Key", dictionaryEntry.Key, indent + 4, sb, processed);

                            try
                            {
                                if (sb[sb.Length - 1] != '\n')
                                {
                                    sb.AppendLine();
                                }

                                DumpRecursively("Value", dictionaryEntry.Value, indent + 4, sb, processed);

                                if (sb[sb.Length - 1] != '\n')
                                {
                                    sb.AppendLine();
                                }

                                sb.Append(indentString).AppendLine("]");
                            }
                            catch (Exception ex)
                            {
                                DumpRecursively("exception in item with key " + dictionaryEntry.Key,
                                                ex.InnerException ?? ex, indent + 4, sb, processed);
                            }
                        }
                        catch (Exception ex)
                        {
                            DumpRecursively("exception in item with key " + dictionaryEntry.Key.ToString(), ex.InnerException ?? ex, indent + 4, sb, processed);
                        }

                        counter++;
                    }

                    if (sb[sb.Length - 1] != '\n')
                    {
                        sb.AppendLine();
                    }

                    sb.Append(indentString).AppendLine("}");
                }
                else if (obj is HttpSessionState)
                {
                    HttpSessionState session = obj as HttpSessionState;

                    foreach (string key in session.Keys)
                    {
                        sb.AppendLine();
                        DumpRecursively(key, session[key], indent + 4, sb, processed);
                    }

                    if (sb[sb.Length - 1] != '\n')
                    {
                        sb.AppendLine();
                    }
                }
                else if (obj is IEnumerable)
                {
                    sb.Append(indentString).Append("IEnumerable[] {");
                    int counter = 0;

                    foreach (var item in obj as IEnumerable)
                    {
                        sb.AppendLine();

                        try
                        {
                            DumpRecursively(string.Format("item[{0}]", counter), item, indent + 4, sb, processed);
                        }
                        catch (Exception ex)
                        {
                            DumpRecursively(string.Format("item[{0}]", counter), ex.InnerException ?? ex, indent + 4, sb, processed);
                        }

                        counter++;
                    }

                    if (sb[sb.Length - 1] != '\n')
                    {
                        sb.AppendLine();
                    }

                    sb.Append(indentString).AppendLine("}");
                }
            }
        }
    }
}
