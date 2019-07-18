using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMeta : MonoBehaviour {

    public static List<GameObject> FireList = new List<GameObject>();
    public static List<GameObject> WetObject = new List<GameObject>();

    private GameObject gameController;

    private float obj_distance;
    private Vector3 wet_pos, fire_pos;
    private FireAI fire_AI;
    private GameController gmctrl;

    private Vector3 next;
    [SerializeField]
    private float limit_distance;

    int i, j, k = 0;
    //int x = 0;          //デバッグ用

    int TooBad;

    // Use this for initialization
    void Start () {
        Debug.Log("FireMetaAI is ready.");
        TooBad = 0;
        gameController = GameObject.Find("GameController");
        gmctrl = gameController.GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (FireList.Count < 1) {
            gmctrl.InsFire();
        }

        for (j = 0; j < WetObject.Count; j++)
        {
            if (WetObject[j] == null) {
                WetObject.RemoveAt(j);
            }
        }

        for (i = 0; i < FireList.Count; i++)
        {
            fire_AI = FireList[i].GetComponent<FireAI>();
            if (fire_AI.DestroyedFlag)
            {
                Destroy(FireList[i]);
                FireList.RemoveAt(i);
            }
        }

        for (i = 0; i < FireList.Count; i++) {

            fire_AI = FireList[i].GetComponent<FireAI>();
            fire_pos = FireList[i].transform.position;

            fire_AI.SpreadFlag = false;
            Debug.Log("Ready to No." + i);

            k = 0;

            if (WetObject.Count >= 1)
            {
                do
                {
                    TooBad = 0;

                    Debug.Log("next area serched at " + (k + 1) + " times...");

                    next = RandomNextVector(fire_pos);

                    for (j = 0; j < WetObject.Count; j++)
                    {
                        if (Vector3.Distance(next, WetObject[j].transform.position) <= limit_distance) {
                            TooBad++;
                        }
                    }

                    k++;
                    
                    if (k >= 50)
                    {
                        Debug.Log("next area can't generated!");
                    }

                } while (TooBad > 0 && k < 50);
            }
            else
            {
                next = RandomNextVector(fire_pos);
            }

            if (k < 50 && FireList.Count <= 10)
            {
                fire_AI.SpreadFlag = true;
                fire_AI.nextArea = next;
                Debug.Log("No." + i + " is able to Spread");
            }
            else {
                Debug.Log("No." + i + " can't Spread...");
                //fire_AI.SpreadFlag = false;
                fire_AI.nextArea = CurrentVector(fire_pos);
            }
        }

        //x++;                          //デバッグ用
        //if (x >= 5) {
        //    Debug.Log("Hello!");
        //    x = 0;
        //}
	}

    public Vector3 RandomNextVector(Vector3 obj) {
        float x = obj.x + Bias(Random.Range(-0.3f, 0.3f));
        float y = obj.y + Bias(Random.Range(0.05f, 0.2f));
        float z = obj.z + Bias(Random.Range(-0.3f, 0.3f));
        return new Vector3(x, y, z);
    }

    public Vector3 CurrentVector(Vector3 obj) {
        float x = obj.x;
        float y = obj.y;
        float z = obj.z;
        return new Vector3(x, y, z);
    }

    public float Bias(float param) {
        if (param < 0.1f) {
            param *= 2.0f;
        }

        return param;
    }
}
