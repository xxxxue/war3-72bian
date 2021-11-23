using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MyUtils
{
    public static class MemoryUtils
    {
        #region API

        //从指定内存中读取字节集数据
        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, IntPtr lpNumberOfBytesRead);

        //从指定内存中写入字节集数据
        [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, int[] lpBuffer, int nSize, IntPtr lpNumberOfBytesWritten);

        //打开一个已存在的进程对象，并返回进程的句柄
        [DllImport("kernel32.dll", EntryPoint = "OpenProcess")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        //关闭一个内核对象。其中包括文件、文件映射、进程、线程、安全和同步对象等。
        [DllImport("kernel32.dll")]
        private static extern void CloseHandle(IntPtr hObject);

        #endregion

        #region 使用方法

        /// <summary>
        /// 根据 进程名 获取 PID
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static int GetPidByProcessName(string processName)
        {
            return GetProcessByProcessName(processName)?.Id ?? 0;
        }

        /// <summary>
        /// 通过 进程名 获取 进程对象
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
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
        /// 读取内存
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static IntPtr ReadMemory(int baseAddress, int pid)
        {
            byte[] buffer = new byte[4];
            //获取缓冲区地址
            IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
            //打开一个已存在的进程对象  0x1F0FFF 最高权限
            IntPtr hProcess = OpenProcess(0x1F0FFF, false, pid);
            //将制定内存中的值读入缓冲区
            ReadProcessMemory(hProcess, (IntPtr)baseAddress, byteAddress, 4, IntPtr.Zero);
            //关闭操作
            CloseHandle(hProcess);
            return byteAddress;
        }


        /// <summary>
        /// 读取内存中的值 int32
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static int ReadMemoryValueToInt32(int baseAddress, int pid)
        {
            try
            {
                var byteAddress = ReadMemory(baseAddress, pid);
                //从非托管内存中读取一个 32 位带符号整数。
                return Marshal.ReadInt32(byteAddress);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }


        /// <summary>
        /// 读取内存中的值 int64
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static long ReadMemoryValueToInt64(int baseAddress, int pid)
        {
            try
            {
                var byteAddress = ReadMemory(baseAddress, pid);
                //从非托管内存中读取一个 64 位带符号整数。
                return Marshal.ReadInt64(byteAddress);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }

        /// <summary>
        /// 将值写入指定内存地址中
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="pid"></param>
        /// <param name="value"></param>        
        public static void WriteMemoryValue(int baseAddress, int pid, int value)
        {
            try
            {
                //打开一个已存在的进程对象  0x1F0FFF 最高权限
                IntPtr hProcess = OpenProcess(0x1F0FFF, false, pid);
                //从指定内存中写入字节集数据
                WriteProcessMemory(hProcess, (IntPtr)baseAddress, new int[] { value }, 4, IntPtr.Zero);
                //关闭操作
                CloseHandle(hProcess);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// 根据窗体标题查找窗口句柄（支持模糊匹配）
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
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
        /// <param name="process"></param>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public static IntPtr GetModuleBasePath(Process process, string moduleName)
        {
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
        /// 读取内存的值
        /// </summary>
        /// <param name="process">进程对象</param>
        /// <param name="moduleName">基址模块名</param>
        /// <param name="offsetArray">多级偏移</param>
        /// <returns>1.具体地址,2.值</returns>
        /// <exception cref="Exception">至少需要一个偏移</exception>
        public static (int, int) ReadMemoryValue(Process process, string moduleName, params int[] offsetArray)
        {
            if ((offsetArray?.Length ?? 0) == 0)
            {
                throw new Exception("至少需要一个偏移");
            }

            var pid = process.Id;

            // 模块的地址
            var address = MemoryUtils.GetModuleBasePath(process, moduleName);

            // 计算一级偏移
            var addr = (int)address + offsetArray[0];
            var addrVal = MemoryUtils.ReadMemoryValueToInt32(addr, pid);

            // 计算剩下的多级偏移
            for (int i = 1; i < offsetArray.Length; i++)
            {
                addr = addrVal + offsetArray[i];
                addrVal = MemoryUtils.ReadMemoryValueToInt32(addr, pid);
            }

            // 最后一级的具体地址, 内存的值
            return (addr, addrVal);
        }

        #endregion
    }
}
