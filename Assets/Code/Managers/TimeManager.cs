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
        Start = 4
    }

    public enum ClockState
    {
        Calm,
        Medium,
        Red,
        Start
    }

    ClockState clockstate;


    private LevelManager levelManager;
    private GameManager gameManager;
    // Start is called before the first frame update

    [SerializeField] private float Timer = 4f;

    [SerializeField]
    public float TimePlayingCurrentLevel = 5f;

    [SerializeField]
    private float TimePlayingScore = 2f;

    public bool pPlayTime = true;

    [SerializeField]
    private GameObject Clock;

    public TimeState state;
    void Start()
    {
        levelManager = gameObject.GetComponent<LevelManager>();
        gameManager = gameObject.GetComponent<GameManager>();
        Clock = GameObject.FindGameObjectWithTag("Clock");
        clockstate = ClockState.Start;
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
        SetClockState();

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
        else if (state == TimeState.Start)
        {
            state = TimeState.Playing;
            gameManager.PlayNextLevel();
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
            case TimeState.Start:
                Timer = TimePlayingScore; //Don't check for win here
                break;
        }
    }

    public void ResetTime()
    {
        Timer = TimePlayingCurrentLevel;
    }

    public float CalculatePercentage()
    {
        return (Timer / TimePlayingCurrentLevel) * 100;
    }

    public void SetClockState()
    {
        float percentage = CalculatePercentage();

        if(!Clock){
            Clock = GameObject.FindGameObjectWithTag("Clock");

            if (percentage > 60 && clockstate != ClockState.Calm)
            {
                clockstate = ClockState.Calm;
                Clock.GetComponent<Animator>().speed = 1;
                //Clock.GetComponent<Animator>().SetBool("Calm", true);
                //Clock.GetComponent<Animator>().SetBool("Medium", false);
                //Clock.GetComponent<Animator>().SetBool("Red", false);
            }
            else if (percentage > 30 && percentage < 60 && clockstate != ClockState.Medium)
            {
                clockstate = ClockState.Medium;
                Clock.GetComponent<Animator>().speed = 1.5f;
                //Clock.GetComponent<Animator>().SetBool("Calm", false);
                //Clock.GetComponent<Animator>().SetBool("Medium", true);
                //Clock.GetComponent<Animator>().SetBool("Red", false);
            }
            else if (percentage < 30 && clockstate != ClockState.Red)
            {
                clockstate = ClockState.Red;
                Clock.GetComponent<Animator>().speed = 3f;
                //Clock.GetComponent<Animator>().SetBool("Calm", false);
                //Clock.GetComponent<Animator>().SetBool("Medium", false);
                //Clock.GetComponent<Animator>().SetBool("Red", true);
            }
        }
    }
}
