using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Argus.Extensions;

namespace Argus.Windows
{
    /// <summary>
    ///     A class that represents a Window, or a child object of a Window.
    /// </summary>
    /// <remarks>
    ///     TODO - Test this, it took quite a bit of wrestling to get the messages and API calls converted.
    /// </remarks>
    public class WindowWrapper
    {
        //*********************************************************************************************************************
        //
        //             Class:  WindowWrapper
        //      Organization:  http://www.blakepell.com  
        //      Initial Date:  11/30/2010
        //     Last Modified:  11/22/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************      

        #region "Windows API Declarations"

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr GetWindowText(IntPtr hwnd, string lpString, int cch);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindowByClass(string lpClassName, IntPtr zero);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindowByCaption(IntPtr zero, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        private static extern bool SetForegroundWindowEx(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool SetWindowText(IntPtr hwnd, string lpString);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            /// <summary>
            /// x position of the upper-left corner
            /// </summary>
            public int Left;

            /// <summary>
            /// y position of the upper-left corner
            /// </summary>
            public int Top;

            /// <summary>
            /// x position of the lower-right corner
            /// </summary>
            public int Right;

            /// <summary>
            ///  y position of the lower-right corner
            /// </summary>
            public int Bottom;
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, string lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, uint wParam, string lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, uint wParam, StringBuilder lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, [Out] StringBuilder lParam);

        /// <summary>
        ///     Used for minimzing the Window, does not destory the window though.
        /// </summary>
        /// <param name="hWnd"></param>
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int CloseWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool ShowWindow(IntPtr hwnd, ShowWindowCommand nCmdShow);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern void GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        /// <summary>
        ///     Commands associated with the ShowWindow API and it's internal implementation in this class, SetWindowState.
        /// </summary>
        public enum ShowWindowCommand
        {
            /// <summary>
            ///     Hides the window and activates another window.
            /// </summary>
            Hide = 0,

            /// <summary>
            ///     Activates and displays a window. If the window is minimized or
            ///     maximized, the system restores it to its original size and position.
            ///     An application should specify this flag when displaying the window
            ///     for the first time.
            /// </summary>
            Normal = 1,

            /// <summary>
            ///     Activates the window and displays it as a minimized window.
            /// </summary>
            ShowMinimized = 2,

            /// <summary>
            ///     Maximizes the specified window.
            /// </summary>
            Maximize = 3,

            // is this the right value?
            /// <summary>
            ///     Activates the window and displays it as a maximized window.
            /// </summary>
            ShowMaximized = 3,

            /// <summary>
            ///     Displays a window in its most recent size and position. This value
            ///     is similar to Win32.ShowWindowCommand.Normal, except
            ///     the window is not actived.
            /// </summary>
            ShowNoActivate = 4,

            /// <summary>
            ///     Activates the window and displays it in its current size and position.
            /// </summary>
            Show = 5,

            /// <summary>
            ///     Minimizes the specified window and activates the next top-level
            ///     window in the Z order.
            /// </summary>
            Minimize = 6,

            /// <summary>
            ///     Displays the window as a minimized window. This value is similar to
            ///     Win32.ShowWindowCommand.ShowMinimized, except the
            ///     window is not activated.
            /// </summary>
            ShowMinNoActive = 7,

            /// <summary>
            ///     Displays the window in its current size and position. This value is
            ///     similar to Win32.ShowWindowCommand.Show, except the
            ///     window is not activated.
            /// </summary>
            ShowNA = 8,

            /// <summary>
            ///     Activates and displays the window. If the window is minimized or
            ///     maximized, the system restores it to its original size and position.
            ///     An application should specify this flag when restoring a minimized window.
            /// </summary>
            Restore = 9,

            /// <summary>
            ///     Sets the show state based on the SW_* value specified in the
            ///     STARTUPINFO structure passed to the CreateProcess function by the
            ///     program that started the application.
            /// </summary>
            ShowDefault = 10,

            /// <summary>
            ///     <b>Windows 2000/XP:</b> Minimizes a window, even if the thread
            ///     that owns the window is not responding. This flag should only be
            ///     used when minimizing windows from a different thread.
            /// </summary>
            ForceMinimize = 11
        }

        /// <summary>
        ///     Messages for use with the SendMessage API
        /// </summary>
        public enum Messages : uint
        {
            WM_USER = 0x400,
            BN_CLICKED = 245,
            BM_GETCHECK = 0xf0,
            BM_SETCHECK = 0xf1,
            BM_GETSTATE = 0xf2,
            BM_SETSTATE = 0xf3,
            BM_SETSTYLE = 0xf4,
            BM_CLICK = 0xf5,
            BM_GETIMAGE = 0xf6,
            BM_SETIMAGE = 0xf7,
            CB_GETEDITSEL = 0x140,
            CB_LIMITTEXT = 0x141,
            CB_SETEDITSEL = 0x142,
            CB_ADDSTRING = 0x143,
            CB_DELETESTRING = 0x144,
            CB_DIR = 0x145,
            CB_GETCOUNT = 0x146,
            CB_GETCURSEL = 0x147,
            CB_GETLBTEXT = 0x148,
            CB_GETLBTEXTLEN = 0x149,
            CB_INSERTSTRING = 0x14a,
            CB_RESETCONTENT = 0x14b,
            CB_FINDSTRING = 0x14c,
            CB_SELECTSTRING = 0x14d,
            CB_SETCURSEL = 0x14e,
            CB_SHOWDROPDOWN = 0x14f,
            CB_GETITEMDATA = 0x150,
            CB_SETITEMDATA = 0x151,
            CB_GETDROPPEDCONTROLRECT = 0x152,
            CB_SETITEMHEIGHT = 0x153,
            CB_GETITEMHEIGHT = 0x154,
            CB_SETEXTENDEDUI = 0x155,
            CB_GETEXTENDEDUI = 0x156,
            CB_GETDROPPEDSTATE = 0x157,
            CB_FINDSTRINGEXACT = 0x158,
            CB_SETLOCALE = 0x159,
            CB_GETLOCALE = 0x15a,
            CB_GETTOPINDEX = 0x15b,
            CB_SETTOPINDEX = 0x15c,
            CB_GETHORIZONTALEXTENT = 0x15d,
            CB_SETHORIZONTALEXTENT = 0x15e,
            CB_GETDROPPEDWIDTH = 0x15f,
            CB_SETDROPPEDWIDTH = 0x160,
            CB_INITSTORAGE = 0x161,
            CB_MSGMAX = 0x162,
            EM_CANUNDO = 0xc6,
            EM_EMPTYUNDOBUFFER = 0xcd,
            EM_FMTLINES = 0xc8,
            EM_FORMATRANGE = WM_USER + 57,
            EM_GETFIRSTVISIBLELINE = 0xce,
            EM_GETHANDLE = 0xbd,
            EM_GETLINE = 0xc4,
            EM_GETLINECOUNT = 0xba,
            EM_GETMODIFY = 0xb8,
            EM_GETPASSWORDCHAR = 0xd2,
            EM_GETRECT = 0xb2,
            EM_GETSEL = 0xb0,
            EM_GETTHUMB = 0xbe,
            EM_GETWORDBREAKPROC = 0xd1,
            EM_LIMITTEXT = 0xc5,
            EM_LINEFROMCHAR = 0xc9,
            EM_LINEINDEX = 0xbb,
            EM_LINELENGTH = 0xc1,
            EM_LINESCROLL = 0xb6,
            EM_REPLACESEL = 0xc2,
            EM_SCROLL = 0xb5,
            EM_SCROLLCARET = 0xb7,
            EM_SETHANDLE = 0xbc,
            EM_SETMODIFY = 0xb9,
            EM_SETPASSWORDCHAR = 0xcc,
            EM_SETREADONLY = 0xcf,
            EM_SETRECT = 0xb3,
            EM_SETRECTNP = 0xb4,
            EM_SETSEL = 0xb1,
            EM_SETTABSTOPS = 0xcb,
            EM_SETTARGETDEVICE = WM_USER + 72,
            EM_SETWORDBREAKPROC = 0xd0,
            EM_UNDO = 0xc7,
            HDS_HOTTRACK = 0x4,
            HDI_BITMAP = 0x10,
            HDI_IMAGE = 0x20,
            HDI_ORDER = 0x80,
            HDI_FORMAT = 0x4,
            HDI_TEXT = 0x2,
            HDI_WIDTH = 0x1,
            HDI_HEIGHT = HDI_WIDTH,
            HDF_LEFT = 0,
            HDF_RIGHT = 1,
            HDF_IMAGE = 0x800,
            HDF_BITMAP_ON_RIGHT = 0x1000,
            HDF_BITMAP = 0x2000,
            HDF_STRING = 0x4000,
            HDM_FIRST = 0x1200,
            HDM_SETITEM = HDM_FIRST + 4,
            LB_ADDFILE = 0x196,
            LB_ADDSTRING = 0x180,
            LB_CTLCODE = 0,
            LB_DELETESTRING = 0x182,
            LB_DIR = 0x18d,

            //LB_ERR = -1
            //LB_ERRSPACE = -2
            LB_FINDSTRING = 0x18f,
            LB_FINDSTRINGEXACT = 0x1a2,
            LB_GETANCHORINDEX = 0x19d,
            LB_GETCARETINDEX = 0x19f,
            LB_GETCOUNT = 0x18b,
            LB_GETCURSEL = 0x188,
            LB_GETHORIZONTALEXTENT = 0x193,
            LB_GETITEMDATA = 0x199,
            LB_GETITEMHEIGHT = 0x1a1,
            LB_GETITEMRECT = 0x198,
            LB_GETLOCALE = 0x1a6,
            LB_GETSEL = 0x187,
            LB_GETSELCOUNT = 0x190,
            LB_GETSELITEMS = 0x191,
            LB_GETTEXT = 0x189,
            LB_GETTEXTLEN = 0x18a,
            LB_GETTOPINDEX = 0x18e,
            LB_INSERTSTRING = 0x181,
            LB_MSGMAX = 0x1a8,
            LB_OKAY = 0,
            LB_RESETCONTENT = 0x184,
            LB_SELECTSTRING = 0x18c,
            LB_SELITEMRANGE = 0x19b,
            LB_SELITEMRANGEEX = 0x183,
            LB_SETANCHORINDEX = 0x19c,
            LB_SETCARETINDEX = 0x19e,
            LB_SETCOLUMNWIDTH = 0x195,
            LB_SETCOUNT = 0x1a7,
            LB_SETCURSEL = 0x186,
            LB_SETHORIZONTALEXTENT = 0x194,
            LB_SETITEMDATA = 0x19a,
            LB_SETITEMHEIGHT = 0x1a0,
            LB_SETLOCALE = 0x1a5,
            LB_SETSEL = 0x185,
            LB_SETTABSTOPS = 0x192,
            LB_SETTOPINDEX = 0x197,
            LBN_DBLCLK = 2,

            //LBN_ERRSPACE = -2
            LBN_KILLFOCUS = 5,
            LBN_SELCANCEL = 3,
            LBN_SELCHANGE = 1,
            LBN_SETFOCUS = 4,
            LVM_FIRST = 0x1000,
            LVM_GETHEADER = LVM_FIRST + 31,
            LVM_GETBKCOLOR = LVM_FIRST + 0,
            LVM_SETBKCOLOR = LVM_FIRST + 1,
            LVM_GETIMAGELIST = LVM_FIRST + 2,
            LVM_SETIMAGELIST = LVM_FIRST + 3,
            LVM_GETITEMCOUNT = LVM_FIRST + 4,
            LVM_GETITEMA = LVM_FIRST + 5,
            LVM_GETITEM = LVM_GETITEMA,
            LVM_SETITEMA = LVM_FIRST + 6,
            LVM_SETITEM = LVM_SETITEMA,
            LVM_INSERTITEMA = LVM_FIRST + 7,
            LVM_INSERTITEM = LVM_INSERTITEMA,
            LVM_DELETEITEM = LVM_FIRST + 8,
            LVM_DELETEALLITEMS = LVM_FIRST + 9,
            LVM_GETCALLBACKMASK = LVM_FIRST + 10,
            LVM_SETCALLBACKMASK = LVM_FIRST + 11,
            LVM_GETNEXTITEM = LVM_FIRST + 12,
            LVM_FINDITEMA = LVM_FIRST + 13,
            LVM_FINDITEM = LVM_FINDITEMA,
            LVM_GETITEMRECT = LVM_FIRST + 14,
            LVM_SETITEMPOSITION = LVM_FIRST + 15,
            LVM_GETITEMPOSITION = LVM_FIRST + 16,
            LVM_GETSTRINGWIDTHA = LVM_FIRST + 17,
            LVM_GETSTRINGWIDTH = LVM_GETSTRINGWIDTHA,
            LVM_HITTEST = LVM_FIRST + 18,
            LVM_ENSUREVISIBLE = LVM_FIRST + 19,
            LVM_SCROLL = LVM_FIRST + 20,
            LVM_REDRAWITEMS = LVM_FIRST + 21,
            LVM_ARRANGE = LVM_FIRST + 22,
            LVM_EDITLABELA = LVM_FIRST + 23,
            LVM_EDITLABEL = LVM_EDITLABELA,
            LVM_GETEDITCONTROL = LVM_FIRST + 24,
            LVM_GETCOLUMNA = LVM_FIRST + 25,
            LVM_GETCOLUMN = LVM_GETCOLUMNA,
            LVM_SETCOLUMNA = LVM_FIRST + 26,
            LVM_SETCOLUMN = LVM_SETCOLUMNA,
            LVM_INSERTCOLUMNA = LVM_FIRST + 27,
            LVM_INSERTCOLUMN = LVM_INSERTCOLUMNA,
            LVM_DELETECOLUMN = LVM_FIRST + 28,
            LVM_GETCOLUMNWIDTH = LVM_FIRST + 29,
            LVM_SETCOLUMNWIDTH = LVM_FIRST + 30,
            LVM_CREATEDRAGIMAGE = LVM_FIRST + 33,
            LVM_GETVIEWRECT = LVM_FIRST + 34,
            LVM_GETTEXTCOLOR = LVM_FIRST + 35,
            LVM_SETTEXTCOLOR = LVM_FIRST + 36,
            LVM_GETTEXTBKCOLOR = LVM_FIRST + 37,
            LVM_SETTEXTBKCOLOR = LVM_FIRST + 38,
            LVM_GETTOPINDEX = LVM_FIRST + 39,
            LVM_GETCOUNTPERPAGE = LVM_FIRST + 40,
            LVM_GETORIGIN = LVM_FIRST + 41,
            LVM_UPDATE = LVM_FIRST + 42,
            LVM_SETITEMSTATE = LVM_FIRST + 43,
            LVM_GETITEMSTATE = LVM_FIRST + 44,
            LVM_GETITEMTEXTA = LVM_FIRST + 45,
            LVM_GETITEMTEXT = LVM_GETITEMTEXTA,
            LVM_SETITEMTEXTA = LVM_FIRST + 46,
            LVM_SETITEMTEXT = LVM_SETITEMTEXTA,
            LVM_SETITEMCOUNT = LVM_FIRST + 47,
            LVM_SORTITEMS = LVM_FIRST + 48,
            LVM_SETITEMPOSITION32 = LVM_FIRST + 49,
            LVM_GETSELECTEDCOUNT = LVM_FIRST + 50,
            LVM_GETITEMSPACING = LVM_FIRST + 51,
            LVM_GETISEARCHSTRINGA = LVM_FIRST + 52,
            LVM_GETISEARCHSTRING = LVM_GETISEARCHSTRINGA,
            LVM_SETICONSPACING = LVM_FIRST + 53,
            LVM_SETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 54,
            LVM_GETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 55,
            LVM_GETSUBITEMRECT = LVM_FIRST + 56,
            LVM_SUBITEMHITTEST = LVM_FIRST + 57,
            LVM_SETCOLUMNORDERARRAY = LVM_FIRST + 58,
            LVM_GETCOLUMNORDERARRAY = LVM_FIRST + 59,
            LVM_SETHOTITEM = LVM_FIRST + 60,
            LVM_GETHOTITEM = LVM_FIRST + 61,
            LVM_SETHOTCURSOR = LVM_FIRST + 62,
            LVM_GETHOTCURSOR = LVM_FIRST + 63,
            LVM_APPROXIMATEVIEWRECT = LVM_FIRST + 64,
            LVS_EX_FULLROWSELECT = 0x20,

            //LVSCW_AUTOSIZE = -1
            //LVSCW_AUTOSIZE_USEHEADER  = -2
            WM_ACTIVATE = 0x6,
            WM_ACTIVATEAPP = 0x1c,
            WM_ASKCBFORMATNAME = 0x30c,
            WM_CANCELJOURNAL = 0x4b,
            WM_CANCELMODE = 0x1f,
            WM_CHANGECBCHAIN = 0x30d,
            WM_CHAR = 0x102,
            WM_CHARTOITEM = 0x2f,
            WM_CHILDACTIVATE = 0x22,
            WM_CHOOSEFONT_GETLOGFONT = WM_USER + 1,
            WM_CHOOSEFONT_SETFLAGS = WM_USER + 102,
            WM_CHOOSEFONT_SETLOGFONT = WM_USER + 101,
            WM_CLEAR = 0x303,
            WM_CLOSE = 0x10,
            WM_COMMAND = 0x111,
            WM_COMMNOTIFY = 0x44,

            // no longer suported
            WM_COMPACTING = 0x41,
            WM_COMPAREITEM = 0x39,
            WM_CONVERTREQUESTEX = 0x108,
            WM_COPY = 0x301,
            WM_COPYDATA = 0x4a,
            WM_CREATE = 0x1,
            WM_CTLCOLORBTN = 0x135,
            WM_CTLCOLORDLG = 0x136,
            WM_CTLCOLOREDIT = 0x133,
            WM_CTLCOLORLISTBOX = 0x134,
            WM_CTLCOLORMSGBOX = 0x132,
            WM_CTLCOLORSCROLLBAR = 0x137,
            WM_CTLCOLORSTATIC = 0x138,
            WM_CUT = 0x300,
            WM_DDE_FIRST = 0x3e0,
            WM_DDE_ACK = WM_DDE_FIRST + 4,
            WM_DDE_ADVISE = WM_DDE_FIRST + 2,
            WM_DDE_DATA = WM_DDE_FIRST + 5,
            WM_DDE_EXECUTE = WM_DDE_FIRST + 8,
            WM_DDE_INITIATE = WM_DDE_FIRST,
            WM_DDE_LAST = WM_DDE_FIRST + 8,
            WM_DDE_POKE = WM_DDE_FIRST + 7,
            WM_DDE_REQUEST = WM_DDE_FIRST + 6,
            WM_DDE_TERMINATE = WM_DDE_FIRST + 1,
            WM_DDE_UNADVISE = WM_DDE_FIRST + 3,
            WM_DEADCHAR = 0x103,
            WM_DELETEITEM = 0x2d,
            WM_DESTROY = 0x2,
            WM_DESTROYCLIPBOARD = 0x307,
            WM_DEVMODECHANGE = 0x1b,
            WM_DRAWCLIPBOARD = 0x308,
            WM_DRAWITEM = 0x2b,
            WM_DROPFILES = 0x233,
            WM_ENABLE = 0xa,
            WM_ENDSESSION = 0x16,
            WM_ENTERIDLE = 0x121,
            WM_ENTERMENULOOP = 0x211,
            WM_ERASEBKGND = 0x14,
            WM_EXITMENULOOP = 0x212,
            WM_FONTCHANGE = 0x1d,
            WM_GETFONT = 0x31,
            WM_GETDLGCODE = 0x87,
            WM_GETHOTKEY = 0x33,
            WM_GETMINMAXINFO = 0x24,
            WM_GETTEXT = 0xd,
            WM_GETTEXTLENGTH = 0xe,
            WM_HOTKEY = 0x312,
            WM_HSCROLL = 0x114,
            WM_HSCROLLCLIPBOARD = 0x30e,
            WM_ICONERASEBKGND = 0x27,
            WM_IME_CHAR = 0x286,
            WM_IME_COMPOSITION = 0x10f,
            WM_IME_COMPOSITIONFULL = 0x284,
            WM_IME_CONTROL = 0x283,
            WM_IME_ENDCOMPOSITION = 0x10e,
            WM_IME_KEYDOWN = 0x290,
            WM_IME_KEYLAST = 0x10f,
            WM_IME_KEYUP = 0x291,
            WM_IME_NOTIFY = 0x282,
            WM_IME_SELECT = 0x285,
            WM_IME_SETCONTEXT = 0x281,
            WM_IME_STARTCOMPOSITION = 0x10d,
            WM_INITDIALOG = 0x110,
            WM_INITMENU = 0x116,
            WM_INITMENUPOPUP = 0x117,
            WM_KEYDOWN = 0x100,
            WM_KEYFIRST = 0x100,
            WM_KEYLAST = 0x108,
            WM_KEYUP = 0x101,
            WM_KILLFOCUS = 0x8,
            WM_LBUTTONDBLCLK = 0x203,
            WM_LBUTTONDOWN = 0x201,
            WM_LBUTTONUP = 0x202,
            WM_MBUTTONDBLCLK = 0x209,
            WM_MBUTTONDOWN = 0x207,
            WM_MBUTTONUP = 0x208,
            WM_MDIACTIVATE = 0x222,
            WM_MDICASCADE = 0x227,
            WM_MDICREATE = 0x220,
            WM_MDIDESTROY = 0x221,
            WM_MDIGETACTIVE = 0x229,
            WM_MDIICONARRANGE = 0x228,
            WM_MDIMAXIMIZE = 0x225,
            WM_MDINEXT = 0x224,
            WM_MDIREFRESHMENU = 0x234,
            WM_MDIRESTORE = 0x223,
            WM_MDISETMENU = 0x230,
            WM_MDITILE = 0x226,
            WM_MEASUREITEM = 0x2c,
            WM_MENUCHAR = 0x120,
            WM_MENUSELECT = 0x11f,
            WM_MOUSEACTIVATE = 0x21,
            WM_MOUSEFIRST = 0x200,
            WM_MOUSELAST = 0x209,
            WM_MOUSEMOVE = 0x200,
            WM_MOVE = 0x3,
            WM_NCACTIVATE = 0x86,
            WM_NCCALCSIZE = 0x83,
            WM_NCCREATE = 0x81,
            WM_NCDESTROY = 0x82,
            WM_NCHITTEST = 0x84,
            WM_NCLBUTTONDBLCLK = 0xa3,
            WM_NCLBUTTONDOWN = 0xa1,
            WM_NCLBUTTONUP = 0xa2,
            WM_NCMBUTTONDBLCLK = 0xa9,
            WM_NCMBUTTONDOWN = 0xa7,
            WM_NCMBUTTONUP = 0xa8,
            WM_NCMOUSEMOVE = 0xa0,
            WM_NCPAINT = 0x85,
            WM_NCRBUTTONDBLCLK = 0xa6,
            WM_NCRBUTTONDOWN = 0xa4,
            WM_NCRBUTTONUP = 0xa5,
            WM_NEXTDLGCTL = 0x28,
            WM_NULL = 0x0,
            WM_OTHERWINDOWCREATED = 0x42,

            // no longer suported
            WM_OTHERWINDOWDESTROYED = 0x43,

            // no longer suported
            WM_PAINT = 0xf,
            WM_PAINTCLIPBOARD = 0x309,
            WM_PAINTICON = 0x26,
            WM_PALETTECHANGED = 0x311,
            WM_PALETTEISCHANGING = 0x310,
            WM_PARENTNOTIFY = 0x210,
            WM_PASTE = 0x302,
            WM_PENWINFIRST = 0x380,
            WM_PENWINLAST = 0x38f,
            WM_POWER = 0x48,
            WM_PSD_ENVSTAMPRECT = WM_USER + 5,
            WM_PSD_FULLPAGERECT = WM_USER + 1,
            WM_PSD_GREEKTEXTRECT = WM_USER + 4,
            WM_PSD_MARGINRECT = WM_USER + 3,
            WM_PSD_MINMARGINRECT = WM_USER + 2,
            WM_PSD_PAGESETUPDLG = WM_USER,
            WM_PSD_YAFULLPAGERECT = WM_USER + 6,
            WM_QUERYDRAGICON = 0x37,
            WM_QUERYENDSESSION = 0x11,
            WM_QUERYNEWPALETTE = 0x30f,
            WM_QUERYOPEN = 0x13,
            WM_QUEUESYNC = 0x23,
            WM_QUIT = 0x12,
            WM_RBUTTONDBLCLK = 0x206,
            WM_RBUTTONDOWN = 0x204,
            WM_RBUTTONUP = 0x205,
            WM_RENDERALLFORMATS = 0x306,
            WM_RENDERFORMAT = 0x305,
            WM_SETCURSOR = 0x20,
            WM_SETFOCUS = 0x7,
            WM_SETFONT = 0x30,
            WM_SETHOTKEY = 0x32,
            WM_SETREDRAW = 0xb,
            WM_SETTEXT = 0xc,
            WM_SHOWWINDOW = 0x18,
            WM_SIZE = 0x5,
            WM_SIZECLIPBOARD = 0x30b,
            WM_SPOOLERSTATUS = 0x2a,
            WM_SYSCHAR = 0x106,
            WM_SYSCOLORCHANGE = 0x15,
            WM_SYSCOMMAND = 0x112,
            WM_SYSDEADCHAR = 0x107,
            WM_SYSKEYDOWN = 0x104,
            WM_SYSKEYUP = 0x105,
            WM_TIMECHANGE = 0x1e,
            WM_TIMER = 0x113,
            WM_UNDO = 0x304,
            WM_VKEYTOITEM = 0x2e,
            WM_VSCROLL = 0x115,
            WM_VSCROLLCLIPBOARD = 0x30a,
            WM_WINDOWPOSCHANGED = 0x47,
            WM_WINDOWPOSCHANGING = 0x46,
            WM_WININICHANGE = 0x1a,
            WS_BORDER = 0x800000,
            WS_CAPTION = 0xc00000,

            // WS_BORDER Or WS_DLGFRAME
            WS_CHILD = 0x40000000,
            WS_CHILDWINDOW = WS_CHILD,
            WS_CLIPCHILDREN = 0x2000000,
            WS_CLIPSIBLINGS = 0x4000000,
            WS_DISABLED = 0x8000000,
            WS_DLGFRAME = 0x400000,
            WS_EX_ACCEPTFILES = 0x10,
            WS_EX_DLGMODALFRAME = 0x1,
            WS_EX_NOPARENTNOTIFY = 0x4,
            WS_EX_TOPMOST = 0x8,
            WS_EX_TRANSPARENT = 0x20,
            WS_GROUP = 0x20000,
            WS_HSCROLL = 0x100000,
            WS_MINIMIZE = 0x20000000,
            WS_ICONIC = WS_MINIMIZE,
            WS_MAXIMIZE = 0x1000000,
            WS_MAXIMIZEBOX = 0x10000,
            WS_MINIMIZEBOX = 0x20000,
            WS_SYSMENU = 0x80000,
            WS_THICKFRAME = 0x40000,
            WS_OVERLAPPED = 0x0,
            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,

            //WS_POPUP = &H80000000
            //WS_POPUPWINDOW = (WS_POPUP Or WS_BORDER Or WS_SYSMENU)
            WS_SIZEBOX = WS_THICKFRAME,
            WS_TABSTOP = 0x10000,
            WS_TILED = WS_OVERLAPPED,
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,
            WS_VISIBLE = 0x10000000,
            WS_VSCROLL = 0x200000,
            LBS_DISABLENOSCROLL = 0x1000,
            LBS_EXTENDEDSEL = 0x800,
            LBS_HASSTRINGS = 0x40,
            LBS_MULTICOLUMN = 0x200,
            LBS_MULTIPLESEL = 0x8,
            LBS_NODATA = 0x2000,
            LBS_NOINTEGRALHEIGHT = 0x100,
            LBS_NOREDRAW = 0x4,
            LBS_NOTIFY = 0x1,
            LBS_OWNERDRAWFIXED = 0x10,
            LBS_OWNERDRAWVARIABLE = 0x20,
            LBS_SORT = 0x2,
            LBS_STANDARD = LBS_NOTIFY | LBS_SORT | WS_VSCROLL | WS_BORDER,
            LBS_USETABSTOPS = 0x80,
            LBS_WANTKEYBOARDINPUT = 0x400,

            //LBSELCHSTRING = "commdlg_LBSelChangedNotify"
            TB_ENABLEBUTTON = WM_USER + 1,
            TB_CHECKBUTTON = WM_USER + 2,
            TB_PRESSBUTTON = WM_USER + 3,
            TB_HIDEBUTTON = WM_USER + 4,
            TB_INDETERMINATE = WM_USER + 5,
            TB_MARKBUTTON = WM_USER + 6,
            TB_ISBUTTONENABLED = WM_USER + 9,
            TB_ISBUTTONCHECKED = WM_USER + 10,
            TB_ISBUTTONPRESSED = WM_USER + 11,
            TB_ISBUTTONHIDDEN = WM_USER + 12,
            TB_ISBUTTONINDETERMINATE = WM_USER + 13,
            TB_ISBUTTONHIGHLIGHTED = WM_USER + 14,
            TB_SETSTATE = WM_USER + 17,
            TB_GETSTATE = WM_USER + 18,
            TB_ADDBITMAP = WM_USER + 19,
            TB_ADDBUTTONSA = WM_USER + 20,
            TB_INSERTBUTTONA = WM_USER + 21,
            TB_ADDBUTTONS = WM_USER + 20,
            TB_INSERTBUTTON = WM_USER + 21,
            TB_DELETEBUTTON = WM_USER + 22,
            TB_GETBUTTON = WM_USER + 23,
            TB_BUTTONCOUNT = WM_USER + 24,
            TB_COMMANDTOINDEX = WM_USER + 25,
            TB_SAVERESTOREA = WM_USER + 26,
            TB_SAVERESTOREW = WM_USER + 76,
            TB_CUSTOMIZE = WM_USER + 27,
            TB_ADDSTRINGA = WM_USER + 28,
            TB_ADDSTRINGW = WM_USER + 77,
            TB_GETITEMRECT = WM_USER + 29,
            TB_BUTTONSTRUCTSIZE = WM_USER + 30,
            TB_SETBUTTONSIZE = WM_USER + 31,
            TB_SETBITMAPSIZE = WM_USER + 32,
            TB_AUTOSIZE = WM_USER + 33,
            TB_GETTOOLTIPS = WM_USER + 35,
            TB_SETTOOLTIPS = WM_USER + 36,
            TB_SETPARENT = WM_USER + 37,
            TB_SETROWS = WM_USER + 39,
            TB_GETROWS = WM_USER + 40,
            TB_SETCMDID = WM_USER + 42,
            TB_CHANGEBITMAP = WM_USER + 43,
            TB_GETBITMAP = WM_USER + 44,
            TB_GETBUTTONTEXTA = WM_USER + 45,
            TB_GETBUTTONTEXTW = WM_USER + 75,
            TB_REPLACEBITMAP = WM_USER + 46,
            TB_SETINDENT = WM_USER + 47,
            TB_SETIMAGELIST = WM_USER + 48,
            TB_GETIMAGELIST = WM_USER + 49,
            TB_LOADIMAGES = WM_USER + 50,
            TB_GETRECT = WM_USER + 51,

            // wParam is the Cmd instead of index
            TB_SETHOTIMAGELIST = WM_USER + 52,
            TB_GETHOTIMAGELIST = WM_USER + 53,
            TB_SETDISABLEDIMAGELIST = WM_USER + 54,
            TB_GETDISABLEDIMAGELIST = WM_USER + 55,
            TB_SETSTYLE = WM_USER + 56,
            TB_GETSTYLE = WM_USER + 57,
            TB_GETBUTTONSIZE = WM_USER + 58,
            TB_SETBUTTONWIDTH = WM_USER + 59,
            TB_SETMAXTEXTROWS = WM_USER + 60,
            TB_GETTEXTROWS = WM_USER + 61,
            TBSTYLE_BUTTON = 0x0,
            TBSTYLE_SEP = 0x1,
            TBSTYLE_CHECK = 0x2,
            TBSTYLE_GROUP = 0x4,
            TBSTYLE_CHECKGROUP = TBSTYLE_GROUP | TBSTYLE_CHECK,
            TBSTYLE_DROPDOWN = 0x8,
            TBSTYLE_AUTOSIZE = 0x10,

            // automatically calculate the cx of the button '
            TBSTYLE_NOPREFIX = 0x20,

            // If this button should not have accel prefix '
            TBSTYLE_TOOLTIPS = 0x100,
            TBSTYLE_WRAPABLE = 0x200,
            TBSTYLE_ALTDRAG = 0x400,
            TBSTYLE_FLAT = 0x800,
            TBSTYLE_LIST = 0x1000,
            TBSTYLE_CUSTOMERASE = 0x2000,
            TBSTYLE_REGISTERDROP = 0x4000,
            TBSTYLE_TRANSPARENT = 0x8000,
            TBSTYLE_EX_DRAWDDARROWS = 0x1
        }

        #endregion

        #region "Contructors"

        /// <summary>
        ///     Constructor:  Loads the active window and return's it's parent in the case of an MDI form.
        /// </summary>
        public WindowWrapper()
        {
            this.LoadWindow(true);
        }

        /// <summary>
        ///     Construtor:  Loads the active window and gives the option of whether to return it's parent in the case of an MDI form.
        /// </summary>
        /// <param name="returnParent"></param>
        public WindowWrapper(bool returnParent)
        {
            this.LoadWindow(returnParent);
        }

        /// <summary>
        ///     Constructor:  Loads a window based off of the handle specified.
        /// </summary>
        /// <param name="handle"></param>
        public WindowWrapper(IntPtr handle)
        {
            this.LoadWindow(handle);
        }

        /// <summary>
        ///     Constructor:  Loads a window based off of the name of the window.
        /// </summary>
        /// <param name="windowName"></param>
        public WindowWrapper(string windowName)
        {
            this.LoadWindow(windowName);
        }

        #endregion

        #region "Methods"

        /// <summary>
        ///     Loads the Window from the handle.
        /// </summary>
        /// <param name="handle"></param>
        /// <remarks>
        ///     The main logic for LoadWindow is implemented in this function.  All other LoadWindow overloads will call into this function
        ///     after they have obtained the Window handle required based off their parameters.
        /// </remarks>
        public void LoadWindow(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                this.InitializeEmpty();

                return;
            }

            this.Handle = handle;

            // Load the window's text
            int windowLength = GetWindowTextLength(handle);
            string buf = string.Empty.PadRight(windowLength + 1, (char)32);

            GetWindowText(handle, buf, windowLength + 1);
            this.Text = buf.SafeLeft(windowLength);

            RECT r = default;
            r.Left = 0;
            r.Right = 0;
            r.Top = 0;
            r.Bottom = 0;
            GetWindowRect(handle, ref r);

            this.Location = new Point(r.Left, r.Top);
            this.Size = new Size(r.Right - r.Left, r.Bottom - r.Top);

            var sClassName = new StringBuilder("", 256);
            GetClassName(this.Handle, sClassName, 256);
            GetClassName(this.Handle, sClassName, 256);
            this.ClassName = sClassName.ToString();
        }

