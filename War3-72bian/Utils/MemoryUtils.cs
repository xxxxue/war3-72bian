using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace MyUtils;

public class MemoryUtils
{
    #region API

    //从指定内存中读取字节集数据
    [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
    private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, IntPtr lpNumberOfBytesRead);

    //从指定内存中写入字节集数据
    [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
    private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, IntPtr lpNumberOfBytesWritten);

    //打开一个已存在的进程对象，并返回进程的句柄
    [DllImport("kernel32.dll", EntryPoint = "OpenProcess")]
    private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    //关闭一个内核对象。其中包括文件、文件映射、进程、线程、安全和同步对象等。
    [DllImport("kernel32.dll")]
    private static extern void CloseHandle(IntPtr hObject);

    #endregion

    // byte		(字节   1字节)
    // int16    (short 2字节),
    // int32    (int   4字节),
    // int64    (long  8字节)
    // float    (单浮点 Single 4字节
    // double   (双浮点 8字节)
    // string	(字符串)
    // byte[]	(字节数组)

    private IntPtr _handle = IntPtr.Zero;
    private int _pid = 0;
    public MemoryUtils(int pid)
    {
        _pid = pid;
        _handle = OpenProcess(0x1F0FFF, false, pid);
    }

    ~MemoryUtils()
    {
        CloseHandle(_handle);
    }

    #region Read
    public byte[] ReadToBytes(IntPtr address, int size)
    {
        byte[] buffer = new byte[size];
        ReadProcessMemory(_handle, address, buffer, size, IntPtr.Zero);
        return buffer;
    }
    public T ReadObject<T>(IntPtr address) where T : struct
    {
        var buffer = ReadToBytes(address, Marshal.SizeOf(typeof(T)));

        var bufferAddress = Marshal.AllocHGlobal(buffer.Length);

        Marshal.Copy(buffer, 0, bufferAddress, buffer.Length);

        var structure = (T)Marshal.PtrToStructure(bufferAddress, typeof(T));

        Marshal.FreeHGlobal(bufferAddress);

        return structure;
    }

    public char ReadToChar(IntPtr address)
    {
        byte[] buffer = ReadToBytes(address, sizeof(char));
        return BitConverter.ToChar(buffer, 0);
    }
    public short ReadToShort(IntPtr address)
    {
        byte[] buffer = ReadToBytes(address, sizeof(short));
        return BitConverter.ToInt16(buffer, 0);
    }

    public int ReadToInt(IntPtr address)
    {
        byte[] buffer = ReadToBytes(address, sizeof(int));

        return BitConverter.ToInt32(buffer, 0);
    }

    public long ReadToLong(IntPtr address)
    {
        byte[] buffer = ReadToBytes(address, sizeof(long));
        return BitConverter.ToInt64(buffer, 0);
    }

    public float ReadToFloat(IntPtr address)
    {
        byte[] buffer = ReadToBytes(address, sizeof(float));
        return BitConverter.ToSingle(buffer, 0);
    }

    public double ReadToDouble(IntPtr address)
    {
        byte[] buffer = ReadToBytes(address, sizeof(double));
        return BitConverter.ToDouble(buffer, 0);
    }

    public string ReadToString(IntPtr address, int stringSize)
    {
        byte[] buffer = ReadToBytes(address, stringSize);
        return BitConverter.ToString(buffer);
    }

    #endregion

    #region Write

    public bool WriteByteArray(IntPtr address, byte[] byteData)
    {
        return WriteProcessMemory(_handle, address, byteData, byteData.Length, IntPtr.Zero);
    }

    public bool WriteChar(IntPtr address, char value)
    {
        return WriteByteArray(address, BitConverter.GetBytes(value));
    }

    public bool WriteShort(IntPtr address, short value)
    {
        return WriteByteArray(address, BitConverter.GetBytes(value));
    }

    public bool WriteInt(IntPtr address, int value)
    {
        return WriteByteArray(address, BitConverter.GetBytes(value));
    }

    public bool WriteLong(IntPtr address, long value)
    {
        return WriteByteArray(address, BitConverter.GetBytes(value));
    }

    public bool WriteFloat(IntPtr address, float value)
    {
        return WriteByteArray(address, BitConverter.GetBytes(value));
    }

