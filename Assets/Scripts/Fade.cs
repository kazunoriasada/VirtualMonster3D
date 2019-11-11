using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Transform EnemyPosition;
    public Transform PlayerPositon;

    //死んだかどうか
    private bool isDead;
    //落とすアイテムゲームオブジェクト
    public GameObject dropItemObj;

    
    //フェードスピード
	//public float fadeSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        // fadeImage = GetComponent<Image> ();
		// red = fadeImage.color.r;
		// green = fadeImage.color.g;
		// blue = fadeImage.color.b;
		// alfa = fadeImage.color.a;
    }

    
    void OnTriggerEnter(Collider col) 
    {
			
        if(col.tag == "Enemy") 
        {
            Debug.Log(Camera.main.gameObject.name);
            Camera.main.gameObject.GetComponent<CameraFade>().FadeIn();
            
            
            //キャラクターの状態をワープ状態に変更
			//col.GetComponent <WarpChara>().SetState(WarpChara.WarpCharaState.goToWarpPoint, transform, warpPoint);
            transform.position = EnemyPosition.position;
            
            Camera.main.gameObject.GetComponent<CameraFade>().FadeOut();
		}
        if(col.tag == "Player")
        {
            //キャラクターの状態をワープ状態に変更
			//col.GetComponent <WarpChara>().SetState(WarpChara.WarpCharaState.goToWarpPoint, transform, warpPoint);
            transform.position = PlayerPositon.position;
            transform.rotation = PlayerPositon.rotation;

            
            //アイテムのドロップ
            // if(Random.Range (0, 1) == 0) 
            // {
            //     Instantiate (dropItemObj, transform.position, transform.rotation);
                

            // }
				
            //Instantiate<GameObject>(dropItemObj, transform.position + Vector3.up, Quaternion.identity);
            
            // if (!isDead) 
            // {
                
            //     isDead = true;
			//     //設定したアイテムを敵の1m上から落とす
			//     Instantiate<GameObject>(dropItemObj, transform.position + Vector3.up, Quaternion.identity);

            // }
			
        }

        //敵キャラの破壊
            //Destroy(gameObject,5f);
     
	}

    void Update()
    {
       
    //     if (isFadeOut) 
    //     {
	// 		StartFadeOut ();
	// 	}
    // }
    // void StartFadeOut()
    // {
	// 	fadeImage.enabled = true;  // a)パネルの表示をオンにする
	// 	alfa += fadeSpeed;         // b)不透明度を徐々にあげる
    //     Debug.Log("fadeimage");
	// 	SetAlpha ();               // c)変更した透明度をパネルに反映する
	// 	if(alfa >= 1)
    //     {             // d)完全に不透明になったら処理を抜ける
	// 		isFadeOut = false;
	// 	}
	// }
    // void SetAlpha()
    // {
	// 	fadeImage.color = new Color(red, green, blue, alfa);
	}
    // public void OnDestroy()
    // {
    //     if(gameObject.tag == "Enemy") 
    //     {}
    //     Debug.Log("OnDestroy");
    // }

}
