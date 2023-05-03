using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CharacterType
{
    None,
    All,
    Warrior,
    MarkMan,
    Magician,
    Support,
    Assassin

}

namespace Framework.UI
{
    public class LobbyUI : ManagementUIBase
    {
        // Start is called before the first frame update

        [SerializeField] TextMeshProUGUI _goldText;

        [SerializeField] TextMeshProUGUI _dateText;

        [SerializeField]
        delegate void SetGoldDelegate<T>(T t);

        [SerializeField]
        static SetGoldDelegate<int> _setVoidGoldDelegate;

        [SerializeField]
        private ChampionListButtonChange[] champListButton;

        [SerializeField]
        private List<CharacterPersnality> characterList;

        [SerializeField]
        private CharacterJsonRead characterJsonRead;

        [SerializeField]
        private ChampListSort champListSort;

        

        

        public void Init()
        {
            _setVoidGoldDelegate += RefreshData;
            _setVoidGoldDelegate(100);


            for (int i = 0; i < champListButton.Length; i++)
            {
                champListButton[i].myCharacterType = (CharacterType)i + 1;
            }

            characterJsonRead.Init();
            champListSort.Init();
            

        }

        public void RefreshData(int Gold)
        {
            _goldText.text = "" + GameManager.PlayerInfomation.Gold;
        }

        public void SetGoldDeleGate(int Gold)
        {
            GameManager.PlayerInfomation.Gold += Gold;
            _setVoidGoldDelegate(Gold);

            Debug.Log(GameManager.PlayerInfomation.Gold);
        }

    }
}
