using MyUtils;

using War3_72bian.Model;

namespace War3_72bian
{
    public partial class Form1 : Form
    {
        private readonly string _processName = "War3";
        private readonly string _moduleName = "Game.dll";

        private Dictionary<string, string> _bossPositionDic = new Dictionary<string, string>()
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

        private HeroInfo _heroInfo = default;
        private MemoryUtils _memoryUtils = default;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化游戏
        /// -------
        /// 注意:
        /// 本程序一定要编译为32位程序(默认是any,会自动识别系统,编译为64位),
        /// 否则无法正确读取32位游戏内存
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
        private void ModifyJingYan(int value)
        {
            // 修改经验
            _memoryUtils.WriteInt(_heroInfo.JinYanAddress, value);
        }

        /// <summary>
        /// 等级5
        /// </summary>
        private void level5_button_Click(object sender, EventArgs e)
        {
            ModifyJingYan(2222);
        }

        /// <summary>
        /// 等级25
        /// </summary>
        private void level25_button_Click(object sender, EventArgs e)
        {
            ModifyJingYan(3800);
        }

        /// <summary>
        /// 等级max
        /// </summary>
        private void level_max_button_Click(object sender, EventArgs e)
        {
            ModifyJingYan(3821192);
        }

        /// <summary>
        /// 力量
        /// </summary>
        private void li_liang_max_button_Click(object sender, EventArgs e)
        {
            _memoryUtils.WriteInt(_heroInfo.LiLiangAddress, 50000);
        }

        /// <summary>
        /// 敏捷
        /// </summary>
        private void min_jie_max_button_Click(object sender, EventArgs e)
        {
            _memoryUtils.WriteInt(_heroInfo.MinJieAddress, 10000);
        }

        /// <summary>
        /// 智力
        /// </summary>
        private void zhi_li_max_Click(object sender, EventArgs e)
        {
            _memoryUtils.WriteInt(_heroInfo.ZhiLiAddress, 100000);
        }

        /// <summary>
        /// 移速
        /// </summary>
        private void yi_su_max_button_Click(object sender, EventArgs e)
        {
            _memoryUtils.WriteFloat(_heroInfo.YiSuAddress, 522);
        }

        /// <summary>
        /// 一技能冷却
        /// </summary>
        private void first_skill_time_button_Click(object sender, EventArgs e)
        {
            _memoryUtils.WriteFloat(_heroInfo.FirstSkillTimeAddress, 0.3f);
        }

        /// <summary>
        /// 瞬移 点击事件
        /// </summary>
        private void shun_yi_Click(object sender, EventArgs e)
        {
            var name = ((Button)sender).Text;
            ShunYi(name);
        }

        /// <summary>
        /// 瞬移
        /// </summary>
        private void ShunYi(string name)
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
        private void SetHeroPosition(float x, float y)
        {
            _memoryUtils.WriteFloat(_heroInfo.PositionXAddress, x);
            _memoryUtils.WriteFloat(_heroInfo.PositionYAddress, y);
        }

        /// <summary>
        /// 自动初始化
        /// </summary>
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
        private void auto_kill_boss_button_Click(object sender, EventArgs e)
        {
            var gameHandle = IntPtr.Zero;

            //获取窗口句柄
            IntPtr GetWindowHandle(string processName)
            {
                var handle = MemoryUtils.GetProcessByProcessName(processName)?.MainWindowHandle ?? IntPtr.Zero;

                if (handle == IntPtr.Zero)
                {
                    throw new Exception($"没有找到[{processName}]进程..");
                }
                return handle;
            }

            void KillAllBoss()
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
                    Delay(500);
                    ShunYi(name);
                    Delay(500);
                }
            }

            void MoveToShop() => MouseKeyboardUtils.MoveTo(gameHandle, 778, 280);

            void MoveToUser() => MouseKeyboardUtils.MoveTo(gameHandle, 34, 74);

            void MoveToCenter() => MouseKeyboardUtils.MoveTo(gameHandle, 630, 392);

            void ShowMsg(string msg) => this.Invoke(() => this.Text = msg);

            void Delay(int ms) => Task.Delay(ms).Wait();

