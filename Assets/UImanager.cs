using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace homework
{
    public class UImanager : MonoSingleton<UImanager>
    {
        public Stack <GameObject> stUI = new Stack<GameObject>();

        public GameObject objUIStatus;

        public GameObject objUIOption;

        public GameObject objUISound;

        
        private void Start()
        {
            GameObject.Find("StateButton").GetComponent<Button>().onClick.AddListener(() => OnBtn_UI(objUIStatus));

            GameObject.Find("OptionButton").GetComponent<Button>().onClick.AddListener(() => OnBtn_UI(objUIOption));

            GameObject.Find("SoundButton").GetComponent<Button>().onClick.AddListener(() => OnBtn_UI(objUISound));
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if (stUI.Count > 0)
                {
                    stUI.Pop().gameObject.SetActive(false);
                }

            }
        
        }

        public void OnBtn_UI(GameObject obj_UI)
        {
            if (obj_UI.activeInHierarchy == false)
            {
                stUI.Push(obj_UI);
                obj_UI.gameObject.SetActive(true);

                Debug.Log(obj_UI + "dd");
            }

        }
    }
}
