using System.Runtime.InteropServices;

namespace MyUtils;

/// <summary>
/// 鼠标键盘工具类
/// </summary>
public class MouseKeyboardUtils
{
    private static Random random = new(Environment.TickCount);

    public static int GetLastError()
    {
        return Marshal.GetLastWin32Error();
    }

    /// <summary>
    /// 获取随机部分
    /// </summary>
    /// <returns></returns>
    private static int GetRandomPart()
    {
        var r = random.NextDouble() / 2;

        var p = new double[] { 0.023, 0.136 };

        if (r < p[0])
        {
            return 0;
        }
        else if (r < p[0] + p[1])
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    /// <summary>
    /// 获取随机坐标
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    private static int GetRandomCoord(int length)
    {
        if (length < 1) return 0;

        length--;

        int part = GetRandomPart();
        if (random.NextDouble() > 0.5) part = 5 - part;

        int coord = (int)Math.Round(((double)length / 6) * random.NextDouble());

        int res = (int)Math.Round((double)part * length / 6 + coord);

        res += random.Next(-5, 5);

        if (res < 0) res = length / 2;
        if (res > length) res = length / 2;

        return res;
    }

    /// <summary>
    /// 随机点
    /// </summary>
    /// <param name="rect"></param>
    /// <returns></returns>
    private static Point GetRandomPoint(Rectangle rect)
    {
        // 我们在这个中心内随机定义一个点
        int x = GetRandomCoord(rect.Width) + rect.X;
        int y = GetRandomCoord(rect.Height) + rect.Y;

        return new System.Drawing.Point(x, y);
    }

    public static void VisualizeRandomPoints(string dest, int tries, int width, int height)
    {
        var rect = new Rectangle(0, 0, width, height);

        int[,] map = new int[width, height];

        int max = 0;
        for (int i = 0; i < tries; i++)
        {
            System.Drawing.Point p = GetRandomPoint(rect);

            int value = ++map[p.X, p.Y];

            if (value > max) max = value;
        }

        Bitmap bmp = new Bitmap(width, height);

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                int color = 255 - (int)(((double)map[x, y] / max) * 255);

                bmp.SetPixel(x, y, Color.FromArgb(color, color, color));
            }

        bmp.Save(dest, System.Drawing.Imaging.ImageFormat.Bmp);
    }

    /// <summary>
    /// 鼠标移动到
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public static void MoveTo(int x, int y)
    {
        SetCursorPos(x, y);
    }

    /// <summary>
    /// 鼠标移动到
    /// </summary>
    /// <param name="windowHandle">窗口句柄</param>
    /// <param name="x">x相对位置</param>
    /// <param name="y">y相对位置</param>
    public static void MoveTo(IntPtr windowHandle, int x, int y)
    {
        if (windowHandle != IntPtr.Zero)
        {
            Rectangle rect = WindowUtils.GetClientRect(windowHandle);

            x += rect.X;
            y += rect.Y;
        }

        SetCursorPos(x, y);
    }

    public static void SetCursorPosition(int x, int y)
    {
        SetCursorPos(x, y);
    }

    /// <summary>
    /// 设置鼠标位置
    /// </summary>
    /// <param name="p"></param>
    public static void SetCursorPosition(System.Drawing.Point p)
    {
        SetCursorPos(p.X, p.Y);
    }

    /// <summary>
    /// 获取鼠标信息
    /// </summary>
    /// <returns></returns>
    public static MouseCursor GetCursorInfo()
    {
        CursorInfo info = new CursorInfo();
        info.cbSize = Marshal.SizeOf(info);
        GetCursorInfo(out info);

        try
        {
            Cursor c = new Cursor(info.hCursor);

            Console.WriteLine(c.Tag);
            return (MouseCursor)info.hCursor.ToInt32();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return MouseCursor.Unknown;
        }
    }

