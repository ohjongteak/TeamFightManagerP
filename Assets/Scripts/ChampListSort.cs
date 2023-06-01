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
    public void Init()//초기화
    {
        var CharacterStateArray = characterJsonRead.characterStateList.characterState;

        for (int i = 0; i < CharacterStateArray.Length; i++)
        {
            if (CharacterStateArray[i].unlock == "true")
            {
                
                GameObject ChampBox = Instantiate(champBoxPrefab, champListPanel);
                champIntroBox.Add(ChampBox);
               // ChampBox.transform.GetChild(0).GetComponent<Image>().sprite = imgManager.champSprite[i];
                ChampBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = CharacterStateArray[i].positionName;
                ChampBox.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = CharacterStateArray[i].characterName;

            }

        }

    }

    public void AllChamp()//올챔프 버튼 클릭
    {
        for(int i = 0; i<champIntroBox.Count; i++)
        {
            champIntroBox[i].SetActive(true);
        }

    }
    public void Assasin()//어썌신 유닛들
    {
        var CharacterStateArray = characterJsonRead.characterStateList.characterState;

        
    }

    public void Warrior()//워리어 유닛들
    {
        var CharacterStateArray = characterJsonRead.characterStateList.characterState;

        for(int i =0; i<champIntroBox.Count; i++)
        {
            if(CharacterStateArray[i].indexCharacter / 100 == 1)
            {
                champIntroBox[i].gameObject.SetActive(true);
            }
            else
                champIntroBox[i].gameObject.SetActive(false);
        }

    }

    public void MarkMan()//원거리 유닛들
    {
        var CharacterStateArray = characterJsonRead.characterStateList.characterState;


        for (int i = 0; i < champIntroBox.Count; i++)
        {
            if (CharacterStateArray[i].indexCharacter / 100 == 2)
            {
                champIntroBox[i].gameObject.SetActive(true);
            }
            else
            {
                champIntroBox[i].gameObject.SetActive(false);
                
            }
        }
    }

    public void Magicion()//마법사들
    {
        var CharacterStateArray = characterJsonRead.characterStateList.characterState;


    }

   

}
