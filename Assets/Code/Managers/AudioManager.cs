using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource m_music, m_sfx, m_boing;
    [SerializeField] AudioClip m_mainMenu, m_loseScreen, m_coin, m_pop;
    [SerializeField] AudioClip[] m_songs, m_fills, m_cheers, m_laughs, m_gasps, m_insults;

    int m_nextBeatSwitch;
    [SerializeField] GameManager m_gameManager;
    int m_sfxCounter;

    public int NextBeatSwitch { set {  m_nextBeatSwitch = value; } }

    private float m_twoBarTimer, m_twoBeatTimer;
    private int m_boinging;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (!m_music.isPlaying && m_nextBeatSwitch != 0)
        {
            StartCoroutine(BeatSwitch(m_nextBeatSwitch - 1));
            m_nextBeatSwitch = 0;
        }

        m_twoBarTimer = m_music.time % 15;

        float previousTwoBeat = m_twoBeatTimer;
        m_twoBeatTimer = m_music.time % 3.75f;

        if (previousTwoBeat < 1.875f && m_twoBeatTimer >= 1.875f && m_nextBeatSwitch > 0)
        {
            StartCoroutine(BeatSwitch(m_nextBeatSwitch - 1));
            m_nextBeatSwitch = 0;
        }
    }

    private IEnumerator BeatSwitch(int beatIndex)
    {
        m_music.clip = m_fills[beatIndex];
        m_music.Play();
        PlayInsult();

        yield return new WaitForSeconds(1.875f);
        m_music.clip = m_songs[beatIndex];
        m_music.Play();

        m_gameManager.PlayNextLevel(); // added for audio continuity between scenes
    }

    public IEnumerator Boing()
    {
        if (m_boinging < 4)
        {
            m_boinging++;
            m_boing.pitch = Random.Range(0.9f, 1.1f);
            m_boing.Play();
            yield return new WaitForSeconds(3.0f);
            m_boinging--;
        }
    }

    public void Coin() { 
        m_sfx.PlayOneShot(m_coin);
    }
    
    public void Pop() { 
        m_sfx.PlayOneShot(m_pop);
    }

    public void PlayCheer()
    {
        int random = Random.Range(0, m_cheers.Length);
        StartCoroutine(ProcessSoundEffect(m_cheers[random]));
    }

    public void PlayInsult()
    {
        int random = Random.Range(0, m_insults.Length);
        m_sfx.PlayOneShot(m_insults[random]);
    }

    public void PlayLaughTrackOrGasp()
    {
        int coinFlip = Random.Range(0, 2);

        AudioClip[] audioArray;
        if (coinFlip == 0)
            audioArray = m_laughs;

        else
            audioArray = m_gasps;

        int random = Random.Range(0, audioArray.Length);
        StartCoroutine(ProcessSoundEffect(audioArray[random]));
    }

    public void PlaySong(string songName)
    {
        if (songName.Equals("menu"))
            m_music.clip = m_mainMenu;

        else if (songName.Equals("lose"))
            m_music.clip = m_loseScreen;

        else if (songName.Equals("dubsteb"))
            m_music.clip = m_songs[1];

        else if (songName.Equals("electro"))
            m_music.clip = m_songs[1];

        m_music.Play();
    }

    private IEnumerator ProcessSoundEffect(AudioClip sound)
    {
        if (m_sfxCounter >= 1)
            yield break;

        m_sfxCounter++;
        m_sfx.PlayOneShot(sound);
        yield return new WaitForSeconds(2.0f);

        m_sfxCounter--;
    }
}
