using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    [SerializeField]
    private GameObject fireball;

    // Use this for initialization
    void Start () {
        Invoke("InsFire", 3.5f);
        Invoke("InsFire", 4.0f);
        Invoke("InsFire", 4.5f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InsFire() {
        var new_fire = GameObject.Instantiate(fireball);
        Vector3 tmp = this.transform.position;
        float x = tmp.x + Random.Range(-1.0f, 1.0f);
        float y = tmp.y + 0.1f;
        float z = tmp.z + Random.Range(-1.0f, 1.0f);
        new_fire.transform.position = new Vector3(x, y, z);
        FireMeta.FireList.Add(new_fire);
    } 
}
