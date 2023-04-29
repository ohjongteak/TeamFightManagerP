using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace homework
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        static T _instance;

        public static T Instance
        {
            get
            {
                _instance = GameObject.FindObjectOfType<T>();

                if (Instance == null)
                {
                    _instance = new GameObject().AddComponent<T>(); 
                }

                DontDestroyOnLoad(_instance);
                return _instance;
            }

        }
    }
}