            void DelayMsg(string msg, int ms)
            {
                for (int i = ms / 1000; i >= 0; i--)
                {
                    ShowMsg(msg + ": " + i);
                    Delay(1000);
                }
            }

            // 平台进程名  Platform
            // 平台开始游戏按钮 576, 569

            // 人物头像  34, 74
            // 宠物头像  35, 633
            // 选择难度 722,465
            // 仙子   382, 493
            // 商店   778, 280
            // NPC   489,315
            // 返回平台按钮  629, 76

            // 记录局数
            var count = 1;

            void Start()
            {
                // 点击开始
                MouseKeyboardUtils.MoveTo(GetWindowHandle("Platform"), 576, 569);
                Delay(1000);
                MouseKeyboardUtils.LeftClick();
                DelayMsg("等待游戏加载", 40000);

                gameHandle = GetWindowHandle(_processName);

                Delay(2000);
                // 选择难度
                MouseKeyboardUtils.MoveTo(gameHandle, 722, 465);
                MouseKeyboardUtils.LeftClick();
                Delay(2000);
                MouseKeyboardUtils.LeftClick();
                Delay(2000);

                //选择角色
                MouseKeyboardUtils.MoveTo(gameHandle, 382, 493);
                MouseKeyboardUtils.LeftDoubleClick();
                Delay(5000);

                // 点击头像
                //MoveToUser();
                //MouseKeyboardUtils.LeftDoubleClick();
                //Delay(2000);

                //移动到商店附近
                MoveToShop();
                MouseKeyboardUtils.RightDoubleClick();
                Delay(2000);

                //调用自动初始化
                auto_init_button_Click(sender, e);

                //点击宠物头像
                MouseKeyboardUtils.MoveTo(gameHandle, 35, 633);
                MouseKeyboardUtils.LeftDoubleClick();
                Delay(1000);

                // 让宠物走到NPC附近,激活任务
                MouseKeyboardUtils.MoveTo(gameHandle, 489, 315);
                MouseKeyboardUtils.RightDoubleClick();
                Delay(1000);

                // 点击商店
                MoveToShop();
                MouseKeyboardUtils.LeftDoubleClick();
                Delay(1000);

                // 点击力量果实,触发属性变更,血量重新计算
                MouseKeyboardUtils.MoveTo(gameHandle, 1006, 750);
                MouseKeyboardUtils.LeftClick();
                Delay(1000);

                // 点击头像
                MoveToUser();
                MouseKeyboardUtils.LeftClick();

                int initStep = 0;

                //瞬移到小海龟
                for (int i = 0; i < 18; i++)
                {
                    ShunYi("小海龟");

                    // 读取当前经验
                    var jinYanValue = _memoryUtils.ReadToInt(_heroInfo.JinYanAddress);

                    //5级前
                    if (initStep == 0 && jinYanValue != 2222)
                    {
                        level_max_button_Click(sender, e);
                        initStep = 1;
                    }
                    // 5级以后
                    if (initStep == 1 && jinYanValue > 2222) 
                    {
                        if (_memoryUtils.ReadToFloat(_heroInfo.FirstSkillTimeAddress) >= 1)
                        {
                            first_skill_time_button_Click(sender, e);
                        }
                    }

                    Delay(1000);
                }

                for (int i = 0; i < 10; i++)
                {
                    ShunYi("小虾兵");
                    Delay(1000);
                }

                DelayMsg("准备扫荡boss", 3000);
                KillAllBoss();

                DelayMsg("扫荡结束,等待退出", 14000);

                // 点击 返回平台
                MouseKeyboardUtils.MoveTo(gameHandle, 629, 76);
                MouseKeyboardUtils.LeftClick();
                count++;
            }

            Task.Factory.StartNew(() =>
                {
                    try
                    {
                        while (true)
                        {
                            ShowMsg($"正在进行第{count}局");
                            Start();
                            DelayMsg("等待开始下一局", 10000);
                        }
                    }
                    catch (Exception e)
                    {
                        //将异常抛给主线程
                        this.BeginInvoke(() => throw e);
                    }
                });
        }
    }
}