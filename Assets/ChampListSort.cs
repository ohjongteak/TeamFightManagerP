using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ChampListSort : MonoBehaviour
{
    //public List<ChampIntroBox> tmepCon;
    public List<GameObject> champIntroBox;
    public GameObject champBoxPrefab;
    public CharacterJsonRead characterJsonRead;

    [SerializeField]
    private Transform champListPanel;

    [SerializeField]
    private ImageManager imgManager;
    public void Init()
    {
        var CharacterStateArray = characterJsonRead.characterStateList.characterState;

        for (int i = 0; i < CharacterStateArray.Length; i++)
        {
            if (CharacterStateArray[i].Unlock == "true")
            {
                GameObject ChampBox = Instantiate(champBoxPrefab, champListPanel);

                ChampBox.transform.GetChild(0).GetComponent<Image>().sprite = imgManager.champSprite[i];
                ChampBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = CharacterStateArray[i].positionName;
                ChampBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = CharacterStateArray[i].characterName;

            }

        }


    }

}
