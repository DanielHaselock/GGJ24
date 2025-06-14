using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource menuMusicSource;

    [SerializeField] private AudioSource levelMusicSource;

    [SerializeField] private AudioClip[] menuTracks;

    [SerializeField] private List<AudioClip> levelTracks;

    private int currentLevelTrackIndex = -1;

    private enum MusicType { None, Menu, Level, GameOver }
    private MusicType currentMusic = MusicType.None;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject); // Prevents the MusicManager from being destroyed when loading a new scene
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.Instance.CurrentGameState)
        {
            case GameManager.GameStates.MainMenu:
                if (currentMusic != MusicType.Menu)
                {
                    StopAllMusic();
                    PlayMenuMusic(0);
                    currentMusic = MusicType.Menu;
                }
                break;
            case GameManager.GameStates.CoinLevel:
            case GameManager.GameStates.RaceLevel:
            case GameManager.GameStates.SurviveLevel:
            case GameManager.GameStates.Scoreboard:
                StopMenuMusic();
                PlayLevelMusic();
                currentMusic = MusicType.Level;
                break;
            case GameManager.GameStates.Credits:
            case GameManager.GameStates.GameOver:
            case GameManager.GameStates.Lobby:
            case GameManager.GameStates.RoundSelect:
                if (currentMusic != MusicType.GameOver)
                {
                    StopAllMusic();
                    PlayMenuMusic(1); // Assuming track index 1 is for credits or game over
                    currentMusic = MusicType.GameOver;
                }
                break;

            default:
                if (currentMusic != MusicType.None)
                {
                    StopAllMusic();
                    currentMusic = MusicType.None;
                }
                break;
        }
    }


    private void StopAllMusic()
    {
        StopMenuMusic();
        StopLevelMusic();
    }

    public void PlayMenuMusic(int trackIndex)
    {
        if (menuMusicSource.isPlaying && menuMusicSource.clip == menuTracks[trackIndex]) return;

        menuMusicSource.clip = menuTracks[trackIndex];
        menuMusicSource.Play();
    }

    public void StopMenuMusic()
    {
        menuMusicSource.Stop();
    }

    public void PlayLevelMusic()
    {
       if(!levelMusicSource.isPlaying || levelMusicSource.clip == null)
        {
            currentLevelTrackIndex = (currentLevelTrackIndex + 1) % levelTracks.Count;
            levelMusicSource.clip = levelTracks[currentLevelTrackIndex];
            levelMusicSource.Play();
        }
    }

    public void StopLevelMusic()
    {
        levelMusicSource.Stop();
    }
}
