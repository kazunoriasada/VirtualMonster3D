using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMyData : MonoBehaviour
{
    private CharaStatus data;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown ("Fire1")) 
        {
			data = ScriptableObject.CreateInstance <CharaStatus> ();
			data.myName = "Name";
			data.hp = Random.Range (10, 50);
			data.attackPower = 10f;
			Debug.Log (data.myName + " : " + data.hp + " : " + data.attackPower);
		} 
        else if(Input.GetButtonDown("Fire2")) 
        {
			Debug.Log (data.myName + " : " + data.hp + " : " + data.attackPower);
		}
	
    }
}
