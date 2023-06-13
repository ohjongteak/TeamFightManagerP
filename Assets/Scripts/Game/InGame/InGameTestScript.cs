using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTestScript : MonoBehaviour
{
    [SerializeField] GameObject objTest;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(objTest.transform.position, transform.position) <= 0.3f)
            Debug.Log("11111111111");
    }
}
