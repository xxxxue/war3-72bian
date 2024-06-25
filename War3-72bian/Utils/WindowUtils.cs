using System.Runtime.InteropServices;
using System.Text;

namespace MyUtils;

/// <summary>
///  窗口工具类
/// </summary>
public class WindowUtils
{
    public static bool Enabled(IntPtr win)
    {
        return !GetStyles(win).Contains(WindowStyles.WS_DISABLED);
    }

    /// <summary> 表示指定的窗口 </summary> 是否存在
    /// <param name = "win">窗口地址</param>
    public static bool Exists(IntPtr win)
    {
        return GetProcessId(win) != 0;
    }

    /// <summary>
    /// 聚焦窗口
    /// </summary>
    /// <param name="hWnd">句柄</param>
    /// <returns></returns>
    public static bool FocuseWindow(IntPtr hWnd)
    {
        return SetForegroundWindow(hWnd);
    }

    public static Icon GetAppIcon(IntPtr hwnd)
    {
        IntPtr iconHandle = SendMessage(hwnd, WM_GETICON, ICON_SMALL2, null);
        if (iconHandle == IntPtr.Zero)
            iconHandle = SendMessage(hwnd, WM_GETICON, ICON_SMALL, null);
        if (iconHandle == IntPtr.Zero)
            iconHandle = SendMessage(hwnd, WM_GETICON, ICON_BIG, null);
        if (iconHandle == IntPtr.Zero)
            iconHandle = GetClassLongPtr(hwnd, GCL_HICON);
        if (iconHandle == IntPtr.Zero)
            iconHandle = GetClassLongPtr(hwnd, GCL_HICONSM);

        if (iconHandle == IntPtr.Zero)
            return null;

        Icon icn = Icon.FromHandle(iconHandle);

        return icn;
    }

    public static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
    {
        if (IntPtr.Size > 4) return GetClassLongPtr64(hWnd, nIndex);
        else return new IntPtr(GetClassLongPtr32(hWnd, nIndex));
    }

    /// <summary> 返回指定窗口的客户区矩形</summary>
    /// <param name = "win">窗口地址</param>
    public static Rectangle GetClientRect(IntPtr win)
    {
        MyRect rect = new MyRect();

        GetClientRect(win, out rect);

        Rectangle result = new Rectangle();
        POINT p = new POINT(0, 0);
        ClientToScreen(win, ref p);

        result.Location = p;
        result.Width = rect.Right - rect.Left;
        result.Height = rect.Bottom - rect.Top;

        return result;
    }

    /// <summary> 返回活动窗口的标题 </summary>
    public static string GetForegroundWindowText()
    {
        var win = GetForegroundWindow();
        return GetWindowText(win);
    }

    /// <summary> 返回该窗口所属进程的ID </summary>
    /// <param name = "win">窗口地址</param>
    public static int GetProcessId(IntPtr win)
    {
        int id = 0;

        GetWindowThreadProcessId(win, ref id);

        return id;
    }

    public static List<WindowStyles> GetStyles(IntPtr win)
    {
        int style = GetWindowLong(win, GWL_STYLE);

        List<WindowStyles> result = new List<WindowStyles>();

        foreach (var ws in Enum.GetValues(typeof(WindowStyles)))
        {
            if ((style & (uint)ws) != 0) result.Add((WindowStyles)ws);
        }

        return result;
    }

    public static string GetWindowCaption(IntPtr hWnd)
    {
        StringBuilder buff = new StringBuilder(256);
        //Get the text from the active window into the stringbuilder
        SendMessage(hWnd, WM_GETTEXT, 256, buff);
        return buff.ToString();
    }

    /// <summary> 返回窗口类的名称</summary>
    /// <param name = "win">窗口地址</param>
    public static string GetWindowClass(IntPtr win)
    {
        if (win == IntPtr.Zero) return "";

        StringBuilder title = new StringBuilder(512);
        RealGetWindowClass(win, title, 512);

        return title.ToString().Trim();
    }

    /// <summary> 返回指定窗口的矩形</summary>
    /// <param name = "win">窗口地址</param>
    public static Rectangle GetWindowRect(IntPtr win)
    {
        MyRect rect = new MyRect();

        GetWindowRect(win, out rect);

        Rectangle result = new Rectangle();

        result.X = rect.Left;
        result.Y = rect.Top;
        result.Width = rect.Right - rect.Left;
        result.Height = rect.Bottom - rect.Top;

        return result;
    }

    /// <summary> 返回窗口标题</summary>
    /// <param name = "hWnd">窗口地址</param>
    public static string GetWindowText(IntPtr hWnd)
    {
        StringBuilder buff = new StringBuilder(256);
        GetWindowText(hWnd, buff, 256);

        return buff.ToString();
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static implicit operator Point(POINT p)
        {
            return new Point(p.X, p.Y);
        }

        public static implicit operator POINT(Point p)
        {
            return new POINT(p.X, p.Y);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MyRect
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    }

    #region User32.dll

    private const int GCL_HICON = -14;
    private const int GCL_HICONSM = -34;
    private const int GWL_STYLE = -16;
    private const int ICON_BIG = 1;
    private const int ICON_SMALL = 0;
    private const int ICON_SMALL2 = 2;
    private const int WM_GETICON = 0x7F;
    private const int WM_GETTEXT = 13;

    [Flags]
    public enum WindowStyles : uint
    {
        WS_OVERLAPPED = 0x00000000,
        WS_POPUP = 0x80000000,
        WS_CHILD = 0x40000000,
        WS_MINIMIZE = 0x20000000,
        WS_VISIBLE = 0x10000000,
        WS_DISABLED = 0x08000000,
        WS_CLIPSIBLINGS = 0x04000000,
        WS_CLIPCHILDREN = 0x02000000,
        WS_MAXIMIZE = 0x01000000,
        WS_BORDER = 0x00800000,
        WS_DLGFRAME = 0x00400000,
        WS_VSCROLL = 0x00200000,
        WS_HSCROLL = 0x00100000,
        WS_SYSMENU = 0x00080000,
        WS_THICKFRAME = 0x00040000,
        WS_GROUP = 0x00020000,
        WS_TABSTOP = 0x00010000,

        WS_MINIMIZEBOX = 0x00020000,
        WS_MAXIMIZEBOX = 0x00010000
    }

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowVisible(IntPtr win);

    [DllImport("user32.dll")]
    private static extern bool ClientToScreen(IntPtr win, ref POINT point);

    [DllImport("user32.dll", EntryPoint = "GetClassLong")]
    private static extern uint GetClassLongPtr32(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
    private static extern IntPtr GetClassLongPtr64(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern bool GetClientRect(IntPtr hWnd, out MyRect lpRect);

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(IntPtr win, out MyRect rect);

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll")]
    private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, ref int processId);

    [DllImport("user32.dll")]
    private static extern uint RealGetWindowClass(IntPtr win, StringBuilder pszType, uint cchType);

    [DllImport("user32.dll")]
    private static extern bool ScreenToClient(IntPtr win, ref POINT point);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, StringBuilder lParam);

    /// <summary>
    ///  该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。
    ///  系统给创建前台窗口的线程分配的权限稍高于其他线程。
    /// </summary>
    /// <param name="hWnd">将被激活并被调入前台的窗口句柄</param>
    /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>
    [DllImport("User32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    #endregion User32.dll
}