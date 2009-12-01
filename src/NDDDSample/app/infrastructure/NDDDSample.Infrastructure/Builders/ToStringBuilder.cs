namespace NDDDSample.Infrastructure.Builders
{
    #region Usings

    using System.Reflection;
    using System.Text;

    #endregion

    public static class ToStringBuilder
    {
        /// <summary>
        /// Logging of the object fields
        /// </summary>
        /// <param name="obj">object to log</param>
        /// <returns>string representation</returns>
        public static string ReflectionToString(object obj)
        {
            var sb = new StringBuilder("\r\nLog: ");

            if (obj == null)
            {
                return "Object is null";
            }

            var clazz = obj.GetType();
            sb.AppendFormat("--Type:<{0}>", clazz);


            FieldInfo[] fields = clazz.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                                 | BindingFlags.GetField);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo f = fields[i];
                if (!f.IsStatic)
                {
                    sb.AppendFormat("-Field <{0}> value <{1}>", f.Name, f.GetValue(obj) ?? "null");
                }
            }
            return sb.ToString();
        }
    }
}