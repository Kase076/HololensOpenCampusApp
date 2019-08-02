using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField]
    private GameObject fireball;

    [SerializeField] private float frontdistance;  //炎の生成範囲（前）
    [SerializeField] private float sidedistance;   //炎の生成範囲（横）

    public bool EndFlag = false;

    // Use this for initialization
    void Start () {
        Invoke("InsFire", 3.5f);
        Invoke("InsFire", 4.0f);
        Invoke("InsFire", 4.5f);
    }
	
	// Update is called once per frame
	void Update () {
        if (!EndFlag)
        {
            if (FireMeta.FireList.Count < 3)
            {
                InsFire();
            }
        }
    }
    
    public void InsFire() {
        var new_fire = GameObject.Instantiate(fireball);
        float x = Random.Range(-sidedistance, sidedistance);
        float y = 0.2f;
        float z = Random.Range(1.0f, frontdistance);
        new_fire.transform.position = new Vector3(x, y, z);
        FireMeta.FireList.Add(new_fire);
    }
}