        /// <summary>
        ///     Loads the active window.
        /// </summary>
        /// <param name="returnParent">If true then the parent form will be loaded in the case of an MDI form.</param>
        public void LoadWindow(bool returnParent)
        {
            var window1 = IntPtr.Zero;
            var window2 = IntPtr.Zero;

            window1 = GetForegroundWindow();

            if (returnParent)
            {
                while (window1 != IntPtr.Zero)
                {
                    window2 = window1;
                    window1 = GetParent(window1);
                }

                window1 = window2;
            }

            this.LoadWindow(window1);
        }

        /// <summary>
        ///     Loads the specified window based on the provided windowText.
        /// </summary>
        /// <param name="windowText"></param>
        public void LoadWindow(string windowText)
        {
            var handle = FindWindow(null, windowText);
            this.LoadWindow(handle);
        }

        /// <summary>
        ///     Loads the specified window based on either the className, the windowText or both.  If one of the two items aren't
        ///     needed then pass a null value in for that parameter.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="windowText"></param>
        public void LoadWindow(string className, string windowText)
        {
            var handle = FindWindow(className, windowText);
            this.LoadWindow(handle);
        }

        /// <summary>
        ///     Initialize properties to an empty value set.
        /// </summary>
        private void InitializeEmpty()
        {
            this.Size = new Size(0, 0);
            this.Location = new Point(0, 0);
            this.Text = "";
            this.Handle = IntPtr.Zero;
        }

