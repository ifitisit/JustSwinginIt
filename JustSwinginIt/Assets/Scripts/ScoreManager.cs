using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI text;
    public int score;
    public int totalScore;
    public TextMeshProUGUI timerText;
    private float startTime;
    public float goalTime;
    private bool Finished = false;
    private float t;

    // Start is called before the first frame update
    void Start()
    {
        ChangeScore(0);
        if (instance == null)
            instance = this;

        startTime = Time.time;
    }

    private void Update()
    {
        if (Finished)
            return;

        t = Time.time - startTime;

        string minutes = ((int)t / 60).ToString();
        string seconds = seconds = (t % 60).ToString("f2");

        timerText.text = minutes + ":" + seconds + " / 0:" +goalTime;
    }

    public void ChangeScore(int coinValue)
    {
        score += coinValue;
        text.text = "X" + score.ToString() + " / " + totalScore;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        score = 0;
        ChangeScore(0);
        startTime = Time.time;
        Finished = false;
    }

    public void Finish()
    {
        Finished = true;
    }

    public float GetTime()
    {
        return t;
    }
}
