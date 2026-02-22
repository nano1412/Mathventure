using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    public static AudioSetting Instance;

    public AudioMixer audioMixer;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider ActionSfxSlider;


    private void Awake()
    {
            Instance = this;
    }

    private void Start()
    {
        masterSlider.value = GetVolume("Master_Volume");
        musicSlider.value = GetVolume("BGM_Volume");
        ActionSfxSlider.value = GetVolume("ActionSFX_Volume");
    
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        ActionSfxSlider.onValueChanged.AddListener(SetActionSFXVolume);

        SetMasterVolume(PlayerPrefs.GetFloat("Master_Volume"));
        SetMusicVolume(PlayerPrefs.GetFloat("BGM_Volume"));
        SetActionSFXVolume(PlayerPrefs.GetFloat("ActionSFX_Volume"));
    }

    public void SetMasterVolume(float volume)
    {
        if (volume == null)
        {
            return;
        }

        PlayerPrefs.SetFloat("Master_Volume", volume);
        audioMixer.SetFloat("Master_Volume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        if (volume == null)
        {
            return;
        }
        
        PlayerPrefs.SetFloat("BGM_Volume", volume);
        audioMixer.SetFloat("BGM_Volume", Mathf.Log10(volume) * 20);
    }

    public void SetActionSFXVolume(float volume)
    {
        if (volume == null)
        {
            return;
        }
        
        PlayerPrefs.SetFloat("ActionSFX_Volume", volume);
        audioMixer.SetFloat("ActionSFX_Volume", Mathf.Log10(volume) * 20);
    }

    public float GetVolume(string volumeName)
    {
        float dB;
        if (audioMixer.GetFloat(volumeName, out dB))
        {
            float linear = Mathf.Pow(10f, dB / 20f);
            return Mathf.Clamp(linear, 0.0001f, 1f);
        }

        return 1f; // fallback
    }
}
