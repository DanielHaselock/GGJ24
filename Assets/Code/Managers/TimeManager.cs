using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public enum TimeState
    {
        Playing = 1,
        Score = 2,
    }


    private LevelManager levelManager;
    private GameManager gameManager;
    // Start is called before the first frame update

    [SerializeField] private float Timer = 4f;

    [SerializeField]
    public float TimePlayingCurrentLevel = 5f;

    [SerializeField]
    private float TimePlayingScore = 5f;

    public bool pPlayTime = true;

    public TimeState state;
    void Start()
    {
        levelManager = gameObject.GetComponent<LevelManager>();
        gameManager = gameObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pPlayTime)
            return;

        Timer -= Time.deltaTime;

        if (Timer <= 0.0f)
        {
            TimerEnded();
        }
    }

    public void TimerEnded()
    {
        if(state == TimeState.Playing)
        {
            state = TimeState.Score;
            Timer = TimePlayingScore;
            levelManager.CheckLevelWin();
        }
        else if (state == TimeState.Score)
        {
            state = TimeState.Playing;
            gameManager.PlayNextLevel();
            Timer = TimePlayingCurrentLevel;
        }
    }
}
