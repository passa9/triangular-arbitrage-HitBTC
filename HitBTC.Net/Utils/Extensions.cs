using System;
using System.Runtime.Serialization;

namespace HitBTC.Net.Utils
{
    public static class Extensions
    {
        internal static string Format(this DateTime dateTime) => dateTime.ToString("o");

        internal static string Format(this Enum value)
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);

            if (attributes?[0] is EnumMemberAttribute enumMemberAttribute)
                return enumMemberAttribute.Value;
            else
                return value.ToString();
        }

        internal static string TryCreateParameter(this object value, string parameterName) =>
            value == null ? string.Empty : CreateParameter(value.ToString(), parameterName);

        internal static string TryCreateParameter(this DateTime? value, string parameterName) =>
            value == null ? string.Empty : CreateParameter(value.Value.Format(), parameterName);

        internal static string TryCreateParameter(this Enum value, string parameterName)
        {
            if (value == null)
                return string.Empty;

            string enumString = value.Format();

            return CreateParameter(enumString, parameterName);
        }

        private static string CreateParameter(string value, string parameterName) =>
            $"{parameterName}={value}&";
    }
}
