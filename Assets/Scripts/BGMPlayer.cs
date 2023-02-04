using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMPlayer : MonoBehaviour
{
    public AudioClip lobbyMusic;
    public AudioClip[] matchMusic;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }
    void Start()
    {
        PlayASong();
    }

    void PlayASong()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            AudioManager.instance.PlayMusic(lobbyMusic);
        }
        else
        {
            AudioManager.instance.PlayMusic(matchMusic[UnityEngine.Random.Range(0, matchMusic.Length)]);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayASong();
    }
}
