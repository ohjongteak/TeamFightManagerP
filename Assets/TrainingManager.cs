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
        private TextMeshProUGUI[] arrInvestPoint;
        [SerializeField]
        private GameObject[] arrStatBoxes;
        [SerializeField]
        private RectTransform[] arrAdvantageGuage;
        [SerializeField]
        private Slider[] arrSavePointGuage;
        [SerializeField]
        private Image[] arrRemainPointImage;
        public void SummonerBox(string name)
        {
            remainText.text = "0";
            Image[] PointInvestImage = new Image[18];
            Color Black;
            ColorUtility.TryParseHtmlString("#383a40", out Black);
            for (int i = 0; i < arrStatBoxes.Length; i++)
            {
                arrStatBoxes[i].gameObject.SetActive(true);
                arrInvestPoint[i].text = "0";
                arrAdvantageGuage[i].sizeDelta = new Vector2(1, arrAdvantageGuage[i].rect.height);
                arrSavePointGuage[i].value = 0;
                arrStatBoxes[i].transform.Find("LeftArrow").GetComponent<Button>().onClick.RemoveAllListeners();
                arrStatBoxes[i].transform.Find("RightArrow").GetComponent<Button>().onClick.RemoveAllListeners();

                
                for (int x = 0; x < 3; x++)
                {
                    PointInvestImage[i * 3 + x] = arrStatBoxes[i].transform.GetChild(6 + x).GetComponent<Image>();
                    PointInvestImage[i * 3 + x].color = Black;
                }
            }
            Color HardBlack;
            ColorUtility.TryParseHtmlString("#22252a", out HardBlack);

            for (int i = 0; i< arrRemainPointImage.Length;i++)
            {
                arrRemainPointImage[i].color = HardBlack;
            }
            //-------------------------------------초기화----------------------------------------------------
            var summonerInfo = summonerManager.GetSummonerInfo();

           
            for (int i = 0; i < summonerInfo.Count; i++)
            {
                if (summonerInfo[i].name == name)
                {
                    remainText.text = summonerInfo[i].remainPoint + "/3";

                    for (int z = 0; z < arrStatBoxes.Length; z++)
                    {
                        arrInvestPoint[z].text = summonerInfo[i].investPoint[z].ToString();
                        int Index = z;
                        arrStatBoxes[z].transform.Find("LeftArrow").GetComponent<Button>().onClick.AddListener
                            (delegate { GetIndexCount(arrStatBoxes[Index].transform,"LeftArrow",summonerInfo[i],PointInvestImage);});
                        arrStatBoxes[z].transform.Find("RightArrow").GetComponent<Button>().onClick.AddListener
                            (delegate { GetIndexCount(arrStatBoxes[Index].transform,"RightArrow",summonerInfo[i],PointInvestImage);});
                    }

                    ChangeRemainPointColor(summonerInfo[i]);
                    ChangePointColor(summonerInfo[i], PointInvestImage);

                    for (int z = summonerInfo[i].mainHero.Count; z <= 2; z++)
                    {
                        arrStatBoxes[z + 3].gameObject.SetActive(false);
                    }

                    for (int z = 0; z < arrAdvantageGuage.Length; z++)
                    {
                        float advantageValue = summonerInfo[i].adventagePoint[z];
                        arrAdvantageGuage[z].sizeDelta = new Vector2(165 * advantageValue, arrAdvantageGuage[i].rect.height);
                        arrSavePointGuage[z].value = summonerInfo[i].savePoint[z];
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

                    for (int i = 0; i < arrRemainPointImage.Length; i++)
                    {
                        arrRemainPointImage[i].color = HardBlack;
                    }

                    if (SummonerInfo.remainPoint > 0)
                    {
                        SummonerInfo.investPoint[Index] += 1;
                        SummonerInfo.remainPoint -= 1;
                        
                    }
                    break;
           }

           arrInvestPoint[Index].text = SummonerInfo.investPoint[Index].ToString();
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
                arrRemainPointImage[z].color = Yellow;
            }
        }

    }
}
