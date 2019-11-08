using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//NaveMeshAgent使うのに必要
using UnityEngine.AI;

public class Move : MonoBehaviour
{
    //先頭のキャラの操作を切り替える
    public bool lead = false;
    //別のクラスからデータを代入する
    //自分の前のキャラを入れる
    public GameObject target;
    //自分の前のキャラの位置を入れてそこにワープする
    public Vector3 movePos;

    //先ほど作成したJoystick
    // [SerializeField]
    // private Joystick _joystick = null;
    // //移動速度
    // private const float SPEED = 0.1f;

    //移動スピード
    float speed = 2.5f;
    //方向転換のスピード
    float angleSpeed = 200;

    
    

    //Animatorを収納する変数
    Animator animator;

    //NaveMeshAgentを収納する変数
    NavMeshAgent agent;
    
    void Start () 
    {
		//AnimatorのComponentを取得する
        animator = GetComponent<Animator>();

        //NaveMeshAgentのComponentを取得する
        agent = GetComponent<NavMeshAgent>();
	}
	
	void Update () 
    {
		//ジョイスティックの操作
        // Vector3 pos = transform.position;

        // pos.x += _joystick.Position.x * SPEED;
        // pos.z += _joystick.Position.y * SPEED;

        // transform.position = pos;
        // GetComponent<Animator> ().SetFloat ("X", _joystick.Position.x);
        // GetComponent<Animator> ().SetFloat ("Z", _joystick.Position.y);

        //キャクターがプレイヤーについてくる
        if (lead)
        {
            //NavMeshAgentを止める
            agent.isStopped = true;

            
            //WSキー、↑↓キーで移動する
            float z = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;
            transform.position += transform.forward * z;

            //ADキー、←→キーで方向を替える
            float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * angleSpeed;
            transform.Rotate(Vector3.up * x);
            

            //Agentの速度の二乗の数値でアニメーションを切り替える
            animator.SetFloat("Blend", z * 100);
            
        }
        else
        {
            //NavMeshAgentの停止をやめる
            agent.isStopped = false;

            //付いていくキャラと自分の位置との距離
            Vector3 dir = target.transform.position - this.transform.position;
            //目的の位置
            Vector3 pos = this.transform.position + dir * 1.5f;
            //目的地の方を向く
            this.transform.rotation = Quaternion.LookRotation(dir);
            //目的地をNavMeshAgentに指定する
            agent.destination = pos;
            //目的地からどれくらい離れて停止するか
            agent.stoppingDistance = 3f;
            //Agentの速度の二乗の数値でアニメーションを切り替える
            //animator.SetFloat("Blend", agent.velocity.sqrMagnitude);
        }

        
        
	}

    //別のクラスから呼び出して自分が移る位置を代入する
    public void TargetPos()
    {
        movePos = target.transform.position;
    }
    
}
