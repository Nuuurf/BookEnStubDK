using System.ComponentModel;

namespace RestfulApi.Models
{
    public enum UserRoles
    {
        [Description("Admin")]
        Admin,

        [Description("Owner")]
        Owner
    }
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}
