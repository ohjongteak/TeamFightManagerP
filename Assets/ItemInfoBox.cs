using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ItemInfoBox : MonoBehaviour , IPointerEnterHandler ,IPointerExitHandler
{
    [SerializeField]
    private Outline outline;

    private void Start()
    {
        outline = this.GetComponent<Outline>();
        
    }

   
    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.enabled = true;
    }

   

    //public void PassThisName()
    //{
    //    trainingSummonerSort.SummonerBox(GetThisName());
    //    trainingManager.SummonerBox(GetThisName());
    //}

}
