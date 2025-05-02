using System;
using System.Runtime.InteropServices;

namespace Perpetuum
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Maps a byte array to struct array
        /// </summary>
        public static T[] ToArray<T>(this byte[] source) where T : struct
        {
            var sizeOf = Marshal.SizeOf(typeof(T));
            var to = new T[source.Length / sizeOf];

            if (typeof(T).IsPrimitive)
            {
                Buffer.BlockCopy(source, 0, to, 0, source.Length);
            }
            else
            {
                var handle = GCHandle.Alloc(to, GCHandleType.Pinned);
                try
                {
                    Marshal.Copy(source, 0, handle.AddrOfPinnedObject(), source.Length);
                }
                finally
                {
                    handle.Free();
                }
            }
            return to;
        }

        public static T ToStruct<T>(this byte[] array) where T : struct
        {
            var size = Marshal.SizeOf(default(T));
            var ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(array, 0, ptr, size);
                return (T)Marshal.PtrToStructure(ptr, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}