    public bool WriteDouble(IntPtr address, double value)
    {
        return WriteByteArray(address, BitConverter.GetBytes(value));
    }

    public bool WriteString(IntPtr address, string value)
    {
        return WriteByteArray(address, Encoding.Default.GetBytes(value));
    }

    #endregion

    #region Utils

    /// <summary>
    /// 根据 进程名 获取 PID
    /// </summary>    
    public static int GetPidByProcessName(string processName)
    {
        return GetProcessByProcessName(processName)?.Id ?? 0;
    }

    /// <summary>
    /// 通过 进程名(不加exe后缀) 获取 进程对象
    /// </summary>      
    public static Process GetProcessByProcessName(string processName)
    {
        var processArr = Process.GetProcessesByName(processName);
        if (processArr.Length > 0)
        {
            return processArr[0];
        }

        return null;
    }

    /// <summary>
    /// 根据窗体标题查找窗口句柄（支持模糊匹配）
    /// </summary>    
    public static IntPtr FindWindow(string title)
    {
        var processArray = Process.GetProcesses();
        foreach (var item in processArray)
        {
            if (item.MainWindowTitle.IndexOf(title) != -1)
            {
                return item.MainWindowHandle;
            }
        }

        return IntPtr.Zero;
    }

    /// <summary>
    /// 获取进程中模块的基址 (例如: Game.dll / War3.exe)
    /// </summary>  
    public IntPtr GetModuleBaseAddress(string moduleName)
    {
        var process = Process.GetProcessById(_pid);
        IntPtr baseAddress = default;

        for (int i = 0; i < process.Modules.Count; i++)
        {
            var item = process.Modules[i];
            if (item.ModuleName == moduleName)
            {
                baseAddress = item.BaseAddress;
                break;
            }
        }

        return baseAddress;
    }

    /// <summary>
    /// 计算地址偏移
    /// </summary>   
    public IntPtr GetMemoryAddress(string moduleName, params IntPtr[] offsetArray)
    {
        if ((offsetArray?.Length ?? 0) == 0)
        {
            throw new Exception("至少需要一个偏移");
        }

        IntPtr addr = IntPtr.Zero;

        // 模块的地址
        var addrVal = GetModuleBaseAddress(moduleName);

        // 计算剩下的多级偏移
        for (int i = 0; i < offsetArray.Length; i++)
        {
            addr = addrVal + offsetArray[i];
            addrVal = (IntPtr)ReadToInt(addr);
        }

        // 最终的地址 (只是一个地址, 需要手动去读里面的值)
        return addr;
    }

    /// <summary>
    /// 通过进程名 获取窗口句柄
    /// </summary>
    /// <param name="processName"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static IntPtr GetWindowHwndByProcessName(string processName)
    {
        var hwnd = MemoryUtils.GetProcessByProcessName(processName)?.MainWindowHandle ?? IntPtr.Zero;

        if (hwnd == IntPtr.Zero)
        {
            throw new Exception($"没有找到[{processName}]进程..");
        }
        return hwnd;
    }

    #endregion

    #region 进制转换

    /// <summary>
    /// 16进制(0x41)转为10进制(65)
    /// </summary>   
    public static int ConvertFrom16To10(string value)
    {
        return Convert.ToInt32(value, 16);
    }

    /// <summary>
    /// 16进制(0x41)转为10进制(65)
    /// </summary>    
    public static int ConvertFrom16To10(IntPtr value)
    {
        return Convert.ToInt32(Convert.ToString(value, 10));
    }

    /// <summary>
    /// 10进制(65)转为16进制(0x41)
    /// </summary>   
    public static string ConvertFrom10To16(IntPtr value)
    {
        //x4 0补齐4位
        //x8 0补齐8位
        return value.ToString("x");
    }

    /// <summary>
    /// 16/10进制转为2进制
    /// </summary>     
    public static string ConvertFrom16Or10To2(IntPtr value)
    {
        return Convert.ToString(value, 2);
    }

    /// <summary>
    /// 2进制(1010)到10进制(2)
    /// </summary>    
    public static int ConvertFrom2To10(string value)
    {
        return Convert.ToInt32(value, 2);
    }

    #endregion

}
