using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverWindow : MonoBehaviour
{
    private Text scoreText;

    private void Awake() {
        scoreText = transform.Find("scoreText").GetComponent<Text>();

        transform.Find("retryBtn").GetComponent<Button>().onClick.AddListener(TaskOnClick);
        Hide();
    }

    private static void TaskOnClick() {
        Loading.Load(Loading.Scene.GameScene);
    }

    private void Start() {
        Bird.GetInstance().OnDied += Bird_OnDied;
    }

    private void Bird_OnDied(object sender, System.EventArgs e) {
        scoreText.text = Level.GetInstance().GetPipesPassedCount().ToString();
        Show();
    }

    private void Hide() {
        // gameObject.SetActive(false);
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }

    private void Show() {
        // gameObject.SetActive(true);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
}
