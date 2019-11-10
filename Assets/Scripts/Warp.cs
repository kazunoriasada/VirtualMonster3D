using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warp : MonoBehaviour
{
    public Transform EnemyPosition;
    public Transform PlayerPositon;
    
    // GameObject Fade;
    // Fade script;

    // public float speed = 0.01f;  //透明化の速さ
    // float alfa;    //A値を操作するための変数
    // float red, green, blue;    //RGBを操作するための変数
    //フェードスピード
	//public float fadeSpeed = 2.0f;
     //フェードイメージ
    //public Image fadeImage;
    void Start()
    {
        //panelの色を取得
        // red = GetComponent<Image>().color.r;
        // green = GetComponent<Image>().color.g;
        // blue = GetComponent<Image>().color.b;
        
    }
    
    void OnTriggerEnter(Collider col) 
    {
        

        if(col.tag == "Enemy") 
        {
			//キャラクターの状態をワープ状態に変更
			//col.GetComponent <WarpChara>().SetState(WarpChara.WarpCharaState.goToWarpPoint, transform, warpPoint);
            transform.position = EnemyPosition.position;
		}
        if(col.tag == "Player")
        {
            //キャラクターの状態をワープ状態に変更
			//col.GetComponent <WarpChara>().SetState(WarpChara.WarpCharaState.goToWarpPoint, transform, warpPoint);
            transform.position = PlayerPositon.position;
            transform.rotation = PlayerPositon.rotation;
        }
     
	}
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
