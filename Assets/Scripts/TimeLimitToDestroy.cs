using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLimitToDestroy : MonoBehaviour {

    GameObject game_obj;
    FireAI fire_ai;

	// Use this for initialization
	void Start () {
        game_obj = this.gameObject;
        fire_ai = game_obj.GetComponent<FireAI>();
    }

    // Update is called once per frame
    void Update () {
        if (TimeLimitController.TimeLimit < 0) {
            //Destroy(gameObject);
            fire_ai.DestroyedFlag = true;
        }
	}
}
