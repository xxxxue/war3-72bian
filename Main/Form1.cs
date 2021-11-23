using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MyUtils;

namespace Main
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 进程名
        /// </summary>
        static string _processName = "War3";

        /// <summary>
        /// 模块名
        /// </summary>
        static string _moduleName = "Game.dll";

        /// <summary>
        /// 获取 进程信息
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">没有找到进程</exception>
        (Process, int) GetProcessInfo()
        {
            var process = MemoryUtils.GetProcessByProcessName(_processName);
            if (process == null)
            {
                throw new Exception($"没有找到进程:{_processName},请先启动后,再使用该功能");
            }
            var pid = process.Id;
            return (process, pid);
        }

        /// <summary>
        /// 获取 经验地址
        /// </summary>
        /// <returns></returns>
        int GetJingYanAddress()
        {
            var (process, pid) = GetProcessInfo();

            var (addr, value) = MemoryUtils.ReadMemoryValue(process, _moduleName, 0x00BE87A4, 0x30, 0x1F0, 0x8C);
            return addr;
        }

        /// <summary>
        /// 获取 力量地址
        /// </summary>
        /// <returns></returns>
        int GetLiLiangAddress()
        {
            var (process, pid) = GetProcessInfo();

            var (addr, value) = MemoryUtils.ReadMemoryValue(process, _moduleName, 0x00BE87A4, 0x30, 0x1F0, 0x94);

            return addr;
        }

        /// <summary>
        /// 获取 敏捷地址
        /// </summary>
        /// <returns></returns>
        int GetMinJieAddress()
        {
            var (process, pid) = GetProcessInfo();

            var (addr, value) = MemoryUtils.ReadMemoryValue(process, _moduleName, 0x00BE87A4, 0x30, 0x1F0, 0xA8);

            return addr;
        }

        /// <summary>
        /// 获取  移速地址
        /// </summary>
        /// <returns></returns>
        int GetYiSuAddress()
        {
            var (process, pid) = GetProcessInfo();

            var (addr, value) = MemoryUtils.ReadMemoryValue(process, _moduleName, 0x00B59790, 0x1EC, 0x70);

            return addr;
        }

        /// <summary>
        /// 修改经验
        /// </summary>
        /// <param name="value"></param>
        void ModifyJingYan(int value)
        {
            var (process, pid) = GetProcessInfo();

            var address = GetJingYanAddress();
            // 修改经验
            MemoryUtils.WriteMemoryValue(address, pid, value);

            msg_lable.Text = "当前经验:" + MemoryUtils.ReadMemoryValueToInt32(address, pid);
        }

        /// <summary>
        /// 等级5
        /// </summary>
        void level5_button_Click(object sender, EventArgs e)
        {
            ModifyJingYan(2222);
        }

        /// <summary>
        /// 等级25
        /// </summary>
        void level25_button_Click(object sender, EventArgs e)
        {
            ModifyJingYan(3800);
        }

        /// <summary>
        /// 等级max
        /// </summary>
        void level_max_button_Click(object sender, EventArgs e)
        {
            ModifyJingYan(9999999);
        }

        /// <summary>
        /// 力量
        /// </summary>
        void li_liang_max_button_Click(object sender, EventArgs e)
        {
            var (process, pid) = GetProcessInfo();

            var addr = GetLiLiangAddress();

            MemoryUtils.WriteMemoryValue(addr, pid, 9999999);

            msg_lable.Text = MemoryUtils.ReadMemoryValueToInt32(addr, pid).ToString();
        }

        /// <summary>
        /// 敏捷
        /// </summary>
        void min_jie_max_button_Click(object sender, EventArgs e)
        {
            var (process, pid) = GetProcessInfo();

            var addr = GetMinJieAddress();

            MemoryUtils.WriteMemoryValue(addr, pid, 999999);

            msg_lable.Text = MemoryUtils.ReadMemoryValueToInt32(addr, pid).ToString();
        }

        /// <summary>
        /// 移速
        /// </summary>
        void yi_su_max_button_Click(object sender, EventArgs e)
        {
            var (process, pid) = GetProcessInfo();

            var addr = GetYiSuAddress();

            //设置移速为522
            var maxVal = Convert.ToInt32(IEEE754Utils.FloatToHex(522), 16);

            MemoryUtils.WriteMemoryValue(addr, pid, maxVal);

            //获取最新的移速
            var val = MemoryUtils.ReadMemoryValueToInt64(addr, pid);
            //内存单浮点 类型 转为 人类可读的 十进制
            msg_lable.Text = IEEE754Utils.HexToFloat(val.ToString("x8")).ToString();
        }

    }
}
