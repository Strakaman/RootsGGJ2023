using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour
{
	//modified from Sebastian Lague
	//https://github.com/SebLague/Create-a-Game-Source/blob/master/Episode%2025/SoundLibrary.cs
	public enum AudioChannel { Master, Sfx, Music, Voice };

	public float masterVolumePercent;
	public float sfxVolumePercent;
	public float musicVolumePercent;
	public float voiceVolumePercent;

	AudioSource sfx2DSource;
	AudioSource[] musicSources;
	int activeMusicSourceIndex;

	public static AudioManager instance;

	Transform audioListener;
	Transform playerT;

	SoundLibrary library;
	Dictionary<string, float> voiceLineCoolDown; 

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);

			library = GetComponent<SoundLibrary>();
			voiceLineCoolDown = new Dictionary<string, float>();
			musicSources = new AudioSource[2];
			for (int i = 0; i < 2; i++)
			{
				GameObject newMusicSource = new GameObject("Music source " + (i + 1));
				musicSources[i] = newMusicSource.AddComponent<AudioSource>();
				newMusicSource.transform.parent = transform;
			}
			GameObject newSfx2Dsource = new GameObject("2D sfx source");
			sfx2DSource = newSfx2Dsource.AddComponent<AudioSource>();
			newSfx2Dsource.transform.parent = transform;

			audioListener = FindObjectOfType<AudioListener>().transform;
		}
	}

	void Update()
	{	
		if (playerT != null)
		{
			//audioListener.position = playerT.position; //don't need this as audio listener is already on the player
		}
		//Dictionary<string, float>.KeyCollection keys = voiceLineCoolDown.Keys;
		List<string> listOfKeys = voiceLineCoolDown.Keys.ToList();
		foreach (string key in listOfKeys)
        {
			voiceLineCoolDown[key] -= Time.deltaTime;
        }
	}

	public void SetVolume(float volumePercent, AudioChannel channel)
	{
		switch (channel)
		{
			case AudioChannel.Master:
				masterVolumePercent = volumePercent;
				break;
			case AudioChannel.Sfx:
				sfxVolumePercent = volumePercent;
				break;
			case AudioChannel.Music:
				musicVolumePercent = volumePercent;
				break;
			case AudioChannel.Voice:
				voiceVolumePercent = volumePercent;
				break;
		}

		musicSources[0].volume = musicVolumePercent * masterVolumePercent;
		musicSources[1].volume = musicVolumePercent * masterVolumePercent;

	}

	public void PlayMusic(AudioClip clip, float fadeDuration = 2)
	{
		activeMusicSourceIndex = 1 - activeMusicSourceIndex;
		musicSources[activeMusicSourceIndex].clip = clip;
		musicSources[activeMusicSourceIndex].loop = true;
		musicSources[activeMusicSourceIndex].Play();

		StartCoroutine(AnimateMusicCrossfade(fadeDuration));
	}

	public void StopMusic()
    {
		foreach(AudioSource audioSource in musicSources)
        {
			audioSource.Stop();
        }
    }

	public void PlaySoundFX(AudioClip clip, Vector3 pos)
	{
		if (clip != null)
		{
			AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * masterVolumePercent);
		}
	}

	public void PlaySoundFX(string soundName, Vector3 pos)
	{
		PlaySoundFX(library.GetClipFromName(soundName), pos);
	}

	public void PlayVoiceLine(string soundName)
	{
		sfx2DSource.PlayOneShot(library.GetClipFromName(soundName), voiceVolumePercent * masterVolumePercent);
	}

	public void PlayVoiceLine(string soundName, float repeatCooldown)
	{
		//if cooldown has not been hit for sound group, then do not play sound again
		if (voiceLineCoolDown.ContainsKey(soundName) && voiceLineCoolDown[soundName]> 0) { return; }

		voiceLineCoolDown[soundName] = repeatCooldown;
		PlayVoiceLine(soundName);
	}


	IEnumerator AnimateMusicCrossfade(float duration)
	{
		float percent = 0;

		while (percent < 1)
		{
			percent += Time.deltaTime * 1 / duration;
			musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
			musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
			yield return null;
		}
	}
}