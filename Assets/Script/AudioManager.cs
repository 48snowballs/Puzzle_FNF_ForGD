using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource bgMusic;
    public AudioSource songMusic;

    private void Start()
    {
        
    }
    public void PlaySong(AudioClip clip, float delay = 0)
    {
        songMusic.clip = clip;

        if (delay == 0)
            songMusic.Play();
        else
        {
            //songMusic.PlayDelayed(delay);
            StartCoroutine(PlaySongDelay(delay));
        }
        bgMusic.Stop();
    }
    public void PlayGame(AudioClip clipSong, float delay)
    {
        songMusic.clip = clipSong;
        StartCoroutine(PlayGameDelay(delay));
        bgMusic.Stop();
    }
    IEnumerator PlayGameDelay(float delay)
    {
        while (!songMusic.clip.LoadAudioData())
            yield return Yielders.EndOffFrame;

        yield return new WaitForSeconds(delay);
        songMusic.Play();
    }
    public void PauseGame()
    {
        songMusic.Pause();
    }

    public void ResumeGame()
    {
        songMusic.Play();
    }

    public void StopGame()
    {
        songMusic.Stop();
    }

    IEnumerator PlaySongDelay(float delay)
    {
        while (!songMusic.clip.LoadAudioData())
            yield return null;

        yield return new WaitForSeconds(delay);
        songMusic.Play();
    }
    public void PlayBackgroundMusic()
    {
        if (!bgMusic.isPlaying)
            bgMusic.Play();
    }

    public void StopBackgroundMusic()
    {
        bgMusic.Stop();
    }
}
