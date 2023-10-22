using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TCGame.Client.UI;
using TCGame.Client.Enum;

namespace TCGame.Client.Deck
{
    public class DeckEvent : MonoBehaviour
    {
        public DeckUI deckUI; 
        public void Awake()
        {
            deckUI.LoadUI();
            deckUI.LoadEvent();
        }
    }
}
