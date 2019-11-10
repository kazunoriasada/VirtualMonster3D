using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Transform EnemyPosition;
    public Transform PlayerPositon;
    
    float alfa;
    float speed = 0.01f;
    //フェードスピード
	public float fadeSpeed = 2.0f;
    float red, green, blue;

    public bool isFadeOut = false;  //フェードアウト処理の開始、完了を管理するフラグ
	public bool isFadeIn = false;   //フェードイン処理の開始、完了を管理するフラグ

    Image fadeImage;
    // Start is called before the first frame update
    void Start()
    {
        fadeImage = GetComponent<Image> ();
		// red = fadeImage.color.r;
		// green = fadeImage.color.g;
		// blue = fadeImage.color.b;
		// alfa = fadeImage.color.a;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider col) 
    {

        
        if(col.tag == "Enemy") 
        {
			 if (isFadeOut)
             {
                 StartFadeOut ();
             }
            //キャラクターの状態をワープ状態に変更
			//col.GetComponent <WarpChara>().SetState(WarpChara.WarpCharaState.goToWarpPoint, transform, warpPoint);
            transform.position = EnemyPosition.position;
		}
        if(col.tag == "Player")
        {
            if (isFadeOut)
             {
                 StartFadeOut ();
             }
            //キャラクターの状態をワープ状態に変更
			//col.GetComponent <WarpChara>().SetState(WarpChara.WarpCharaState.goToWarpPoint, transform, warpPoint);
            transform.position = PlayerPositon.position;
            transform.rotation = PlayerPositon.rotation;
        }
     
	}
    // void Update()
    // {
       
    //     if (isFadeOut) 
    //     {
	// 		StartFadeOut ();
	// 	}
    // }
    void StartFadeOut()
    {
		fadeImage.enabled = true;  // a)パネルの表示をオンにする
		alfa += fadeSpeed;         // b)不透明度を徐々にあげる
        Debug.Log("fadeimage");
		SetAlpha ();               // c)変更した透明度をパネルに反映する
		if(alfa >= 1)
        {             // d)完全に不透明になったら処理を抜ける
			isFadeOut = false;
		}
	}
    void SetAlpha()
    {
		fadeImage.color = new Color(red, green, blue, alfa);
	}
}
