using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using HoloToolkit.Unity.SpatialMapping;

public class WallDecision : MonoBehaviour {

    GameObject plane;
    int NearWallFrag;
	int CellingFrag;

	private SurfacePlane surfaceplane;

	public static int Failed;

	FireAI fire_ai;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (NearWallFrag == 1) {
			fire_ai = this.GetComponent<FireAI> ();
			fire_ai.NearWall = 1;
        }
		if (CellingFrag == 1) {
			fire_ai = this.GetComponent<FireAI> ();
			fire_ai.celling = 1;
		}
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Plane")
        {
            plane = other.gameObject;
			surfaceplane = plane.GetComponent<SurfacePlane>();
			if (surfaceplane.PlaneType == PlaneTypes.Wall)
			{
				NearWallFrag = 1;
				return;
			}
			else if (surfaceplane.PlaneType == PlaneTypes.Ceiling) {
				CellingFrag = 1;
				Failed = 1;
			}
			else
			{
				NearWallFrag = 0;
				return;
			}
        }
    }
}