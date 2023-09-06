using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTP.Utilities
{
    public class IntPtrToIWin32Window : System.Windows.Forms.IWin32Window
    {
        public IntPtrToIWin32Window(IntPtr handle)
        {
            _hwnd = handle;
        }

        public IntPtr Handle
        {
            get { return _hwnd; }
        }

        private IntPtr _hwnd;
    }
}