    /// <summary>
    /// 按下鼠标左键
    /// </summary>
    /// <param name="windowHandle">窗口句柄</param>
    /// <param name="time">按住的时间</param>
    /// <param name="x">光标位置（窗口中的相对坐标）</param>
    /// <param name="y">光标位置（窗口中的相对坐标）</param>
    public static void PressLeftMouseButton(IntPtr windowHandle, int time, int x, int y)
    {
        if (windowHandle != IntPtr.Zero)
        {
            Rectangle rect = WindowUtils.GetClientRect(windowHandle);

            x += rect.X;
            y += rect.Y;
        }

        SetCursorPos(x, y);

        PressLeftMouseButton(time);
    }

    /// <summary>
    /// 按下鼠标左键
    /// </summary>
    /// <param name="windowHandle">窗口句柄</param>
    /// <param name="time">按住的时间</param>
    /// <param name="p">光标位置（窗口中的相对坐标）</param>
    public static void PressLeftMouseButton(IntPtr windowHandle, int time, System.Drawing.Point p)
    {
        PressLeftMouseButton(windowHandle, time, p.X, p.Y);
    }

    /// <summary>
    /// 按下鼠标左键
    /// </summary>
    /// <param name="time">按住的时间</param>
    /// <param name="x">光标位置x</param>
    /// <param name="y">光标位置y</param>
    public static void PressLeftMouseButton(int time, int x, int y)
    {
        PressLeftMouseButton(IntPtr.Zero, time, x, y);
    }

    /// <summary>
    /// 按下鼠标左键
    /// </summary>
    /// <param name="time">按住的时间</param>
    /// <param name="p">光标位置</param>
    public static void PressLeftMouseButton(int time, System.Drawing.Point p)
    {
        SetCursorPosition(p);

        PressLeftMouseButton(time);
    }

    /// <summary>
    /// 按下鼠标左键
    /// </summary>
    /// <param name="p">光标位置</param>
    public static void PressLeftMouseButton(System.Drawing.Point p)
    {
        SetCursorPosition(p);

        PressLeftMouseButton(0);
    }

    /// <summary>
    /// 按下鼠标中键
    /// </summary>
    /// <param name="p">光标位置</param>
    public static void PressMiddleMouseButton(System.Drawing.Point p)
    {
        SetCursorPosition(p);

        PressMiddleMouseButton(0);
    }

    /// <summary>
    /// 拖动
    /// </summary>
    /// <param name="p1">起点</param>
    /// <param name="p2">终点</param>
    public static void Drag(System.Drawing.Point p1, System.Drawing.Point p2)
    {
        SetCursorPosition(p1);

        Thread.Sleep(200);

        mouse_event(MouseFlags.Absolute | MouseFlags.LeftDown, p1.X, p1.Y, 0, new IntPtr());

        System.Drawing.Point cursor = new System.Drawing.Point();

        GetCursorPos(ref cursor);

        while (cursor != p2)
        {
            cursor = new System.Drawing.Point(cursor.X + Math.Sign(p2.X - cursor.X), cursor.Y + Math.Sign(p2.Y - cursor.Y));

            SetCursorPosition(cursor);
        }

        //Thread.Sleep(50);

        mouse_event(MouseFlags.Absolute | MouseFlags.LeftUp, p2.X, p2.Y, 0, new IntPtr());

        //Thread.Sleep(50);
    }

    /// <summary>
    /// 拖动
    /// </summary>
    /// <param name="r1">起点</param>
    /// <param name="r2">终点</param>
    public static void Drag(Rectangle r1, Rectangle r2)
    {
        System.Drawing.Point p1 = GetRandomPoint(r1);
        System.Drawing.Point p2 = GetRandomPoint(r2);

        Drag(p1, p2);
    }

    /// <summary>
    /// 拖动
    /// </summary>
    /// <param name="p1">起点</param>
    /// <param name="dx">终点x</param>
    /// <param name="dy">终点y</param>
    public static void Drag(System.Drawing.Point p1, int dx, int dy)
    {
        System.Drawing.Point p2 = new System.Drawing.Point(p1.X + dx, p1.Y + dy);

        Drag(p1, p2);
    }

