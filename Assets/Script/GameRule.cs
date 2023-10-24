using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TCGame.Client
{
    //我们的游戏规则类
    public class GameRule
    {
        //设置规则上卡片的最大值
        public static int MaxMainCount = 60;
        public static int MaxExtraCount = 15;
        public static int MaxSecondCount = 15;

        //允许的最大卡片重复数
        public static int MaxRepeatCardCount = 3;
    }
}
