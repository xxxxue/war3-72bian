using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

using MyUtils;
using System.Threading.Tasks;

namespace Main
{
    public partial class Form1 : Form
    {
        readonly string _processName = "War3";  
        readonly string _moduleName = "Game.dll";
        Dictionary<string, string> _bossPositionDic = new Dictionary<string, string>()
        {
            {"出生点", "49.8,29.4"},
            {"竞技场", "246,201"},

            {"小海龟", "105,135.5"},
            {"小虾兵", "101.6,94"},
            {"蟹将", "187.5,34.2"},

            {"黑熊精", "77.1,362.4"},
            {"白骨精", "142,357.7"},
            {"羊力大仙", "264.5,44.4"},
            {"鹿力大仙", "333.8,127.5"},
            {"虎力大仙", "582.5,230.9"},
            {"九头虫", "500.7,331.7"},
            {"金鱼精", "452.2,448.5"},

            {"主城", "442,548"},

            {"白面狐狸", "585.3,528.8"},
            {"玉面狐狸", "588.4,584.7"},
            {"银角大王", "273.6,489.8"},
            {"金角大王", "296,564.5"},

            {"七蜘蛛", "142.2,604.2"},
            {"五/六蜘蛛", "184.8,455.2"},
            {"剩余蜘蛛", "277.3,342.3"},

            {"红孩儿", "679,593.2"},
            {"铁扇公主", "717.2,414.8"},
            {"牛魔王", "714,332"},

            {"青毛狮子", "658.5,184.7"},
            {"黄牙老象", "721.9,125.1"},
            {"金翅大鹏", "652.3,64.1"},

            {"女儿国巫师", "493.7,715.9"},
            {"女儿国国师", "716.9,714.9"},
            {"女儿国国王", "722.1,648.9"},

            {"黄风怪", "410.5,715.5"},
            {"黄袍怪", "302.2,706.5"},
            {"黄眉大王", "187.9,708.4"},
        };
        HeroInfo _heroInfo = null;
        MemoryUtils _memoryUtils = null;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void init_game_button_Click(object sender, EventArgs e)
        {
            var process = MemoryUtils.GetProcessByProcessName(_processName);
            if (process == null)
            {
                throw new Exception($"没有找到进程:{_processName},请先启动后,再使用该功能");
            }

            _memoryUtils = new MemoryUtils(process.Id);

            _heroInfo = new HeroInfo
            {
                JinYanAddress = _memoryUtils.GetMemoryAddress(_moduleName, 0x00BE87A4, 0x30, 0x1F0, 0x8C),
                LiLiangAddress = _memoryUtils.GetMemoryAddress(_moduleName, 0x00BE87A4, 0x30, 0x1F0, 0x94),
                MinJieAddress = _memoryUtils.GetMemoryAddress(_moduleName, 0x00BE87A4, 0x30, 0x1F0, 0xA8),
                ZhiLiAddress = _memoryUtils.GetMemoryAddress(_moduleName, 0x00BC5FF4, 0x30, 0x5C, 0x28, 0xF8),
                FirstSkillTimeAddress = _memoryUtils.GetMemoryAddress(_moduleName, 0x00BE9F24, 0x54, 0x8, 0x4, 0x8, 0x48, 0x69C),
                YiSuAddress = _memoryUtils.GetMemoryAddress(_moduleName, 0x00B59790, 0x1EC, 0x70),
                PositionXAddress = _memoryUtils.GetMemoryAddress(_moduleName, 0x00BC5FF4, 0x78),
                PositionYAddress = _memoryUtils.GetMemoryAddress(_moduleName, 0x00BC5FF4, 0x7C)
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
            _memoryUtils.WriteInt(_heroInfo.JinYanAddress, value);

            msg_lable.Text = "当前经验:" + _memoryUtils.ReadToInt(_heroInfo.JinYanAddress);
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
            ModifyJingYan(3821192);
        }

        /// <summary>
        /// 力量 
        /// </summary>
        void li_liang_max_button_Click(object sender, EventArgs e)
        {
            _memoryUtils.WriteInt(_heroInfo.LiLiangAddress, 50000);

            msg_lable.Text = _memoryUtils.ReadToInt(_heroInfo.LiLiangAddress).ToString();
        }

        /// <summary>
        /// 敏捷
        /// </summary>
        void min_jie_max_button_Click(object sender, EventArgs e)
        {
            _memoryUtils.WriteInt(_heroInfo.MinJieAddress, 10000);

            msg_lable.Text = _memoryUtils.ReadToInt(_heroInfo.MinJieAddress).ToString();
        }

        /// <summary>
        /// 智力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zhi_li_max_Click(object sender, EventArgs e)
        {
            _memoryUtils.WriteInt(_heroInfo.ZhiLiAddress, 100000);

            msg_lable.Text = _memoryUtils.ReadToInt(_heroInfo.ZhiLiAddress).ToString();
        }

        /// <summary>
        /// 移速
        /// </summary>
        void yi_su_max_button_Click(object sender, EventArgs e)
        {
            _memoryUtils.WriteFloat(_heroInfo.YiSuAddress, 522);

            //内存单浮点 类型 转为 人类可读的 十进制
            msg_lable.Text = _memoryUtils.ReadToFloat(_heroInfo.YiSuAddress).ToString();
        }

        /// <summary>
        /// 一技能冷却
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void first_skill_time_button_Click(object sender, EventArgs e)
        {
            _memoryUtils.WriteFloat(_heroInfo.FirstSkillTimeAddress, 0.5f);

            msg_lable.Text = _memoryUtils.ReadToFloat(_heroInfo.FirstSkillTimeAddress).ToString();
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
            ShunYi(name);
            msg_lable.Text = $"{name} 瞬移成功";

        }


        void ShunYi(string name)
        {
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
        }


        /// <summary>
        ///  设置 角色位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void SetHeroPosition(float x, float y)
        {
            _memoryUtils.WriteFloat(_heroInfo.PositionXAddress, x);
            _memoryUtils.WriteFloat(_heroInfo.PositionYAddress, y);
        }


        /// <summary>
        /// 自动初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void auto_init_button_Click(object sender, EventArgs e)
        {
            init_game_button_Click(sender, e);
            level5_button_Click(sender, e);
            li_liang_max_button_Click(sender, e);
            zhi_li_max_Click(sender, e);
        }

        /// <summary>
        /// 自动刷boss
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void auto_kill_boss_button_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                var ignorePositionNameArr = new string[]
                {
                    "出生点","竞技场","主城","小海龟","小虾兵"
                };

                foreach (var item in _bossPositionDic)
                {
                    var name = item.Key;

                    if (ignorePositionNameArr.Contains(name)) continue;

                    ShunYi(name);
                    Task.Delay(500).Wait();
                    ShunYi(name);
                    Task.Delay(500).Wait();
                }
            });
        }
    }


    public class HeroInfo
    {
        /// <summary>
        /// 经验
        /// </summary>
        public IntPtr JinYanAddress { get; set; }

        /// <summary>
        /// 力量
        /// </summary>
        public IntPtr LiLiangAddress { get; set; }

        /// <summary>
        /// 敏捷
        /// </summary>
        public IntPtr MinJieAddress { get; set; }

        /// <summary>
        /// 智力
        /// </summary>
        public IntPtr ZhiLiAddress { get; set; }

        /// <summary>
        /// 第一个技能冷却时间地址
        /// </summary>
        public IntPtr FirstSkillTimeAddress { get; set; }

        /// <summary>
        /// 移速
        /// </summary>
        public IntPtr YiSuAddress { get; set; }

        /// <summary>
        /// X坐标
        /// </summary>
        public IntPtr PositionXAddress { get; set; }

        /// <summary>
        /// Y坐标
        /// </summary>
        public IntPtr PositionYAddress { get; set; }
    }

}