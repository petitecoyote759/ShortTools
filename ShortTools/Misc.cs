using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShortTools.General
{
    public static class Misc
    {
        /// <summary>
        /// The alphabet in alphabetical order, all lowercase.
        /// </summary>
        public static readonly char[] Alphabet =
            { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k',
                'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
                'w', 'x', 'y', 'z' };



















































        internal static readonly Dictionary<string, string> types = new Dictionary<string, string>()
        {
            { "System.Char", "char" },
            { "System.String", "string" },

            { "System.Int16", "short" },
            { "System.Int32", "int" },
            { "System.Int64", "long" },

            { "System.UInt16", "ushort" },
            { "System.UInt32", "uint" },
            { "System.UInt64", "ulong" },

            { "System.Boolean", "bool" },

            { "System.Single", "float" },
            { "System.Decimal", "decimal" },
            { "System.Double", "double" }
        };


        /// <summary>
        /// <para> Creates a string containing all of the variables, their types, and the values, in the form: </para>
        /// <code> AccessibilityModifier Type : VariableName = value </code>
        /// <para> Can be used to override the ToString function, example as follows: </para>
        /// <code> 
        ///  public overrride string ToString()
        ///  {
        ///      return GetDisplayString(this);
        ///  }
        /// </code>
        /// </summary>
        /// <typeparam name="T"> The type of the object given </typeparam>
        /// <param name="obj"> The object to be displayed </param>
        /// <returns> A string in the format given in the summary </returns>
        public static string GetDisplayString<T>(T obj)
        {
            StringBuilder values = new StringBuilder();

            values.AppendLine("Variables:");

            BindingFlags bindingFlags = BindingFlags.Public |
                            BindingFlags.NonPublic |
                            BindingFlags.Instance |
                BindingFlags.Static;


            foreach (FieldInfo field in typeof(T).GetFields(bindingFlags))
            {
                if (field.Name[0] == '<') { continue; }

                Type type = field.FieldType;
                object? value = field.GetValue(obj);

                values.AppendLine(

                    (field.IsPrivate ? "private" : "public") + " " +

                    (types.ContainsKey(type.ToString()) ?
                    types[type.ToString()] :
                    type) + " : " +

                    field.Name + " = " +

                    (type == typeof(string) ? "\"" + value + "\"" :
                    (type == typeof(char) ? "\'" + value + "\'" :
                    value
                    ))
                    );
            }


            values.AppendLine("Fields:");


            PropertyInfo[] props = obj.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                Type type = prop.GetValue(obj).GetType();
                object? value = prop.GetValue(obj);


                values.AppendLine(

                    (types.ContainsKey(type.ToString()) ? types[type.ToString()] : type) + " : " +

                    prop.Name + " = " +

                    (type == typeof(string) ? "\"" + value + "\"" :
                    (type == typeof(char) ? "\'" + value + "\'" :
                    value
                    ))
                    );
            }
            return values.ToString();
        }
    }
}
