using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public enum TimeState
    {
        Playing = 0,
        Score = 1,
    }


    private LevelManager levelManager;
    // Start is called before the first frame update

    [SerializeField] private float Timer = 4f;

    [SerializeField]
    private float TimePlayingCurrentLevel = 5f;

    [SerializeField]
    private float TimePlayingScore = 5f;


    TimeState state;
    void Start()
    {
        levelManager = gameObject.GetComponent<LevelManager>();
        state = TimeState.Playing;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0.0f)
        {
            TimerEnded();
        }
    }

    void TimerEnded()
    {
        if(state == TimeState.Playing)
        {
            state = TimeState.Score;
            Timer = TimePlayingScore;
            levelManager.LoadNextLevel();
        }
        else if (state == TimeState.Score)
        {
            state = TimeState.Playing;
            Timer = TimePlayingCurrentLevel; //todo Get Time for current LEvel
            levelManager.PlayNextLevel();
        }
        



    }


}
