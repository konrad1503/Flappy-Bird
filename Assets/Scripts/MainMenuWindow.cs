using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuWindow : MonoBehaviour
{
    private void Awake() {
        transform.Find("playBtn").GetComponent<Button>().onClick.AddListener(GameStart);

        transform.Find("exitBtn").GetComponent<Button>().onClick.AddListener(GameExit);
    }

    private static void GameStart() {
        Loading.Load(Loading.Scene.GameScene);
    }

    private static void GameExit() {
        Application.Quit();
    }
}
