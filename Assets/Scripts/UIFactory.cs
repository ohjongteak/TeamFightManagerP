using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Framework.UI
{
    public partial class UIManager
    {
        //public static MainHUD CreateMainHUD()
        //{
        //    string path = "UI/MainHUD";
        //    ManagementUIBase newUI = GameManager.Instance.UIManager.CreateUI(path, UIName.MainHUD);
        //    return newUI as MainHUD;
        //}

        public static void CreateMainHUD(Action<MainHUD> createCallback)
        {
            GameManager.UIManager.CreateUI<MainHUD>("UI/MainHUD", UIName.MainHUD, createCallback);
        }

      
        public static void CreateLobbyUI(Action<LobbyUI> createCallback)
        {
          
        }

        //public static void CreateLobbyMainUIButton(Action<LobbyMainUIButton> createCallback)
        //{
        //    GameManager.UIManager.CreateUI<LobbyMainUIButton>("UI/LobbyUI/GameButton", UIName.LobbyUIButton, createCallback);
        //    GameManager.UIManager.CreateUI<LobbyMainUIButton>("UI/LobbyUI/LeagueButton", UIName.LobbyUIButton, createCallback);
        //    GameManager.UIManager.CreateUI<LobbyMainUIButton>("UI/LobbyUI/ManagementButton", UIName.LobbyUIButton, createCallback);
        //    GameManager.UIManager.CreateUI<LobbyMainUIButton>("UI/LobbyUI/SystemButton", UIName.LobbyUIButton, createCallback);
        //    GameManager.UIManager.CreateUI<LobbyMainUIButton>("UI/LobbyUI/TeamManagerButton", UIName.LobbyUIButton, createCallback);
        //}
    }
}