    /// <summary>
    /// 移到矩形
    /// </summary>
    /// <param name="rect">矩形范围</param>
    public static void MoveToRect(Rectangle rect)
    {
        var point = GetRandomPoint(rect);

        SetCursorPos(point.X, point.Y);
    }

    /// <summary>
    /// “到达”光标到指定的矩形
    /// </summary>
    /// <param name="rect">矩形</param>
    /// <param name="clicks">单击次数</param>
    /// <param name="left">true左键, false右键</param>
    public static void ClickOnRect(Rectangle rect, int clicks, bool left)
    {
        var point = GetRandomPoint(rect);

        SetCursorPos(point.X, point.Y);

        // 如果你需要做不止一次
        for (int i = 0; i < clicks; i++)
        {
            // 延迟 100 毫秒
            Thread.Sleep(100);
            // 按
            if (left)
            {
                PressLeftMouseButton(0);
            }
            else
            {
                PressRightMouseButton(0);
            }
        }
    }

    public static void ClickOnRect(int x, int y)
    {
        ClickOnRect(new Rectangle(x, y, 2, 2));
    }

    public static void ClickOnRect(Rectangle rect)
    {
        ClickOnRect(rect, 1, true);
    }

    /// <summary>点击矩形中随机坐标</summary>
    /// <param name="rect">长方形</param>
    /// <param name="clicks">移动后鼠标点击次数</param>
    /// <remarks>** 未测试 **</remarks>
    public static void ClickOnRect(Rectangle rect, int clicks)
    {
        ClickOnRect(rect, clicks, true);
    }

    /// <summary>点击矩形中随机坐标</summary>
    /// <param name="box">长方形</param>
    /// <param name="speed">移动速度</param>
    /// <param name="clicks">移动后鼠标点击次数</param>
    /// <remarks>** 未测试 **</remarks>
    public static void ClickOnRect(IntPtr windowHandle, Rectangle rect, int clicks)
    {
        Rectangle winRect = WindowUtils.GetClientRect(windowHandle);

        Rectangle clickRect = new Rectangle(rect.X + winRect.X, rect.Y + winRect.Y, rect.Width, rect.Height);

        ClickOnRect(clickRect, clicks);
    }

    public static bool ClickOnButton(string processName, string buttonText, bool exact)
    {
        List<IntPtr> wins = WindowSearchUtils.GetAllWindowsByText(processName, buttonText, exact);
        wins.AddRange(WindowSearchUtils.GetAllWindowsByCaption(processName, buttonText, exact));
        wins = wins.Distinct().ToList();

        if (wins.Count == 0) return false;

        IntPtr windowHandle = wins.Last();

        //WindowManipulation.SetForegroundWindow(windowHandle);

        Thread.Sleep(500);

        //WindowManipulation.SetForegroundWindow(windowHandle);

        Rectangle rect = WindowUtils.GetWindowRect(windowHandle);

        rect.Location = new System.Drawing.Point(rect.X + 7, rect.Y + 7);
        rect.Size = new Size(rect.Width - 10, rect.Height - 10);

        ClickOnRect(rect, 1);

        Thread.Sleep(200);

        return true;
    }

    public static void LeftClick()
    {
        PressLeftMouseButton(50);
    }

    public static void LeftDoubleClick()
    {
        PressLeftMouseButton(20);
        Thread.Sleep(50);
        PressLeftMouseButton(20);
    }

    public static void PressLeftMouseButton()
    {
        PressLeftMouseButton(50);
    }

    /// <summary>
    /// 按下鼠标左键
    /// </summary>
    /// <param name="time">按住的时间</param>
    public static void PressLeftMouseButton(int time)
    {
        // 我们取光标的坐标
        System.Drawing.Point point = new System.Drawing.Point();
        GetCursorPos(ref point);

        // 单击鼠标左键
        mouse_event(MouseFlags.Absolute | MouseFlags.LeftDown, point.X, point.Y, 0, new IntPtr());

        // 如有必要，请延迟
        if (time > 0) Thread.Sleep(time);

        // 松开按钮
        mouse_event(MouseFlags.Absolute | MouseFlags.LeftUp, point.X, point.Y, 0, new IntPtr());
    }

