using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testStart : MonoBehaviour
{
    // Start is called before the first frame update
    testStart instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        if (instance != this)
            Destroy(this.gameObject);

        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
    }

    // Update is called once per frame
    
}
