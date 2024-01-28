using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource m_music, m_sfx;
    [SerializeField] AudioClip[] m_songs;
    [SerializeField] AudioClip[] m_fills;
    [SerializeField] int nextBeatSwitch;

    private float m_twoBarTimer, m_twoBeatTimer;

    private void Update()
    {
        m_twoBarTimer = m_music.time % 15;

        float previousTwoBeat = m_twoBeatTimer;
        m_twoBeatTimer = m_music.time % 3.75f;

        if (previousTwoBeat < 1.875f && m_twoBeatTimer >= 1.875f && nextBeatSwitch > 0)
        {
            StartCoroutine(BeatSwitch(nextBeatSwitch - 1));
        }
    }

    private IEnumerator BeatSwitch(int beatIndex)
    {
        nextBeatSwitch = 0;
        m_music.clip = m_fills[beatIndex];
        m_music.Play();

        yield return new WaitForSeconds(1.875f);
        m_music.clip = m_songs[beatIndex];
        m_music.Play();
    }
}