    /// <summary>
    ///  按下鼠标中键
    /// </summary>
    /// <param name="time"></param>
    public static void PressMiddleMouseButton(int time)
    {
        System.Drawing.Point point = new System.Drawing.Point();
        GetCursorPos(ref point);

        mouse_event(MouseFlags.Absolute | MouseFlags.MiddleDown, point.X, point.Y, 0, new IntPtr());

        if (time > 0) Thread.Sleep(time);

        mouse_event(MouseFlags.Absolute | MouseFlags.MiddleUp, point.X, point.Y, 0, new IntPtr());
    }

    public static void RightClick()
    {
        PressRightMouseButton(50);
    }

    public static void RightDoubleClick()
    {
        PressRightMouseButton(20);
        Thread.Sleep(50);
        PressRightMouseButton(20);
    }

    /// <summary>
    /// 按下鼠标右键
    /// </summary>
    /// <param name="windowHandle"></param>
    /// <param name="time"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public static void PressRightMouseButton(IntPtr windowHandle, int time, int x, int y)
    {
        if (windowHandle != IntPtr.Zero)
        {
            Rectangle rect = WindowUtils.GetClientRect(windowHandle);

            x += rect.X;
            y += rect.Y;
        }

        SetCursorPos(x, y);

        PressRightMouseButton(time);
    }

    /// <summary>
    /// 按下鼠标右键
    /// </summary>
    /// <param name="time"></param>
    public static void PressRightMouseButton(int time)
    {
        // 我们取光标的坐标
        System.Drawing.Point point = new System.Drawing.Point();
        GetCursorPos(ref point);

        // 单击鼠标左键
        mouse_event(MouseFlags.Absolute | MouseFlags.RightDown, point.X, point.Y, 0, new IntPtr());

        // 如有必要，请延迟
        if (time > 0) Thread.Sleep(time);

        // 松开按钮
        mouse_event(MouseFlags.Absolute | MouseFlags.RightUp, point.X, point.Y, 0, new IntPtr());
    }

    /// <summary> 按下键盘键</summary>
    /// <param name = "key">键码</param>
    /// <param name = "shift"> 按下 shift </param>
    /// <param name = "alt"> 按下 alt </param>
    /// <param name = "ctrl"> Ctrl </param>
    /// <param name = "holdFor"> 按住键多少毫秒 </param>
    /// <备注> ** 未测试 ** </备注>
    public static void PressKey(int key, bool shift, bool alt, bool ctrl, int holdFor)
    {
        // 如果我们按下 PrintScreen，则 bScan = 0
        byte bScan = 0x45;
        if (key == (int)KeyEnum.VK_SNAPSHOT) bScan = 0;
        if (key == (int)KeyEnum.VK_SPACE) bScan = 39;

        // 如有必要，按住 alt、ctrl 或 shift
        if (alt) keybd_event((int)KeyEnum.VK_MENU, 0, 0, 0);
        if (ctrl) keybd_event((int)KeyEnum.VK_LCONTROL, 0, 0, 0);
        if (shift) keybd_event((int)KeyEnum.VK_LSHIFT, 0, 0, 0);

        //按下指定的键
        keybd_event((int)key, bScan, KEYEVENTF_EXTENDEDKEY, 0);

        if (holdFor > 0) Thread.Sleep(holdFor);

        // 让她走
        keybd_event((int)key, bScan, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);

        // 如果有任何键被按下，松开它们
        if (shift) keybd_event((int)KeyEnum.VK_LSHIFT, 0, KEYEVENTF_KEYUP, 0);
        if (ctrl) keybd_event((int)KeyEnum.VK_LCONTROL, 0, KEYEVENTF_KEYUP, 0);
        if (alt) keybd_event((int)KeyEnum.VK_MENU, 0, KEYEVENTF_KEYUP, 0);
    }

