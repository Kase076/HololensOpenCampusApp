using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMeta : MonoBehaviour {

    public static List<GameObject> FireList = new List<GameObject>();
    public static List<GameObject> WetObject = new List<GameObject>();

    private float obj_distance;
    private Vector3 wet_pos, fire_pos;
    private FireAI fire_AI;

    private Vector3 next;
    [SerializeField]
    private float limit_distance;

    int i, j, x = 0;

    // Use this for initialization
    void Start () {
        Debug.Log("FireMetaAI is ready.");
	}
	
	// Update is called once per frame
	void Update () {

        for (j = 0; j < WetObject.Count; j++)
        {
            if (WetObject[j] == null) {
                WetObject.RemoveAt(j);
            }
        }

        for (i = 0; i < FireList.Count; i++) {
            fire_AI = FireList[i].GetComponent<FireAI>();
            fire_pos = FireList[i].transform.position;

            if (fire_AI.DestroyedFlag) {
                Destroy(FireList[i]);
                FireList.RemoveAt(i);
            }

            Debug.Log("Ready to No." + i);

            int k = 0;

            if (WetObject.Count >= 1)
            {
                for (j = 0; j < WetObject.Count; j++)
                {
                    Debug.Log("WetObject No." + j);

                    wet_pos = WetObject[j].transform.position;
                    obj_distance = Vector3.Distance(fire_pos, wet_pos);

                    Debug.Log("Far From Wetarea?");
                    if (obj_distance <= limit_distance)
                    {
                        Debug.Log("No.");

                        do
                        {
                            next = RandomNextVector(fire_pos);
                            k++;
                        } while (Vector3.Distance(next, WetObject[j].transform.position) <= limit_distance || k <= 5);
                    }
                    else {
                        Debug.Log("Yes");
                        next = RandomNextVector(fire_pos);
                    }
                }
            }
            else
            {
                next = RandomNextVector(fire_pos);
            }

            if (k <= 5)
            {
                if (FireList.Count <= 30)
                {
                    fire_AI.SpreadFlag = true;
                }
                fire_AI.nextArea = next;
            }
            else {
                fire_AI.SpreadFlag = false;
                fire_AI.nextArea = CurrentVector(fire_pos);
            }
        }

        x++;
        if (x >= 5) {
            Debug.Log("Hello!");
            x = 0;
        }
	}

    public Vector3 RandomNextVector(Vector3 obj) {
        float x = obj.x + Random.Range(-0.3f, 0.3f);
        float y = obj.y + Random.Range(0.05f, 0.2f);
        float z = obj.z + Random.Range(-0.3f, 0.3f);
        return new Vector3(x, y, z);
    }

    public Vector3 CurrentVector(Vector3 obj) {
        float x = obj.x;
        float y = obj.y;
        float z = obj.z;
        return new Vector3(x, y, z);
    }
}
