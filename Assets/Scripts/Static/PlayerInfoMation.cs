using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.UI
{
    public class PlayerInfoMation 
    {
        public int Gold { get; set; }

       

       public void Init()
       {
            SetGold(Gold);
           
            
       }

       public void SetGold(int gold)
       {
            Gold = gold;
           // GameManager.UIManager.GetUI<LobbyUI>(UIName.LobbyUI)?.RefreshData();
           
       }

       
    }
}
