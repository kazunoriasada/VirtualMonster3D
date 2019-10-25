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
    
    void Start()
    {
        //NaveMeshAgentのComponentを取得する
        agent = GetComponent<NavMeshAgent>();
        //AnimatorのComponentを取得する
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        //目的地と自分の位置との距離
        Vector3 dir = unityChan.transform.position - this.transform.position;
        //目的地の位置
        Vector3 pos = this.transform.position + dir * 1.5f;
        //目的地の方を向く
        this.transform.rotation = Quaternion.LookRotation(dir);
        //目的地を指定する
        agent.destination = pos;
        //目的地からどれくらい離れて停止するか
        agent.stoppingDistance = 3f;
        //Agentの速度の二乗の数値でアニメーションを切り替える
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);

    }
}
