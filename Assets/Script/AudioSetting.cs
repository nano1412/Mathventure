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
    public Slider CardSfxSlider;
    public Slider ItemsSfxSlider;



    private void Awake()
    {
            Instance = this;
    }

    private void Start()
    {
        masterSlider.value = GetVolume("Master_Volume");
        musicSlider.value = GetVolume("BGM_Volume");
        ActionSfxSlider.value = GetVolume("ActionSFX_Volume");
        CardSfxSlider.value = GetVolume("CardSFX_Volume");
        ItemsSfxSlider.value = GetVolume("Items_Volume");

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        ActionSfxSlider.onValueChanged.AddListener(SetActionSFXVolume);
        CardSfxSlider.onValueChanged.AddListener(SetCardSFXVolume);
        ItemsSfxSlider.onValueChanged.AddListener(SetItemsSFXVolume);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master_Volume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("BGM_Volume", Mathf.Log10(volume) * 20);
    }

    public void SetActionSFXVolume(float volume)
    {
        audioMixer.SetFloat("ActionSFX_Volume", Mathf.Log10(volume) * 20);
    }

    public void SetCardSFXVolume(float volume)
    {
        audioMixer.SetFloat("CardSFX_Volume", Mathf.Log10(volume) * 20);
    }

    public void SetItemsSFXVolume(float volume)
    {
        audioMixer.SetFloat("Items_Volume", Mathf.Log10(volume) * 20);
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
