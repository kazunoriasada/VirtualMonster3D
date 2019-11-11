using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFade : MonoBehaviour
{
     /// <summary>フェード中の透明度</summary>
	private float fadeAlpha = 0;
	/// <summary>フェード中かどうか</summary>
	private bool isFading = false;
	/// <summary>フェード色</summary>
	public Color fadeColor = Color.black;
    /// <param name='interval'>暗転にかかる時間(秒)</param>///  
    float time = 0;
    float interval = 5.0f;
    //[SerializeField]
    public Image image;  

    void Start()
    {
        fadeColor = image.color;
    }
    
    // Fadeの設定
    //カメラにつけるとメインカメラを直で呼べる = ユニティがカメラを持っている（シングルトン）
    public void  FadeIn() 
    {
        Debug.Log("FadeIn");
        // if (this.isFading) 
        // {
        //色と透明度を更新して白テクスチャを描画 .
        // this.fadeColor.a = this.fadeAlpha;
        // image.color = fadeColor;

            // GUI.color = this.fadeColor;
            // GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            //だんだん暗く .
		this.isFading = true;
		float time = 0;
		while (time <= interval) 
        {
            this.fadeAlpha = Mathf.Lerp (0f, 1f, time / interval);      
			time += Time.deltaTime;

            this.fadeColor.a = this.fadeAlpha;
            image.color = fadeColor;
			//yield return 0;
		}

        // }		

    }
    //非同期で処理するもの = 別軸で動くもの
    //同期とは始めがあり終わりがある
    //float intervalとFadeOutWithTimerの(FadeOutの中身)
    public void FadeOut ()
	{
		StartCoroutine (FadeOutWithTimer ());
	}
    private IEnumerator FadeOutWithTimer ()
	{
		
		//だんだん明るく .
		time = 0;
		while (time <= interval) 
        {
			yield return new WaitForSeconds(5.0f);
            this.fadeAlpha = Mathf.Lerp (1f, 0f, time / interval);
			time += Time.deltaTime;
            this.fadeColor.a = this.fadeAlpha;
            image.color = fadeColor;
			//yield return 0;
		}
		
		this.isFading = false;
	}
}
