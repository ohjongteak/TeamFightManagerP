using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Framework.UI
{
    using UI;
    public class Lobby
    {

        public Canvas Canvas { get; set; }
        public EventSystem EventSystem { get; set; }

        public LobbyUI lobbyUI;
        public void Init()
        {
            lobbyUI.Init();
            //  GameManager.UIManager.GetUI<LobbyUI>(UIName.LobbyUI).
          
        }

        
    }
}