using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
public class testload : MonoBehaviour
{
    AsyncOperationHandle Handle;
    [SerializeField] Image mIMG;

    public void LoadTest()
    {
        Addressables.LoadAssetAsync<Sprite>("LobbyHouse").Completed +=
       (AsyncOperationHandle<Sprite> obj) =>
       {
           Handle = obj;
           mIMG.sprite = obj.Result;

       };

    }

    public void UnLoad()
    {
        Addressables.Release(Handle);
        mIMG.sprite = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadTest();
       
    }

    
}
