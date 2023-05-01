using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJsonRead : MonoBehaviour
{

    public CharacterStateList characterStateList = new CharacterStateList();
    public TextAsset textJson;
    // Start is called before the first frame update
    public void Init()
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
        public string positionName;
        public string characterName;
        public string Unlock;
        public int healthPoint;
        public int Attack;
        public float attackSpeed;
        public float moveSpeed;
        public CharacterType characterType;
        public float attackRange;
    }

    // Update is called once per frame

}
