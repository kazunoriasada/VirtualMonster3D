using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NaveMeshAgent使うのに必要
using UnityEngine.AI;

public class Following : MonoBehaviour
{
    //目的地の基準になるユニティちゃんの位置
    public Transform unityChan;
    //NaveMeshAgentを収納する変数
    NavMeshAgent agent;
    //Animatorを収納する変数
    Animator animator;

    //　ジャンプ力
	[SerializeField]
	private float jumpPower = 5f;
    
    void Start()
    {
        //NaveMeshAgentのComponentを取得する
        agent = GetComponent<NavMeshAgent>();
        //AnimatorのComponentを取得する
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        //目的地と自分の位置との距離 = ユニティちゃんの位置 - モンスターの位置
        Vector3 dir = unityChan.transform.position - this.transform.position;
        Debug.Log(dir);
        //目的地の位置 = モンスターの位置 + ユニティちゃんとの距離
        Vector3 pos = this.transform.position + dir * 1.5f;

        //モンスターの現在地
        Vector3 mop = this.transform.position;
        //目的地の方を向く
        this.transform.rotation = Quaternion.LookRotation(dir);
        //目的地を指定する
        agent.destination = pos;
        //目的地からどれくらい離れて停止するか
        agent.stoppingDistance = 3f;
        //Agentの速度の二乗の数値でアニメーションを切り替える
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);

         //ついてくるキャラクターが坂を登れるようにする
        if(Input.GetButtonDown("Jump")) 
        {
			mop.y += jumpPower;
            //agent.Warp(new Vector3(x, y, z));

		}
         //プレイヤーがジャンプしたら、合わせてジャンプする
         
         //距離に対して制御を入れる
        //  if (dir.y > 0)
        //     {
        //         mop.y += 1.3f;
        //         transform.position = mop;
        //     }
        // //距離が離れすぎたら、プレイヤーの元まで飛ぶ
        // if(dir.z > 7)
        // {
        //    mop.z = unityChan.transform.position.z;
        //    transform.position = mop;
        //}

    }
}
