using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOrder : MonoBehaviour
{
    //それぞれのキャラクターを入れるリスト
    public List<GameObject> chara = new List<GameObject>();
    //それぞれのキャラクターから取得したスクリプトのリスト
    public List<Move> move = new List<Move>();
    //先頭のキャラクターを表す数字
    int leaderNum;

    void Start()
    {
        //各キャラクターに自分がついていく
        //キャラクターをセットするためのfor文
        //chara.Countはcharaリストの要素数
        for (int i = 0; i < chara.Count; i++)
        {
            //自分の手前のキャラクターの数字のため1を引く
            int moveNum = i - 1;
            Debug.Log(moveNum);
            //0番目のキャラクターの場合は最後尾の数字になる
            //リストの要素数は1から始まり、リストの番号は0から
            //始まるので1を引く
            if (moveNum < 0)
            {
                moveNum = chara.Count - 1;
            }
            //各キャラにセットしたスクリプトのクラスを取得
            Move moveScr = chara[i].GetComponent<Move>();
            //取得したものをmoveリストに入れる
            move.Add(moveScr);
            //Moveスクリプトに各キャラがついていくべきキャラの
            //GameObjectのデータを送る
            move[i].target = chara[moveNum];
        }

        //リーダーは最初は0番目のキャラが務める
        leaderNum = 0;
        //リーダーの操作方法を切り替える
        move[leaderNum].lead = true;
    }
    
    void Update()
    {
        //スペースキーを押すと作動する
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     //まず前のリーダーの操作方法を切り替える
        //     move[leaderNum].lead = false;

        //     //MoveスクリプトのTargetPos()メソッドで
        //     //前のキャラの位置を取得させる
        //     for (int i = 0; i < chara.Count; i++)
        //     {
        //         move[i].TargetPos();
        //     }

        //     //↑のfor文でそれぞれ取得した位置に移動させる
        //     for (int i = 0; i < chara.Count; i++)
        //     {
        //         chara[i].transform.position
        //             = move[i].movePos;
        //     }

        //     //リーダーの番号をひとつ上げる
        //     leaderNum++;
        //     //リーダーの番号がcharaリストの要素数と
        //     //同じになったら最初に戻す
        //     //リストの順番は0から始まり、要素数は1から始まるため
        //     if (leaderNum >= chara.Count)
        //     {
        //         leaderNum = 0;
        //     }
        //     //新リーダーの操作方法を切り替える
        //     move[leaderNum].lead = true;
        //}
    }
    
}
