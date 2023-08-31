using System.Runtime.InteropServices;


namespace RusBlazor.Extensions
{
    public static class ListExtensions
    {
        public static T GetValueByFlags<T>(this List<T> list)
        {
            Type? unsignedType;
            if (!typeof(T).IsWholeNumberType())
                throw new ArgumentException($"Type {typeof(T)} is not supported here.");
            else if (!typeof(T).TryGetTypeAsUnsigned(out unsignedType))
                throw new ArgumentException($"Type {typeof(T)} does not have a supported unsigned type.");

            dynamic result = 0;
            dynamic unsignedResult = Activator.CreateInstance(unsignedType);

            foreach (var item in list)
            {
                byte[] bytes = BitConverter.GetBytes((dynamic)item);

                if (unsignedType == typeof(byte))
                    unsignedResult += bytes[0];
                else if (unsignedType == typeof(ushort))
                    unsignedResult += BitConverter.ToUInt16(bytes, 0);
                else if (unsignedType == typeof(uint))
                    unsignedResult += BitConverter.ToUInt32(bytes, 0);
                else if (unsignedType == typeof(ulong))
                    unsignedResult += BitConverter.ToUInt64(bytes, 0);
            }

            unchecked
            {
                if (typeof(T) == typeof(int))
                    result = (int)(uint)unsignedResult;
                else if (typeof(T) == typeof(long))
                    result = (long)(ulong)unsignedResult;
                else if (typeof(T) == typeof(short))
                    result = (short)(ushort)unsignedResult;
                else if (typeof(T) == typeof(sbyte))
                    result = (sbyte)(byte)unsignedResult;
                else
                    result = Convert.ChangeType(unsignedResult, typeof(T));
            }

            return (T)result;
        }

        public static void GetFlagsByValue<T>(this List<T> list, T flag)
        {
            Type? unsignedType;
            if (!typeof(T).IsWholeNumberType())
                throw new ArgumentException($"Type {typeof(T)} is not supported here.");
            else if (!typeof(T).TryGetTypeAsUnsigned(out unsignedType))
                throw new ArgumentException($"Type {typeof(T)} does not have a supported unsigned type.");

            dynamic value = 0;
            byte[] bytes = BitConverter.GetBytes((dynamic)flag);

            if (unsignedType == typeof(byte))
                value = bytes[0];
            else if (unsignedType == typeof(ushort))
                value = BitConverter.ToUInt16(bytes, 0);
            else if (unsignedType == typeof(uint))
                value = BitConverter.ToUInt32(bytes, 0);
            else if (unsignedType == typeof(ulong))
                value = BitConverter.ToUInt64(bytes, 0);

            dynamic one = Convert.ChangeType(1, unsignedType);
            list.Clear();

            int size = Marshal.SizeOf(Activator.CreateInstance(unsignedType)) * 8;
            for (int i = 0; i < size; i++)
            {
                dynamic mask = one << i;
                if ((value & mask) != 0)
                {
                    unchecked
                    {
                        if (typeof(T) == typeof(int))
                            list.Add((T)(object)(int)(uint)mask);
                        else if (typeof(T) == typeof(long))
                            list.Add((T)(object)(long)(ulong)mask);
                        else if (typeof(T) == typeof(short))
                            list.Add((T)(object)(short)(ushort)mask);
                        else if (typeof(T) == typeof(sbyte))
                            list.Add((T)(object)(sbyte)(byte)mask);
                        else
                            list.Add((T)Convert.ChangeType(mask, typeof(T)));
                    }
                }
            }
        }

        public static bool TrySum<T>(this List<T> list, out T value)
        {
            value = default(T);
            if (!typeof(T).IsWholeNumberType())
                return false;

            dynamic sum = default(T);
            foreach (var item in list)
            {
                sum += (dynamic)item;
            }
            value = sum;
            return true;
        }
    }
}
