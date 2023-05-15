using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChampionListButtonChange : MonoBehaviour
{
    [SerializeField]
    private Image thisImage;
    [SerializeField]
    private Outline outline;


    public CharacterType myCharacterType;
    // Start is called before the first frame update

    private void OnMouseEnter()
    {
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        outline.enabled = false;
    }

    public void ChangeBoxSize()
    {
        Transform[] temp = new Transform[this.transform.parent.childCount];

       for(int i = 0; i <temp.Length; i++)
       {
            temp[i] = this.transform.parent.GetChild(i);
            temp[i].transform.localScale = new Vector3(0.2f, 0.2f, this.transform.localScale.z);
       }


        this.transform.localScale = new Vector3(this.transform.localScale.x *1.2f, this.transform.localScale.y * 1.2f, this.transform.localScale.z);

    }

   
}
