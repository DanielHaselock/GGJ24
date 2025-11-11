using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    // Start is called before the first frame update

    [SerializeField] private float Timer = -1f;

    [SerializeField]
    public float TimePlayingCurrentLevel = 15f;

    [SerializeField]
    private float TimePlayingScore = 5f;

    public bool hasPlayTime = true;

    [SerializeField]
    private GameObject Clock;

    private Slider clockSlider;

    public TimeState state;

    private Animator clockAnimator;
    private float initialTime = 0f;
    void Start()
    {
        Clock = GameObject.FindGameObjectWithTag("Clock");
        clockstate = ClockState.Start;
        state = TimeState.Start;

        clockAnimator = GetComponentInChildren<Animator>();
        if (!clockAnimator)
        {
            Debug.LogError("Clock Animator not found!");
            return;
        }
        clockSlider = Clock.GetComponentInChildren<Slider>();
        if (!clockSlider)
        {
            Debug.LogError("Clock Slider not found!");
            return;
        }

        TimerStarted();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasPlayTime)
        {
            return;
        }

        // Tickdown the timer if it is in the playing / scoring state
        if (state == TimeState.Playing || state == TimeState.Score)
        {
            Timer -= Time.deltaTime;
        }

        SetClockState();

        if (Timer <= 0.0f)
        {
            TimerEnded();
        }
    }

    public void TimerStarted()
    {
        if (state == TimeState.Start)
        {
            state = TimeState.Playing;
            if (GameManager.Instance.CurrentGameState == GameManager.GameStates.CoinLevel)
                initialTime = TimePlayingCurrentLevel - (1.25f * (PlayerManager.Instance.players.Count - 1));
            else if (GameManager.Instance.CurrentGameState == GameManager.GameStates.RaceLevel)
                initialTime = TimePlayingCurrentLevel + (5f * (PlayerManager.Instance.players.Count - 1));
            else
                initialTime = TimePlayingCurrentLevel;

            Timer = initialTime;
        }
        else if (state == TimeState.Playing) //should not happen
        { 
            Timer = TimePlayingCurrentLevel;
        }
    }

    public void TimerEnded()
    {
        FindFirstObjectByType<BaseLevelManager>().ToggleDeathZone();
        // set animation trigger
        if (clockAnimator)
        {
            clockAnimator.SetTrigger("times_up");
            clockAnimator.speed = 1f;
        }

        if (state == TimeState.Playing)
        {
            state = TimeState.Score;
            Timer = TimePlayingScore;

            PlayerManager.Instance.checkGameStateAndPlayers();
            
            // Change the silder fill to transparent
            if (clockSlider)
            {
                clockSlider.fillRect.GetComponent<Image>().color = new Color(0,0,0,0);
            }
        }
        else if (state == TimeState.Score)
        {
            state = TimeState.End;
            FindFirstObjectByType<BaseLevelManager>().OnRoundEnd();
        }
        else if (state == TimeState.End)
        {
            Timer = 0.0f;
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
        return Timer / initialTime * 100;
    }

    public void SetClockState()
    {
        float percentage = CalculatePercentage();

        clockAnimator.speed = 2f - (percentage / 100f);
        
        clockSlider.value = percentage/100f; // Assuming the slider value is between 0 and 1
    }
}
