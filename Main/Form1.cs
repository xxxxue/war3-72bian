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

using Main.Helper;
using Main.Model;

using MyUtils;

namespace Main
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string _moduleName = default;
        Process _process = default;
        int _pid = default;

        HeroInfo _heroInfo = default;
        Dictionary<string, string> _bossPositionDic = default;

        /// <summary>
        /// 初始化游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void init_game_button_Click(object sender, EventArgs e)
        {
            (_process, _pid) = InfoHelper.GetProcessInfo();

            _moduleName = InfoHelper._moduleName;
            _bossPositionDic = InfoHelper._bossPositionDic;
            _heroInfo = new HeroInfo
            {
                JinYanAddress = MemoryUtils.GetMemoryAddress(_process, _moduleName, 0x00BE87A4, 0x30, 0x1F0, 0x8C),
                LiLiangAddress = MemoryUtils.GetMemoryAddress(_process, _moduleName, 0x00BE87A4, 0x30, 0x1F0, 0x94),
                MinJieAddress = MemoryUtils.GetMemoryAddress(_process, _moduleName, 0x00BE87A4, 0x30, 0x1F0, 0xA8),
                YiSuAddress = MemoryUtils.GetMemoryAddress(_process, _moduleName, 0x00B59790, 0x1EC, 0x70),
                PositionXAddress = MemoryUtils.GetMemoryAddress(_process, _moduleName, 0x00BC5FF4, 0x78),
                PositionYAddress = MemoryUtils.GetMemoryAddress(_process, _moduleName, 0x00BC5FF4, 0x7C)
            };

            init_game_button.Text = "初始化成功";
        }

        /// <summary>
        /// 修改经验
        /// </summary>
        /// <param name="value"></param>
        void ModifyJingYan(int value)
        {
            // 修改经验
            MemoryUtils.WriteMemoryValue(_heroInfo.JinYanAddress, _pid, value);

            msg_lable.Text = "当前经验:" + MemoryUtils.ReadMemoryValueToInt32(_heroInfo.JinYanAddress, _pid);
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
            ModifyJingYan(999999);
        }

        /// <summary>
        /// 力量
        /// </summary>
        void li_liang_max_button_Click(object sender, EventArgs e)
        {
            MemoryUtils.WriteMemoryValue(_heroInfo.LiLiangAddress, _pid, 9999999);

            msg_lable.Text = MemoryUtils.ReadMemoryValueToInt32(_heroInfo.LiLiangAddress, _pid).ToString();
        }

        /// <summary>
        /// 敏捷
        /// </summary>
        void min_jie_max_button_Click(object sender, EventArgs e)
        {
            MemoryUtils.WriteMemoryValue(_heroInfo.MinJieAddress, _pid, 9999);

            msg_lable.Text = MemoryUtils.ReadMemoryValueToInt32(_heroInfo.MinJieAddress, _pid).ToString();
        }

        /// <summary>
        /// 移速
        /// </summary>
        void yi_su_max_button_Click(object sender, EventArgs e)
        {
            MemoryUtils.WriteMemoryFloatValue(_heroInfo.YiSuAddress, _pid, 522);

            //内存单浮点 类型 转为 人类可读的 十进制
            msg_lable.Text = MemoryUtils.ReadMemoryFloatToShow(_heroInfo.YiSuAddress, _pid);
        }

        /// <summary>
        /// 瞬移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        private void shun_yi_Click(object sender, EventArgs e)
        {                       
            var name = ((Button)sender).Text;           

            var bossPos = _bossPositionDic
                 .Where(item => item.Key == name)
                 .Select(item =>
                 {
                     var pos = item.Value.Split(',');
                     if (pos.Length != 2)
                     {
                         throw new Exception($"分割{item.Key}坐标信息({item.Value})发生错误...");
                     }
                     return new
                     {
                         x = float.Parse(pos[0]),
                         y = float.Parse(pos[1]),
                     };
                 }).FirstOrDefault();

            if (bossPos == null)
            {
                throw new Exception($"没有与[{name}]匹配的信息..");
            }
            SetHeroPosition(bossPos.x, bossPos.y);
            msg_lable.Text = $"{name} 瞬移成功";
        }

        /// <summary>
        ///  设置 角色位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void SetHeroPosition(float x, float y)
        {
            MemoryUtils.WriteMemoryFloatValue(_heroInfo.PositionXAddress, _pid, x);
            MemoryUtils.WriteMemoryFloatValue(_heroInfo.PositionYAddress, _pid, y);
        }
    }
}
