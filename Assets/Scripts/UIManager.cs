using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Framework.UI
{
    public enum UIName : int
    {
        MainHUD = 1,
        LobbyUI,
    }

    public partial class UIManager
    {
        //const �� ������ Ÿ�ӿ� ���ȭ, staticȭ
        //readonly�� ��Ÿ�ӿ� ���ȭ, �Ϲ� ��� ����ȭ
        readonly List<ManagementUIBase> _uiList = new List<ManagementUIBase>();

        public Canvas Canvas { get; set; }
        public EventSystem EventSystem { get; set; }
        /// <summary>
        /// UI������ Resources.LoadAsync �Ἥ �񵿱�� �����ϰ� ó���ϰ� delegate�� ���� �Ŀ� �˸��� �ް� ����
        public void CreateUI<T>(string path,UIName uiName,Action<T> callbackCreate) where T : ManagementUIBase
        {
            if ((int)uiName == 1)
            {
                GameManager.Instance.StartCoroutine(CoroutineCreate<T>(path, uiName, callbackCreate));
            }
            else if ((int)uiName == 2)
            {
                GameManager.Instance.StartCoroutine(CoroutineLobbyButtonCreate<T>(path, uiName, callbackCreate));
            }
        }

        IEnumerator CoroutineCreate<T>(string path, UIName uiName, Action<T> callbackCreate) where T : ManagementUIBase
        {
            ResourceRequest request = Resources.LoadAsync<GameObject>(path);
            yield return request;
            if(request.asset != null)
            {
                GameObject newObject = GameObject.Instantiate<GameObject>(request.asset as GameObject, Canvas.transform);
                T ui = newObject.GetComponent<T>();

                AddUI(ui, uiName);

                callbackCreate(ui);
            }
        }

        IEnumerator CoroutineLobbyButtonCreate<T>(string path, UIName uiName, Action<T> callbackCreate) where T : ManagementUIBase
        {
            ResourceRequest request = Resources.LoadAsync<GameObject>(path);
            yield return request;
            if (request.asset != null)
            {
                GameObject newObject = GameObject.Instantiate<GameObject>(request.asset as GameObject, GameObject.Find("DownUIButtons").transform);
                T ui = newObject.GetComponent<T>();

                AddUI(ui, uiName);

                callbackCreate(ui);
            }
        }

        public void AddUI(ManagementUIBase newUI, UIName uiName)
        {
            if (newUI == null)
                return;
            if (IsContain(newUI))
                return;

            newUI.SetUIName(uiName);

            _uiList.Add(newUI);
        }

        public void RemoveUI(ManagementUIBase ui)
        {
            if (ui == null)
                return;

            _uiList.Remove(ui);

            GameObject.Destroy(ui);
        }

        public void RemoveUI()
        {
            if (_uiList.Count == 0)
                return;

            RemoveUI(_uiList[_uiList.Count - 1]);
        }

        public T GetUI<T>(UIName uiName) where T : ManagementUIBase
        {
            return _uiList.Find(ui => ui.UIName == uiName) as T;
        }

        public bool IsContain(ManagementUIBase ui)
        {
            return _uiList.Contains(ui);
        }
    }
}