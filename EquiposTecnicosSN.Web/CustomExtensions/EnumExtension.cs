using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EquiposTecnicosSN.Web.CustomExtensions
{
    public static class EnumExtension
    {
        public static string DisplayName(this Enum value)
        {
            if (value == null)
            {
                return "";
            }

            Type enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            MemberInfo member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);

            if (attrs.Length == 0)
            {
                return value.ToString();
            }

            var outString = ((DisplayAttribute)attrs[0]).Name;

            if (((DisplayAttribute)attrs[0]).ResourceType != null)
            {
                outString = ((DisplayAttribute)attrs[0]).GetName();
            }

            return outString;
        }
    }
}