using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //先頭のキャラの操作を切り替える
    public bool lead = false;
    //別のクラスからデータを代入する
    //自分の前のキャラを入れる
    public GameObject target;
    //自分の前のキャラの位置を入れてそこにワープする
    public Vector3 movePos;

    //移動スピード
    float speed = 2.5f;
    //方向転換のスピード
    float angleSpeed = 200;

    //Animatorを収納する変数
    Animator animator;
    
    void Start () 
    {
		//AnimatorのComponentを取得する
        animator = GetComponent<Animator>();
	}
	
	void Update () 
    {
		if (lead)
        {
            //WSキー、↑↓キーで移動する
            float z = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;
            transform.position += transform.forward * z;

            //ADキー、←→キーで方向を替える
            float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * angleSpeed;
            transform.Rotate(Vector3.up * x);

            //Agentの速度の二乗の数値でアニメーションを切り替える
            animator.SetFloat("Blend", z * 100);
        }
	}

    //別のクラスから呼び出して自分が移る位置を代入する
    public void TargetPos()
    {
        movePos = target.transform.position;
    }
    
}
