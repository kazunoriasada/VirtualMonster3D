using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndTalk : MonoBehaviour
{
    //定数 = 変更しない一定の数
    //enum = 複数の定数をひとつにまとめておくことができる型
    public enum CharacterState 
    {
        normal,
        talk
    }

    private CharacterController characterController;
    private Animator animator;
    [SerializeField]
    private float walkSpeed = 1.25f;
    private Vector3 velocity;
    private CharacterState characterState;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterState = CharacterState.normal;
    }

    // Update is called once per frame
    void Update()
    {
        // 通常動作
        if (characterState == CharacterState.normal) {
            Move();
        // 会話中
        } else if(characterState == CharacterState.talk) {
            Talk();
        }
 
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

     void Move() 
     {
        if (characterController.isGrounded) {
            velocity = Vector3.zero;
            var input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
 
            if (input.magnitude > 0f) {
                velocity = input.normalized * walkSpeed;
                animator.SetFloat("WalkSpeed", input.magnitude);
                transform.LookAt(transform.position + input.normalized);
            } else {
                animator.SetFloat("WalkSpeed", 0f);
            }
        }
    }

    //会話中
    void Talk() 
    {
 
    }

    //状態確認メソッド
    public CharacterState GetState() 
    {
        return characterState;
    }
    //状態変更メソッド
    public void SetState(CharacterState setState) 
    {
        characterState = setState;
        //会話の最中は動かない
        if(characterState == CharacterState.talk) 
        {
            velocity = Vector3.zero;
            animator.SetFloat("WalkSpeed", 0f);
        }
    }


}
