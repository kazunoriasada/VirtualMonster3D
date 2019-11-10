using System.Collections;
using UnityEngine;

//　キャラクター毎のステータス
[CreateAssetMenu(fileName = "CharacterStatus", menuName = "CreateCharacterStatus")]
public class CharaStatus : ScriptableObject
{
    public string charaName;
	public int hp;
	public int mp;
	public float attackPower;
	public int speed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
