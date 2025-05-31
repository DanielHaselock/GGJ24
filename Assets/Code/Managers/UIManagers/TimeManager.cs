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

    [SerializeField] private float Timer = 4f;

    [SerializeField]
    public float TimePlayingCurrentLevel = 5f;

    [SerializeField]
    private float TimePlayingScore = 2f;

    public bool pPlayTime = true;

    [SerializeField]
    private GameObject Clock;

    private Slider clockSlider;

    public TimeState state;

    private Animator clockAnimator;
    void Start()
    {
        Clock = GameObject.FindGameObjectWithTag("Clock");
        clockstate = ClockState.Start;

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
        }
        else if (state == TimeState.Score)
        {
            state = TimeState.Playing;
            //gameManager.PlayNextLevel(); commendted out because it breaks audio
            Timer = TimePlayingCurrentLevel;
        }

        else if (state == TimeState.End)
        {
            Timer = TimePlayingCurrentLevel;
        }
        else if (state == TimeState.Start)
        {
            state = TimeState.Playing;
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

        clockAnimator.speed = 2f - (percentage / 100f);
        
        clockSlider.value = percentage/100f; // Assuming the slider value is between 0 and 1
    }
}
