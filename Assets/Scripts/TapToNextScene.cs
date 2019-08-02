using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using HoloToolkit.Unity.InputModule;

public class TapToNextScene : MonoBehaviour, IInputClickHandler {

    public enum scenes
    {
        Start,
        Main,
        End
    };

    [SerializeField] private scenes nextScene;

	// Use this for initialization
	void Start () {
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnInputClicked (InputClickedEventData eventData)
    {
        SceneManager.LoadScene((int)nextScene, LoadSceneMode.Additive);
        Destroy(gameObject);
    }
}
