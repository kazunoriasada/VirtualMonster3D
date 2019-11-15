using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFade : MonoBehaviour
{
    public Transform EnemyPosition;
    public Transform PlayerPositon;
    public GameObject dropItemObj;

    public float timeOut = 10.0f;
    private float timeElapsed;
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerEnter(Collider col) 
    {
        if(col.tag == "Player")
        {
            //キャラクターの状態をワープ状態に変更
			//col.GetComponent <WarpChara>().SetState(WarpChara.WarpCharaState.goToWarpPoint, transform, warpPoint);
            transform.position = PlayerPositon.position;
            transform.rotation = PlayerPositon.rotation;

            
            //アイテムのドロップ
            // if(Random.Range (0, 1) == 0) 
            // {
            //     Instantiate (dropItemObj, transform.position, transform.rotation);
                

            // }
				
            //Instantiate<GameObject>(dropItemObj, transform.position + Vector3.up, Quaternion.identity);
            
            // if (!isDead) 
            // {
                
            //     isDead = true;
			//     //設定したアイテムを敵の1m上から落とす
			//     Instantiate<GameObject>(dropItemObj, transform.position + Vector3.up, Quaternion.identity);

            // }
			
        }

        //敵キャラの破壊
            //Destroy(gameObject,5f);
     
    }
    // Update is called once per frame
    void Update()
    {
       timeElapsed += Time.deltaTime;

        if(timeElapsed >= timeOut)
        {
            Instantiate<GameObject>(dropItemObj, transform.position + Vector3.up, Quaternion.identity);
            
            Destroy(gameObject);
        }
    }
}
