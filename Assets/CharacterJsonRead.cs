using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJsonRead : MonoBehaviour
{

    public CharacterStateList characterStateList = new CharacterStateList();
    public TextAsset textJson;
    // Start is called before the first frame update
    void Start()
    {
        characterStateList = JsonUtility.FromJson<CharacterStateList>(textJson.text);
        //Debug.Log(aa.characterState[0].Name);
    }

    [System.Serializable]
    public class CharacterStateList
    {
        public CharacterState[] characterState;
    }
    [System.Serializable]
    public class CharacterState
    {
        public int indexCharacter;
        public string Name;
        public int Hp;
        public int Attack;
        public float attackSpeed;
        public float moveSpeed;
        public CharacterType characterType;
        public float AttackRange;
    }

    // Update is called once per frame

}
