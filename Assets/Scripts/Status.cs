using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour//,exp_tekiatk_cs インターフェースはデザインパターンの中でよく使われる
{
    //static = シーンがかわっても消えない, そのためNullを入れないとならない
    public static int Lv_p = 1;
    //Sliderの部分と重複するため、挙動の精査が必要！
    public static float HP = 100;
    public static float HP_max = 100;
    public static float ATK = 10;
    public static float EXP_rui = 0; //経験値（累計）
    public static float EXP_nextrui = 20; //次の経験値（累計）
    public static float EXP_next = 20;
 
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

 // Use this for initialization
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //敵のキャラに付けるやつかもしれない
        //  if (tama_hp <= 0)
        //  {
        //     var yoexp_tama = GameObject.Find("Status").GetComponent<exp_tekiatk_cs>();  
 
        //     yoexp_tama.yoexp_I(10); 
        //     //Destroyで破棄する前にこれやっとかないとと思った         
        //     Destroy(this.gameObject);
        //     //ｈｐがゼロより少なくなったら、消去

        //  }
         if (EXP_rui >= EXP_nextrui)
         {
            Lv_p += 1;
            //Mathf.RoundToInt = こうすると四捨五入される
            EXP_nextrui = Mathf.RoundToInt(EXP_nextrui * 1.2f);
            ATK = Mathf.RoundToInt(ATK * 1.2f);
            HP_max = Mathf.RoundToInt(HP_max * 1.2f);
            HP = HP_max;//レベルアップしたら体力満タンにしたかったから
         }
            
        
    }
//     void exp_tekiatk_cs.yoexp_I(int yoexp)    
//     {                                  
//         EXP_rui += yoexp;
//     }

//     void exp_tekiatk_cs.yoatk_I(int yoatk)
//     {                                  
//         HP -= yoatk;
// 　　　　Debug.Log("あいてー");

//     }
//     private void OnCollisionEnter(Collision col)
//     {
//         if (col.gameObject.tag == "Player")  //ここも修正
//         {
//             var atkatk = GameObject.Find("Status").GetComponent<exp_tekiatk_cs>();//ここも修正
//             if (atkatk != null)
//             {
//                 atkatk.yoatk_I(10);
//             }
//         }

//     }
}
