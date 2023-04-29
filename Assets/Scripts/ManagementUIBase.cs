using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.UI
{
    public class ManagementUIBase : MonoBehaviour
    {
        protected UIName _uiName;

        public UIName UIName { get { return _uiName; } }
        public void SetUIName(UIName uiName) => _uiName = uiName;
    }
}
