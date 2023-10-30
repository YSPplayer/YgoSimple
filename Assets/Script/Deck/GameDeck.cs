using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TCGame.Client.Deck
{
    public class GameDeck
    {
        public List<int> MianCodes { get; private set; }
        public List<int> ExtraCodes { get; private set; }
        public List<int> SecondCodes { get; private set; }

        public float[] MainSpacing { get; private set; }
        public float[] ExtraSpacing { get; private set; }
        public float[] SecondSpacing { get; private set; }
        public GameDeck(List<int> mianCodes, List<int> extraCodes, List<int> secondCodes, float[] mainSpacing, float[] extraSpacing, float[] secondSpacing)
        {
            MianCodes = mianCodes;
            ExtraCodes = extraCodes;
            SecondCodes = secondCodes;
            MainSpacing = mainSpacing;
            ExtraSpacing = extraSpacing;
            SecondSpacing = secondSpacing;
        }
    }
    
}