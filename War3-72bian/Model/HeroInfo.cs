using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War3_72bian.Model
{
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
