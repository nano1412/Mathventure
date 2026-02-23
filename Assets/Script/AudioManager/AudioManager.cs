using UnityEngine.Audio;
using System;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using static Utils;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;
	private string currentBGMPlay;

	public Sound[] sounds;

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
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.mixerGroup;

        }
	}

    void OnEnable()
    {
        SceneManager.sceneLoaded += UpdateBGM;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= UpdateBGM;
    }

	void UpdateBGMOnGameStateChange(GameState gameState)
	{
		UpdateBGM(SceneManager.GetActiveScene(), LoadSceneMode.Single);

        switch (GameController.current.GameState)
        {
            case GameState.Shop:
                OverrideBGM("Shop");
                break;

            case GameState.Lose:
                OverrideBGM("Lose");
                break;

            case GameState.Win:
                OverrideBGM("Win");
                break;

            default:
                OverrideBGM("Level" + GameController.current.Level);
                break;
        }
    }

	void UpdateBGM(Scene scene, LoadSceneMode mode)
	{
        if (scene.name == SceneNav.current.MenuSceneName || scene.name == SceneNav.current.LeaderBoardSceneName)
        {
			OverrideBGM(SceneNav.current.MenuSceneName);
        }

		if(scene.name == SceneNav.current.GameSceneName)
		{
			GameController.current.OnGameStateChange += UpdateBGMOnGameStateChange;

            OverrideBGM("Level" + GameController.current.Level);

        } else
		{
            if (GameController.current != null)
            {
                GameController.current.OnGameStateChange -= UpdateBGMOnGameStateChange;
            }
        }
    }

    public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

    public void OverrideBGM(string bgmName)
    {
        if(currentBGMPlay == bgmName)
        {
            return;
        }
        foreach (Sound s in sounds)
        {
            if (s.mixerGroup.name == "BGM" && s.source.isPlaying)
            {
                s.source.Stop();
            }
        }
        currentBGMPlay = bgmName;

        Play(bgmName);
    }

}
