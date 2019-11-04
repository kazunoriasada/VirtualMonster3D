using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;	// uGUIの機能を使うお約束

public class TextController : MonoBehaviour
{
    [SerializeField]
    public string[] scenarios; // シナリオを格納する
	[SerializeField]
    public Text uiText;	// uiTextへの参照を保つ

    [SerializeField][Range(0.001f, 0.3f)]
	float intervalForCharacterDisplay = 0.05f;	// 1文字の表示にかかる時間

    int currentLine = 0; // 現在の行番号
    private string currentText = string.Empty;	// 現在の文字列
	private float timeUntilDisplay = 0;		// 表示にかかる時間
	private float timeElapsed = 1;			// 文字列の表示を開始した時間
	private int lastUpdateCharacter = -1;		// 表示中の文字数

    // 文字の表示が完了しているかどうか
	public bool IsCompleteDisplayText 
	{
		get { return  Time.time > timeElapsed + timeUntilDisplay; }
	}
    
    // Start is called before the first frame update
    void Start()
    {
        //TextUpdate();
        SetNextLine();
    }

    // Update is called once per frame
    void Update()
    {
        if( IsCompleteDisplayText )
        {
            // 現在の行番号がラストまで行ってない状態でクリックすると、テキストを更新する
            if(currentLine < scenarios.Length && Input.GetMouseButtonDown(0))
            {
                //TextUpdate();
                SetNextLine();
            }
        }
        else
        {
            // 完了してないなら文字をすべて表示する
			if(Input.GetMouseButtonDown(0))
            {
				timeUntilDisplay = 0;
            }
        }
            
        //Mathfクラス = 様々な計算関数がまとめられているクラス
        //公式：https://docs.unity3d.com/ja/current/ScriptReference/Mathf.html
        //Time.time = アプリケーション起動からの経過時間[ms]が格納されている
        // クリックから経過した時間が想定表示時間の何%か確認し、表示文字数を出す
		int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);
		
		// != = 非等値演算子 → そのオペランドが等しくない場合には true を返し、それ以外の場合は false を返す
        // 表示文字数が前回の表示文字数と異なるならテキストを更新する
		if( displayCharacterCount != lastUpdateCharacter )
        {
			uiText.text = currentText.Substring(0, displayCharacterCount);
			lastUpdateCharacter = displayCharacterCount;
		}
    }
    // テキストを更新する
	// void TextUpdate()
	// {
	// 	// 現在の行のテキストをuiTextに流し込み、現在の行番号を一つ追加する
	// 	uiText.text = scenarios[currentLine];
	// 	currentLine ++;
	//}

    void SetNextLine()
	{
		currentText = scenarios[currentLine];
		currentLine ++;
		
		// 想定表示時間と現在の時刻をキャッシュ
		timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
		timeElapsed = Time.time;
		
		// 文字カウントを初期化
		lastUpdateCharacter = -1;
	}
}
