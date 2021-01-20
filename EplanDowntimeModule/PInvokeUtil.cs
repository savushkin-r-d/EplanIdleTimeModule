using System;
using System.Runtime.InteropServices;

namespace PInvoke
{
    public class PI
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal
                .SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 cbSize;

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }

        [DllImport("user32.dll")]
        public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
    }
}
