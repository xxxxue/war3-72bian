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
        /// 初始化信息
        /// </summary>
        public static void InitInfo()
        {
            var (process, pid) = GetProcessInfo();
        }

        /// <summary>
        /// 怪物坐标
        /// </summary>
        public static Dictionary<string, string> _bossPositionDic = new Dictionary<string, string>()
        {
            { "出生点","49.8,29.4"},
            { "竞技场","246,201"},
            { "蟹将","158,33"},
            { "黑熊精/白骨精","69,344"},
            { "羊力大仙/鹿力大仙","302,47"},
            { "虎力大仙","585,181"},
            { "九头虫/金鱼精","454,308"},
            { "主城","442,548"},
            { "白面/玉面狐狸","548,525"},
            { "银角大王","311,486"},
            { "金角大王","282,584"},
            { "七蜘蛛","166,559"},
            { "五/六蜘蛛","220,430"},
            { "剩余蜘蛛","254,334"},
            { "红孩儿","714,580"},
            { "铁扇公主","732,434"},
            { "牛魔王","673,351"},
            { "青毛狮子/黄毛老象/金翅大鹏","662,143"},
            { "女儿国巫师","557,710"},
            { "女儿国国师/国王","657,722"},
            { "黄风怪","399,642"},
            { "黄袍怪","264,685"},
            { "黄眉大王","179,638"},
        };
    }
}
