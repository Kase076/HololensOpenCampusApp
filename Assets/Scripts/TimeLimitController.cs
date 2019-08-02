using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeLimitController : MonoBehaviour {

    private static float _time = 0;
    public static float TimeLimit { get { return _time; } }

    private GameObject gameController;
    private GameController gmctrl;

    [SerializeField]
    private float limit_time;

    void Start()
    {
        _time = limit_time;
        gameController = GameObject.Find("GameController");
        gmctrl = gameController.GetComponent<GameController>();
    }

    void Update()
    {
        // 毎フレーム毎に残り時間を減らしていく
        _time -= Time.deltaTime;
        if (_time < 0)
        {
            gmctrl.EndFlag = true;
            SceneManager.LoadScene("End", LoadSceneMode.Additive);
            Destroy(this);
        }
        
    }
}
