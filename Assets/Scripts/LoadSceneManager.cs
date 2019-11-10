using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    //フェードスピード
	public float fadeSpeed = 2.0f;
    //フェードイメージ
    public Image fadeImage;
	//アンロードするシーン
	private Scene unLoadScene;
	//戦闘終了ボタン
//	public GameObject button;
	//シーン用データ
	private SceneData sceneData;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        //最初にWorldシーンを読み込む
		yield return LoadNewScene ("World");
		//SceneDataを保持
		sceneData = FindObjectOfType (typeof(SceneData)) as SceneData;
    }
    public void FadeAndLoadScene(string sceneName) 
    {
		//Coroutine = 非同期処理を行ってくれる
		//個々のシーンのデータを取得(sceneNameの中に該当するシーンの名前を入れる形の可能性あり)
		StartCoroutine (LoadScene(sceneName));
	}

    IEnumerator LoadScene(string sceneName) 
    {
 
		//戦闘終了ボタンが設定されていれば無効
		if (sceneData.button != null) 
        {
			sceneData.button.SetActive (false);
		}
		//現在のシーンデータを取得
		sceneData = FindObjectOfType (typeof(SceneData)) as SceneData;
		//他のシーンへ遷移する時にフェードアウト
		yield return StartCoroutine (Fade(sceneData.fadeImage, 1f));
 
		Destroy (FindObjectOfType (typeof(AudioListener)));
		unLoadScene = SceneManager.GetActiveScene ();
		//フェードアウトが完了したら新しいシーンを読み込む
		yield return StartCoroutine(LoadNewScene(sceneName));
		//フェードアウトが完了したら前のシーンをアンロード
		yield return StartCoroutine(UnLoadScene());
 
		//現在のシーンデータを取得
		sceneData = FindObjectOfType (typeof(SceneData)) as SceneData;
 
		//Battleシーンの時だけフェードイン
		if (SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("Battle")) 
        {
			yield return StartCoroutine (Fade (sceneData.fadeImage, 0f));
		}
 
		if (sceneData.button != null) 
        {
			//フェードイン後に戦闘終了ボタンを有効にする
			if (SceneManager.GetActiveScene ().buildIndex == SceneManager.GetSceneByName ("Battle").buildIndex) 
            {
				sceneData.button.SetActive (true);
			} 
            else 
            {
				sceneData.button.SetActive (false);
			}
		}
	}
    public IEnumerator Fade(Image fadeImage, float alpha) 
    {
 
		//目的のアルファ値になるまで徐々に変化させる
		while (!Mathf.Approximately (fadeImage.color.a, alpha)) 
        {
			fadeImage.color = new Color (0f, 0f, 0f, Mathf.MoveTowards (fadeImage.color.a, alpha, fadeSpeed * Time.deltaTime));
			yield return null;
		}
	}
    //新しいシーンをロード
	IEnumerator LoadNewScene(string sceneName) 
    {
 
		//シーン読み込み処理
		AsyncOperation async = SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);
		while (!async.isDone) 
        {
			yield return null;
		}
		SceneManager.SetActiveScene (SceneManager.GetSceneAt (SceneManager.sceneCount - 1));
 
		if (SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("World")) 
        {
			(FindObjectOfType (typeof(GenerateEnemy)) as GenerateEnemy).InstantiateEnemy ();
		}
	}
 
	//シーンのアンロード
	IEnumerator UnLoadScene() 
    {
		yield return SceneManager.UnloadScene (unLoadScene.buildIndex);
	}
    // Update is called once per frame
    void Update()
    {
        
    }
}
