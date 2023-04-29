using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.UI
{
    public class LobbyMainUIButton : MonoBehaviour
    {
        // Start is called before the first frame update

        [SerializeField]
        private GameObject _objButtons;

        [SerializeField]
        private List<float> _objButtonsPositionY;

        [SerializeField]
        private bool Onbtn;


       
        private void Awake()
        {

            for (int i = 0; i < _objButtons.transform.childCount; i++)
            {
                _objButtonsPositionY.Add(_objButtons.transform.GetChild(i).gameObject.transform.localPosition.y);

            }

            
        }
       

        public void OnBtn_UIButton()
        {
            List<GameObject> objButton = new List<GameObject>();

            for (int i = 0; i <_objButtons.transform.childCount; i++)
            {
             
                objButton.Add(_objButtons.transform.GetChild(i).gameObject);
            }
           
            StartCoroutine(CoroutineMoveButton(objButton));

        }

        IEnumerator CoroutineMoveButton(List<GameObject> objButton)
        {
            if (objButton[objButton.Count - 1].transform.localPosition != objButton[objButton.Count / 2 - 1].transform.localPosition && Onbtn == false)
            {
              
                while (objButton[objButton.Count - 1].transform.localPosition != objButton[objButton.Count / 2 - 1].transform.localPosition)
                {
                    for (int i = (objButton.Count / 2); i < objButton.Count; i++)
                    {
                        objButton[i].transform.localPosition = Vector3.MoveTowards(objButton[i].transform.localPosition, objButton[i - ((objButton.Count / 2))].transform.localPosition, 100);
                        yield return new WaitForSeconds(0.001f);
                    }

                    if (objButton[objButton.Count - 1].transform.localPosition == objButton[objButton.Count / 2 - 1].transform.localPosition)
                    {
                        Onbtn = true;
                    }

                }  
            }

            else
            {
               
                while (objButton[objButton.Count - 1].transform.localPosition.y != _objButtonsPositionY[_objButtonsPositionY.Count -1])
                {
                 
                    for (int i = (objButton.Count / 2); i < objButton.Count; i++)
                    {
                        Vector3 target = new Vector3(objButton[i].transform.localPosition.x, _objButtonsPositionY[i], objButton[i].transform.localPosition.z);

                        objButton[i].transform.localPosition = Vector3.MoveTowards(objButton[i].transform.localPosition, target, 100);
                        yield return new WaitForSeconds(0.001f);
                    }

                    if(objButton[objButton.Count - 1].transform.localPosition.y == _objButtonsPositionY[_objButtonsPositionY.Count - 1])
                    {
                        Onbtn = false;
                    }

                }
               
            }
        }
    }
}
