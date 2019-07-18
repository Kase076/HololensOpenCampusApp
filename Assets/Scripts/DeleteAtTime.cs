using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAtTime : MonoBehaviour {

    [SerializeField]
    private float time_limit;

    private float time;

	// Use this for initialization
	void Start () {
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if (time >= time_limit) {
            Destroy(this.gameObject);
        }
	}
}