using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
namespace Framework.UI
{
    public class TrainingManager : MonoBehaviour
    {
        [SerializeField]
        SummonerManager summonerManager;
        [SerializeField]
        Transform summonerGridPanel;
        [SerializeField]
        private TextMeshProUGUI remainText;
        [SerializeField]
        private TextMeshProUGUI[] investPoint;
        [SerializeField]
        private GameObject[] statBoxes;
        [SerializeField]
        private RectTransform[] advantageGuage;
        [SerializeField]
        private Slider[] savePointGuage;
        [SerializeField]
        private Image[] remainPointImage;
        public void SummonerBox(string name)
        {
            remainText.text = "0";
            Image[] PointInvestImage = new Image[18];
            Color Black;
            ColorUtility.TryParseHtmlString("#383a40", out Black);
            for (int i = 0; i < statBoxes.Length; i++)
            {
                statBoxes[i].gameObject.SetActive(true);
                investPoint[i].text = "0";
                advantageGuage[i].sizeDelta = new Vector2(1, advantageGuage[i].rect.height);
                savePointGuage[i].value = 0;
                statBoxes[i].transform.Find("LeftArrow").GetComponent<Button>().onClick.RemoveAllListeners();
                statBoxes[i].transform.Find("RightArrow").GetComponent<Button>().onClick.RemoveAllListeners();

                
                for (int x = 0; x < 3; x++)
                {
                    PointInvestImage[i * 3 + x] = statBoxes[i].transform.GetChild(6 + x).GetComponent<Image>();
                    PointInvestImage[i * 3 + x].color = Black;
                }
            }
            Color HardBlack;
            ColorUtility.TryParseHtmlString("#22252a", out HardBlack);

            for (int i = 0; i< remainPointImage.Length;i++)
            {
                remainPointImage[i].color = HardBlack;
            }
            //-------------------------------------초기화----------------------------------------------------
            var summonerInfo = summonerManager.GetSummonerInfo();

           
            for (int i = 0; i < summonerInfo.Count; i++)
            {
                if (summonerInfo[i].name == name)
                {
                    remainText.text = summonerInfo[i].remainPoint + "/3";

                    for (int z = 0; z < statBoxes.Length; z++)
                    {
                        investPoint[z].text = summonerInfo[i].investPoint[z].ToString();
                        int Index = z;
                        statBoxes[z].transform.Find("LeftArrow").GetComponent<Button>().onClick.AddListener
                            (delegate { GetIndexCount(statBoxes[Index].transform,"LeftArrow",summonerInfo[i],PointInvestImage);});
                        statBoxes[z].transform.Find("RightArrow").GetComponent<Button>().onClick.AddListener
                            (delegate { GetIndexCount(statBoxes[Index].transform,"RightArrow",summonerInfo[i],PointInvestImage);});
                    }

                    ChangeRemainPointColor(summonerInfo[i]);
                    ChangePointColor(summonerInfo[i], PointInvestImage);

                    for (int z = summonerInfo[i].mainHero.Count; z <= 2; z++)
                    {
                        statBoxes[z + 3].gameObject.SetActive(false);
                    }

                    for (int z = 0; z < advantageGuage.Length; z++)
                    {
                        float advantageValue = summonerInfo[i].adventagePoint[z];
                        advantageGuage[z].sizeDelta = new Vector2(165 * advantageValue, advantageGuage[i].rect.height);
                        savePointGuage[z].value = summonerInfo[i].savePoint[z];
                    }

                    break;
                }

            }

        }

        public void GetIndexCount(Transform MySelf, string Arrow , SummonerCharacterState SummonerInfo , Image[] PointInvestImage)
        {
            int Index = (MySelf.transform.GetSiblingIndex());

           


           switch(Arrow)
           {
                case "LeftArrow":
                    Debug.Log("왼쪽화살표");

                    Color Black;
                    ColorUtility.TryParseHtmlString("#383a40", out Black);
                    for (int i = 0; i < PointInvestImage.Length; i++)
                        PointInvestImage[i].color = Black;

                    if (SummonerInfo.investPoint[Index] > 0)
                    {
                        SummonerInfo.investPoint[Index] -= 1;
                        SummonerInfo.remainPoint += 1;
                        
                    }

                    break;
                case "RightArrow":
                    Debug.Log("오른쪽화살표");

                    Color HardBlack;
                    ColorUtility.TryParseHtmlString("#22252a", out HardBlack);

                    for (int i = 0; i < remainPointImage.Length; i++)
                    {
                        remainPointImage[i].color = HardBlack;
                    }

                    if (SummonerInfo.remainPoint > 0)
                    {
                        SummonerInfo.investPoint[Index] += 1;
                        SummonerInfo.remainPoint -= 1;
                        
                    }
                    break;
           }

           investPoint[Index].text = SummonerInfo.investPoint[Index].ToString();
           remainText.text = SummonerInfo.remainPoint + "/3";
           ChangePointColor(SummonerInfo,PointInvestImage);
           ChangeRemainPointColor(SummonerInfo);
        }

        public void ChangePointColor(SummonerCharacterState summonerInfo, Image[] PointInvestImage)
        {
            Color Yellow;
            ColorUtility.TryParseHtmlString("#ffbe1b", out Yellow);

            for (int z = 0; z < summonerInfo.investPoint.Length; z++)
            {
                for (int x = 0; x < summonerInfo.investPoint[z]; x++)
                {
                    PointInvestImage[z * 3 + x].color = Yellow;
                }
                //PointInvestImage[z].color = Color.yellow;
            }
        }

        public void ChangeRemainPointColor(SummonerCharacterState summonerInfo)
        {
            Color Yellow;
            ColorUtility.TryParseHtmlString("#ffbe1b", out Yellow);

            for (int z = 0; z < summonerInfo.remainPoint; z++)
            {
                remainPointImage[z].color = Yellow;
            }
        }

    }
}
