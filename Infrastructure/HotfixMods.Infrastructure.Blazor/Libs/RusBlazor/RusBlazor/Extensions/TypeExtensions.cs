using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RusBlazor.Extensions
{
    public static class TypeExtensions
    {
        const string int8 = "int8";
        const string uint8 = "uint8";
        const string int16 = "int16";
        const string uint16 = "uint16";
        const string int32 = "int32";
        const string uint32 = "uint32";
        const string int64 = "int64";
        const string uint64 = "uint64";
        const string dec = "float";
        const string str = "string";
        public static Dictionary<Type, string> DataTypes = new()
        {
            { typeof(sbyte), int8 },
            { typeof(byte), uint8 },
            { typeof(short), int16 },
            { typeof(ushort), uint16 },
            { typeof(int), int32 },
            { typeof(uint), uint32 },
            { typeof(long), int64 },
            { typeof(ulong), uint64 },
            { typeof(decimal), dec },
            { typeof(string), str },
        };

        public static bool IsWholeNumberType(this Type type)
        {
            return (type == typeof(byte) || type == typeof(sbyte) ||
                type == typeof(short) || type == typeof(ushort) ||
                type == typeof(int) || type == typeof(uint) ||
                type == typeof(long) || type == typeof(ulong));
        }

        public static bool TryGetMaxValue<T>(this Type type, out T value)
        {
            if (type.IsUnsignedNumberType())
            {
                FieldInfo? field = type.GetField("MaxValue");
                if (field != null)
                {
                    value = (T)field.GetValue(null);
                    return true;
                }
            }
            else
            {
                try
                {
                    value = (T)Convert.ChangeType(-1, type);
                    return true;
                }
                catch (Exception ex)
                {
                    // Not valid type
                }
            }

            value = default(T);
            return false;
        }

        public static bool TryGetTypeAsUnsigned(this Type type, out Type? value)
        {
            value = null;
            if (DataTypes.ContainsKey(type))
            {
                value = DataTypes[type] switch
                {
                    int8 or uint8 => typeof(byte),
                    int16 or uint16 => typeof(ushort),
                    int32 or uint32 => typeof(uint),
                    int64 or uint64 => typeof(ulong),
                    _ => null
                };
            }


            return value != null;
        }

        public static bool IsUnsignedNumberType(this Type type)
        {
            var unsignedTypes = new[] { typeof(byte), typeof(ushort), typeof(uint), typeof(ulong) };
            return unsignedTypes.Contains(type);
        }

        public static string ToTypeDisplayName(this Type type)
        {
            if (DataTypes.ContainsKey(type))
                return DataTypes[type];
            return type.Name;
        }
    }
}