        /// <summary>
        ///     Sets the focus on the current window that is loaded and brings it to the foreground.
        /// </summary>
        public void ActivateWindow()
        {
            if (this.Handle != IntPtr.Zero)
            {
                SetForegroundWindowEx(this.Handle);
            }
        }

        /// <summary>
        ///     Minimizes the current window via the CloseWindowAPI.
        /// </summary>
        public void MinimizeWindow()
        {
            if (this.Handle != IntPtr.Zero)
            {
                CloseWindow(this.Handle);
            }
        }

        /// <summary>
        ///     Removes the window and any child windows it contains.
        /// </summary>
        public void DestroyWindow()
        {
            this.SendMessage(Messages.WM_DESTROY, Convert.ToUInt32(0), "");
        }

        /// <summary>
        ///     Sets the window state to the specified command option, this will allow for minimize, maximize, restore, changing of visisbility, etc.
        /// </summary>
        /// <param name="cmd"></param>
        public void SetWindowState(ShowWindowCommand cmd)
        {
            if (this.Handle != IntPtr.Zero)
            {
                ShowWindow(this.Handle, cmd);
            }
        }

        /// <summary>
        ///     Sets the position of the current window on the screen.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public void SetPosition(int x, int y, int height, int width)
        {
            if (this.Handle != IntPtr.Zero)
            {
                SetWindowPos(this.Handle, IntPtr.Zero, x, y, width, height, 0);
            }
        }

