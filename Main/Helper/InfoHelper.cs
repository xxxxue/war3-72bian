using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyUtils;

namespace Main.Helper
{
    public class InfoHelper
    {

        /// <summary>
        /// 进程名
        /// </summary>
        public static string _processName = "War3";

        /// <summary>
        /// 模块名
        /// </summary>
        public static string _moduleName = "Game.dll";

        /// <summary>
        /// 获取 进程信息
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">没有找到进程</exception>
        public static (Process, int) GetProcessInfo()
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
        /// 怪物坐标
        /// </summary>
        public static Dictionary<string, string> _bossPositionDic = new Dictionary<string, string>()
        {
            { "出生点","49.8,29.4"},
            { "竞技场","246,201"},

            { "小海龟","105,135.5"},
            { "小虾兵","101.6,94"},
            { "蟹将","187.5,34.2"},

            { "黑熊精","77.1,362.4"},
            { "白骨精","142,357.7"},
            { "羊力大仙","264.5,44.4"},
            { "鹿力大仙","333.8,127.5"},
            { "虎力大仙","582.5,230.9"},
            { "九头虫","500.7,331.7"},
            { "金鱼精","452.2,448.5"},

            { "主城","442,548"},

            { "白面狐狸","585.3,528.8"},
            { "玉面狐狸","588.4,584.7"},
            { "银角大王","273.6,489.8"},
            { "金角大王","296,564.5"},

            { "七蜘蛛","142.2,604.2"},
            { "五/六蜘蛛","184.8,455.2"},
            { "剩余蜘蛛","277.3,342.3"},

            { "红孩儿","679,593.2"},
            { "铁扇公主","717.2,414.8"},
            { "牛魔王","714,332"},

            { "青毛狮子","658.5,184.7"},
            { "黄牙老象","721.9,125.1"},
            { "金翅大鹏","652.3,64.1"},

            { "女儿国巫师","493.7,715.9"},
            { "女儿国国师","716.9,714.9"},
            { "女儿国国王","722.1,648.9"},

            { "黄风怪","410.5,715.5"},
            { "黄袍怪","302.2,706.5"},
            { "黄眉大王","187.9,708.4"},
        };
    }
}