    public static void PressKey(KeyEnum key, bool shift, bool alt, bool ctrl)
    {
        PressKey((int)key, shift, alt, ctrl, 0);
    }

    /// <summary> 按下键盘键</summary>
    /// <param name = "key">键码</param>
    /// <备注> ** 未测试 ** </备注>
    public static void PressKey(KeyEnum key)
    {
        PressKey(key, false, false, false);
    }

    public static void PressKey(int key, bool shift, bool alt, bool ctrl)
    {
        PressKey(key, false, false, false, 0);
    }

    public static void Paste(string text, int delay)
    {
        Paste(text, delay, true);
    }

    public static void Paste(string text, int delay, bool withBackup)
    {
        if (text == "" || text == null) return;

        string backup = null;

        if (withBackup)
        {
            backup = Clipboard.GetText();

            Thread.Sleep(delay);
        }

        Clipboard.SetText(text);

        Thread.Sleep(delay);

        PressKey((int)'V', false, false, true);

        Thread.Sleep(delay);

        if (withBackup) Clipboard.SetText(backup);
    }

    public static void Type(string text, int delay)
    {
        Type(text, delay, delay);
    }

    public static void Type(string text, int delayFrom, int delayTo)
    {
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];

            bool shift = char.IsUpper(c);

            int key;
            if (char.IsLetterOrDigit(c))
            {
                key = (int)char.ToUpper(c);
            }
            else
            {
                if (c == '.' || c == ',')
                {
                    key = (int)KeyEnum.VK_DECIMAL;
                }
                else
                {
                    continue;
                }
            }

            PressKey(key, shift, false, false);

            int delay = random.Next(delayFrom, delayTo);