        /// <summary>
        ///     Sets the position of the current window on the screen.
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="sz"></param>
        public void SetPosition(Point loc, Size sz)
        {
            this.SetPosition(loc.X, loc.Y, sz.Height, sz.Width);
        }

        /// <summary>
        ///     Sends a mouse click to the current handle.
        /// </summary>
        public void Click()
        {
            SendMessage(this.Handle, (uint) Messages.BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        ///     Enum representing the Left, Middle and Right mouse buttons.
        /// </summary>
        public enum MouseButton
        {
            /// <summary>
            ///     Left Mouse Button
            /// </summary>
            Left,

            /// <summary>
            ///     Middle Mouse Button
            /// </summary>
            Middle,

            /// <summary>
            ///     Right Mouse Button
            /// </summary>
            Right
        }

        /// <summary>
        ///     Sends a mouse down message and a mouse up message at the specified coordinate.  The coordiante is not the coordinate on
        ///     the screen, but where it lays on the form.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Click(int x, int y, MouseButton button)
        {
            var lParam = MakeLParam(x, y);

            switch (button)
            {
                case MouseButton.Left:
                    this.SendMessage(Messages.WM_LBUTTONDOWN, IntPtr.Zero, lParam);
                    this.SendMessage(Messages.WM_LBUTTONUP, IntPtr.Zero, lParam);

                    break;
                case MouseButton.Middle:
                    this.SendMessage(Messages.WM_MBUTTONDOWN, IntPtr.Zero, lParam);
                    this.SendMessage(Messages.WM_MBUTTONUP, IntPtr.Zero, lParam);

                    break;
                case MouseButton.Right:
                    this.SendMessage(Messages.WM_RBUTTONDOWN, IntPtr.Zero, lParam);
                    this.SendMessage(Messages.WM_RBUTTONUP, IntPtr.Zero, lParam);

                    break;
            }
        }

        /// <summary>
        ///     Sets the title of the active window.
        /// </summary>
        /// <param name="windowText"></param>
        public void SetWindowText(string windowText)
        {
            if (this.Handle != IntPtr.Zero)
            {
                SetWindowText(this.Handle, windowText);
            }
        }

        public void SetValue(string value)
        {
            SendMessage(this.Handle, (uint) Messages.WM_SETTEXT, IntPtr.Zero, value);
        }

        public void SendMessage(Messages msg, uint param1, string param2)
        {
            if (this.Handle != IntPtr.Zero)
            {
                SendMessage(this.Handle, (uint) msg, param1, param2);
            }
        }

        public void SendMessage(Messages msg, uint param1, uint param2)
        {
            if (this.Handle != IntPtr.Zero)
            {
                SendMessage(this.Handle, (uint) msg, param1, param2);
            }
        }

        public void SendMessage(Messages msg, IntPtr param1, IntPtr param2)
        {
            if (this.Handle != IntPtr.Zero)
            {
                SendMessage(this.Handle, (uint) msg, param1, param2);
            }
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hWnd, Messages Msg, IntPtr wParam, IntPtr lParam);

        public string GetStringMessage(Messages msg, uint param1)
        {
            if (this.Handle != IntPtr.Zero)
            {
                var sb = new StringBuilder();
                SendMessage(this.Handle, (uint) msg, param1, sb);

                return sb.ToString();
            }

            return "";
        }

        public string GetStringMessage(Messages msg)
        {
            var sb = new StringBuilder();
            SendMessage(this.Handle, (uint) msg, 0, sb);

            return sb.ToString();
        }

        /// <summary>
        ///     Returns the parent handle for the current Window.
        /// </summary>
        public IntPtr GetParentHandle()
        {
            if (this.Handle != IntPtr.Zero)
            {
                return GetParent(this.Handle);
            }

            return IntPtr.Zero;
        }

        /// <summary>
        ///     Gets the handles to the child controls.
        /// </summary>
        public List<IntPtr> GetChildHandles(bool recurse)
        {
            var children = new List<IntPtr>();
            var child = IntPtr.Zero;

            if ((this.Handle == IntPtr.Zero) & this.IgnoreRecursionOnZeroIntPtr)
            {
                return children;
            }

            do
            {
                child = FindWindowEx(this.Handle, child, null, null);

                if ((child != IntPtr.Zero) & (children.Contains(child) == false))
                {
                    children.Add(child);

                    if ((this.Handle == IntPtr.Zero) & this.IgnoreRecursionOnZeroIntPtr)
                    {
                        continue;
                    }

                    if (recurse)
                    {
                        var win = new WindowWrapper(child);
                        children.AddRange(win.GetChildHandles(true));
                    }
                }
                else
                {
                    break; // TODO: might not be correct. Was : Exit Do
                }
            } while (true);

            return children;
        }

        public List<WindowWrapper> GetChildWindows(bool recurse)
        {
            var winList = new List<WindowWrapper>();
            var children = this.GetChildHandles(recurse);

            foreach (var i in children)
            {
                var win = new WindowWrapper(i);
                winList.Add(win);
            }

            return winList;
        }

        public static IntPtr MakeLParam(int loWord, int hiWord)
        {
            return (IntPtr) ((hiWord << 16) | (loWord & 0xffff));
        }

        /// <summary>
        ///     Returns the GETTEXT value from the window/control.
        /// </summary>
        public string GetText()
        {
            if (this.Handle == null || this.Handle == IntPtr.Zero)
            {
                return "";
            }

            // Gets the text length.
            int length = (int) SendMessage(this.Handle, (uint) Messages.WM_GETTEXTLENGTH, IntPtr.Zero, "");

            // Init the string builder to hold the text.
            var sb = new StringBuilder(length + 1);

            // Writes the text from the handle into the StringBuilder
            SendMessage(this.Handle, (uint) Messages.WM_GETTEXT, (IntPtr) sb.Capacity, sb);

            // Return the text as a string.
            return sb.ToString();
        }

        /// <summary>
        ///     Returns the GETTEXT value provided the handle
        /// </summary>
        /// <param name="handle"></param>
        public static string GetText(IntPtr handle)
        {
            if (handle == null || handle == IntPtr.Zero)
            {
                return "";
            }

            // Gets the text length.
            int length = (int) SendMessage(handle, (uint) Messages.WM_GETTEXTLENGTH, IntPtr.Zero, "");

            // Init the string builder to hold the text.
            var sb = new StringBuilder(length + 1);

            // Writes the text from the handle into the StringBuilder
            SendMessage(handle, (uint) Messages.WM_GETTEXT, (IntPtr) sb.Capacity, sb);

            // Return the text as a string.
            return sb.ToString();
        }

        #endregion

        #region "Properties"

        /// <summary>
        ///     Whether or not the recursive features will do so when the Handle value is equal to IntPtr.  In this case, it
        ///     seems to recurse all controls currently in existence.
        /// </summary>
        public bool IgnoreRecursionOnZeroIntPtr { get; set; } = true;

        /// <summary>
        ///     The handle to the currently loaded window.
        /// </summary>
        public IntPtr Handle { get; private set; } = IntPtr.Zero;

        /// <summary>
        ///     The currently loaded Window's caption.
        /// </summary>
        public string Text { get; private set; } = "";

        /// <summary>
        ///     The size of the loaded form.  Use the SetPosition method to change these dimensions.
        /// </summary>
        public Size Size { get; private set; } = new Size(0, 0);

        /// <summary>
        ///     The location of the loaded form.  Use the SetPosition method to change this point.
        /// </summary>
        public Point Location { get; private set; } = new Point(0, 0);

        public string ClassName { get; private set; } = "";

        #endregion
    }
}