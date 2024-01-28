using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    // Start is called before the first frame update

    private float Score;

    [SerializeField] private int MaxLevels;

    [SerializeField] private int[] LevelThreshholds = { 0, 500, 1000, 1500, 2000, 2500 };

    public int CurrentLevel = 0;

    CurrentLevelManager currentLevelManager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(float pNewScore, LevelManager levmanager)
    {
        Score = pNewScore;
        if (CurrentLevel < MaxLevels && pNewScore >= LevelThreshholds[CurrentLevel + 1])
        {
            CurrentLevel++;
            PlayerPrefs.SetInt("DifficultyLevel", CurrentLevel);
            levmanager.ChangeAllSceneTime();
        }

        if(pNewScore == 0)
            ResetScore();
    }

    public void ResetScore()
    {
        PlayerPrefs.DeleteKey("DifficultyLevel");
        Score = 0;
        CurrentLevel = 0;
    }
}
