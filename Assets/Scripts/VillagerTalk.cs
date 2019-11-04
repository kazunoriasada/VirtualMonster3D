// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class VillagerTalk : MonoBehaviour
// {
//     //会話のUI
//     [SerializeField]
//     private GameObject talkUI;
//     //主人公キャラクター操作スクリプト
//     [SerializeField]
//     private MoveAndTalkChara moveAndTalkChara;
//     //今見ている文字番号
//     private int nowTextNum = 0;

//     [SerializeField]
//     [TextArea(1, 10)]
//     private string talkMessage = "初期メッセージ";

//     //1回分のメッセージを表示したかどうか
//     private bool isOneMessage = false;
//     //メッセージをすべて表示したかどうか
//     private bool isEndMessage = false;

//     // Start is called before the first frame update
//     void Start()
//     {
//         messageText.text = "";
//         SetMessage(talkMessage);
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         //メッセージが終わっていない、または設定されていない、または会話が開始されていない
//         if (isEndMessage || message == null || !isTalk) 
//         {
//             return;
//         }
 
//         //1回に表示するメッセージを表示していない	
//         if (!isOneMessage) 
//         {
 
//             //その他処理
 
//             //メッセージ表示中にマウスの左ボタンを押したら一括表示
//             if (Input.GetButtonDown("Action")) 
//             {
 
//                 //その他処理
//                 Input.ResetInputAxes();
//             }
//         //1回に表示するメッセージを表示した
//         } 
//         else
//         {
 
//             //その他処理
//             //マウスクリックされたら次の文字表示処理
//             if (Input.GetButtonDown("Action")) 
//             {
 
//                 //その他処理
//                 //メッセージが全部表示されていたら通常
//                 if (nowTextNum >= message.Length) 
//                 {
//                     nowTextNum = 0;
//                     isEndMessage = true;
 
//                     Debug.Log("通常");
//                     talkUI.SetActive(false);
//                     isTalk = false;
//                     moveAndTalkChara.SetState(MoveAndTalkChara.CharacterState.normal);
//                 }
//                 Input.ResetInputAxes();
//             }
//         }
//     }
//     //現在会話中かどうか
//     private bool isTalk;
    
//     private void OnTriggerStay(Collider other) 
//     {

//         if(other.tag == "Player") 
//         {
//             //他の人と会話している場合は何もしない
//             if (moveAndTalkChara.GetState() == MoveAndTalkChara.CharacterState.talk) 
//             {
//                 return;
//             }
//             // Actionに割り当てたキーで会話開始と終了
//             if (Input.GetButtonDown("Action")) 
//             {
//                 if (!isTalk) 
//                 {
//                     Debug.Log("会話");
//                     //会話リセットメソッドを読んで会話を初期化する
//                     InitializeTalk();

//                     //1フレームに何度もキー押し判定されないようにリセット
//                     Input.ResetInputAxes();
//                 }
//             }
//         }
//     }
//     //会話を初期化する
//     void InitializeTalk() 
//     {
//         isOneMessage = false;
//         isEndMessage = false;
//         moveAndTalkChara.SetState(MoveAndTalkChara.CharacterState.talk);
//         talkUI.SetActive(true);
//         isTalk = true;
//     }

    
// }
