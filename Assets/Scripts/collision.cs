using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision : MonoBehaviour
{
   // public Status status;
    // Start is called before the first frame update
    void OnCollisionEnter(Collision other)
    {
         Debug.Log("OnCollisionEnter "+ other.gameObject.tag);

         if (other.gameObject.tag == "PowerUp") //tag=PowerUpの場合は
         {
            // public static void Main() メソッドの中にメソッドを入れてしまっている
            // {
                 
                //collision sta = new collision();
                // int i = status.Lv_p;
                 Status.Lv_p += 1;  //60秒タイマーにプラス
                 
            // }
             Destroy(other.gameObject); //ぶつかったオブジェクトを消す（消さないと回復の泉みたいになりますね）
             
         }
         if (other.gameObject.tag == "PowerUp*2") //tag=PowerUpの場合は
         {
             //public static void Main() 
            // {
                // collision sta = new collision();
                // int i = sta.Lv_p;
                 Status.Lv_p += 2;  //60秒タイマーにプラス
            // }
             //status.Lv_p += 2; //30秒タイマーにプラス
             Destroy(other.gameObject); //ぶつかったオブジェクトを消す（消さないと回復の泉みたいになりますね）
         }
        // public static void Main() 
        //      {
        //          if (other.gameObject.tag == "PowerUp*2") 
        //          {
        //              collision sta = new collision();
        //          int i = sta.Lv_p;
        //          sta.Lv_p += 2;  //60秒タイマーにプラス
                 
        //          Destroy(other.gameObject);

        //          }
                 
        //      }
 }            
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
