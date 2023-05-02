using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace homework
{
   class IngameManager : MonoBehaviour
   {
        UImanager uiManager = new UImanager();
        [SerializeField] private CharacterJsonRead characterJsonRead;

        private void Awake()
        {
            characterJsonRead.Init();
        }
    }
}
