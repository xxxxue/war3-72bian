using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main.Model
{
    public class HeroInfo
    {

        public int JinYanAddress { get; set; }
        public int LiLiangAddress { get; set; }
        public int MinJieAddress { get; set; }
        public int YiSuAddress { get; set; }

        /// <summary>
        /// X坐标
        /// </summary>
        public int PositionXAddress { get; set; }

        /// <summary>
        /// Y坐标
        /// </summary>
        public int PositionYAddress { get; set; }

    }
}
