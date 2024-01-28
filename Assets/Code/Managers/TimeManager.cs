using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public enum TimeState
    {
        Playing = 1,
        Score = 2,
        End = 3,
    }


    private LevelManager levelManager;
    private GameManager gameManager;
    // Start is called before the first frame update

    [SerializeField] private float Timer = 4f;

    [SerializeField]
    public float TimePlayingCurrentLevel = 5f;

    [SerializeField]
    private float TimePlayingScore = 2f;

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
        {
            Timer = TimePlayingCurrentLevel;
            return;
        }

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
            //gameManager.PlayNextLevel(); commendted out because it breaks audio
            Timer = TimePlayingCurrentLevel;
        }

        else if (state == TimeState.End)
        {
            gameManager.ShowEndUI();
            Timer = TimePlayingCurrentLevel;
        }
    }

    public void SwitchTimeExternal(TimeState pstate)
    {
        state = pstate;

        switch (pstate)
        {
            case TimeState.Playing:
                Timer = TimePlayingCurrentLevel;
                break;

            case TimeState.Score:
                Timer = TimePlayingScore; //Don't check for win here
                break;
            case TimeState.End:
                Timer = TimePlayingScore; //Don't check for win here
                break;
        }
    }

    public void ResetTime()
    {
        Timer = TimePlayingCurrentLevel;
    }

}
