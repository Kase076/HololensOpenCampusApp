using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class PlayerController : MonoBehaviour, IInputClickHandler {

	// Use this for initialization
	void Start () {
        InputManager.Instance.AddGlobalListener(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log("OnInputClicked is called");

        // 画面の中央に対してRaycastを行う
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            // Raycastが当たった相手が炎であれば、
            if (hitInfo.collider.CompareTag("Fireball"))
            {
                // 撃ったことをCommandによってサーバーに通知する
                Debug.Log("This is fireball");
                CmdFireDestroyCall(hitInfo.collider.gameObject);
            }
        }
    }

    private void CmdFireDestroyCall(GameObject target)
    {
        target.GetComponent<FireAI>().TriggerThis();
    }
}