            Thread.Sleep(delay);
        }
    }

    public static void Scroll(int scrollValue)
    {
        mouse_event(MouseFlags.Scroll, 0, 0, scrollValue, new IntPtr());
    }

    #region API

    [Flags]
    public enum MouseFlags
    {
        Move = 0x0001, LeftDown = 0x0002, LeftUp = 0x0004, RightDown = 0x0008, RightUp = 0x0010, Absolute = 0x8000, MiddleDown = 0x0020, MiddleUp = 0x0040, Scroll = 2048
    };

    public enum MouseCursor
    { Unknown = 0, Arrow = 65539, Text = 65541 };

    public const UInt32 KEYEVENTF_EXTENDEDKEY = 1;
    public const UInt32 KEYEVENTF_KEYUP = 2;
    public const int KEY_ALT = 0x12;
    public const int KEY_CONTROL = 0x11;

    public enum WMessages : int
    {
        WM_LBUTTONDOWN = 0x201, //Left mousebutton down
        WM_LBUTTONUP = 0x202,  //Left mousebutton up
        WM_LBUTTONDBLCLK = 0x203, //Left mousebutton doubleclick
        WM_RBUTTONDOWN = 0x204, //Right mousebutton down
        WM_RBUTTONUP = 0x205,   //Right mousebutton up
        WM_RBUTTONDBLCLK = 0x206, //Right mousebutton doubleclick
        WM_KEYDOWN = 0x100,  //Key down
        WM_KEYUP = 0x101,   //Key up
    }

    /// <summary> 键盘控制功能</summary>
    /// <param name = "bVk">关键代码</param>
    /// <param name = "bScan">扫描</param>
    /// <param name = "dwFlags">动作类型</param>
    /// <param name = "dwExtraInfo">附加数据</param>
    [DllImport("user32.dll")]
    private static extern void keybd_event(int bVk, byte bScan, UInt32 dwFlags, int dwExtraInfo);

    /// <summary> 获取光标坐标</summary>
    /// <param name = "lpPoint">光标坐标</param>
    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);

    /// <summary> 指定光标坐标</summary>
    /// <param name = "x"> X 坐标 </param>
    /// <param name = "y"> Y 坐标</param>
    [DllImport("user32.dll")]
    private static extern void SetCursorPos(int x, int y);

    /// <summary> 鼠标控制函数</summary>
    /// <param name = "dwFlags">动作类型</param>
    /// <param name = "dx"> X 坐标 </param>
    /// <param name = "dy"> Y 坐标</param>
    /// <param name = "dwData">数据</param>
    /// <param name = "dwExtraInfo">附加数据</param>
    [DllImport("user32.dll")]
    private static extern void mouse_event(MouseFlags dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);

    [DllImport("user32.dll")]
    private static extern bool GetCursorInfo(out CursorInfo pci);

    [StructLayout(LayoutKind.Sequential)]
    private struct MyPoint
    {
        public Int32 x;
        public Int32 y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct CursorInfo
    {
        public Int32 cbSize;        // Specifies the size, in bytes, of the structure.

        // The caller must set this to Marshal.SizeOf(typeof(CURSORINFO)).
        public Int32 flags;         // Specifies the cursor state. This parameter can be one of the following values:

        //    0             The cursor is hidden.
        //    CURSOR_SHOWING    The cursor is showing.
        public IntPtr hCursor;          // Handle to the cursor.

        public MyPoint ptScreenPos;       // A POINT structure that receives the screen coordinates of the cursor.
    }

    public enum KeyEnum
    {
        VK_LBUTTON = 1, VK_RBUTTON = 2, VK_CANCEL = 3, VK_MBUTTON = 4,
        VK_BACK = 8, VK_TAB = 9, VK_CLEAR = 12, VK_RETURN = 13,
        VK_SHIFT = 10, VK_CONTROL = 17, VK_MENU = 18, VK_PAUSE = 19,
        VK_CAPITAL = 20, VK_ESCAPE = 27, VK_SPACE = 32, VK_PRIOR = 33,
        VK_NEXT = 34, VK_END = 35, VK_HOME = 36, VK_LEFT = 37,
        VK_UP = 38, VK_RIGHT = 39, VK_DOWN = 40, VK_SELECT = 41,
        VK_PRINT = 42, VK_EXECUTE = 43, VK_SNAPSHOT = 44, VK_INSERT = 45,
        VK_DELETE = 46, VK_HELP = 47, VK_LWIN = 91, VK_RWIN = 92,
        VK_APPS = 93, VK_NUMPAD0 = 96, VK_NUMPAD1 = 97, VK_NUMPAD2 = 98,
        VK_NUMPAD3 = 99, VK_NUMPAD4 = 100, VK_NUMPAD5 = 101, VK_NUMPAD6 = 102,
        VK_NUMPAD7 = 103, VK_NUMPAD8 = 104, VK_NUMPAD9 = 105, VK_MULTIPLY = 106,
        VK_ADD = 107, VK_SEPARATOR = 108, VK_SUBTRACT = 109, VK_DECIMAL = 110,
        VK_DIVIDE = 111, VK_F1 = 112, VK_F2 = 113, VK_F3 = 114,
        VK_F4 = 115, VK_F5 = 116, VK_F6 = 117, VK_F7 = 118,
        VK_F8 = 119, VK_F9 = 120, VK_F10 = 121, VK_F11 = 122,
        VK_F12 = 123, VK_F13 = 124, VK_F14 = 125, VK_F15 = 126,
        VK_F16 = 127, VK_F17 = 128, VK_F18 = 129, VK_F19 = 130,
        VK_F20 = 131, VK_F21 = 132, VK_F22 = 133, VK_F23 = 134,
        VK_F24 = 135, VK_NUMLOCK = 144, VK_SCROLL = 145, VK_LSHIFT = 160,
        VK_RSHIFT = 161, VK_LCONTROL = 162, VK_RCONTROL = 163, VK_LMENU = 164,
        VK_RMENU = 165, VK_PROCESSKEY = 229, VK_ATTN = 246, VK_CRSEL = 247,
        VK_EXSEL = 248, VK_EREOF = 249, VK_PLAY = 250, VK_ZOOM = 251,
        VK_NONAME = 252, VK_PA1 = 253, VK_OEM_CLEAR = 254,
    }

    #endregion API
}