using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;

namespace TipClicker
{
    class Emulator
    {
        [DllImport("user32.dll")]
        private static extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, UInt32 dwData, IntPtr dwExtraInfo);
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(UInt16 virtualKeyCode);
        private const UInt32 MouseEventRightDown = 0x0008;
        private const UInt32 MouseEventRightUp = 0x0010;
        private const UInt32 MouseEventLeftDown = 0x0002;
        private const UInt32 MouseEventLeftUp = 0x0004;

        public static bool ifMouseActive()
        {
            if (GetAsyncKeyState(0x01) == -32768 || GetAsyncKeyState(0x01) == -32767)
            {
                //Console.WriteLine("A");
                return true;
            }
            return false;
        }

        public static void ClickDown()
        {
            mouse_event(MouseEventLeftDown, 200, 200, 0, new System.IntPtr());
        }
        public static void ClickUp()
        {
            mouse_event(MouseEventLeftUp, 200, 200, 0, new System.IntPtr());
        }
    }
}
