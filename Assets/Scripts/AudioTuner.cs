using UnityEngine;
using UnityEngine.UI;

public abstract class AudioTuner : MonoBehaviour
{
    [SerializeField] private UnityEngine.Audio.AudioMixer AudioMixer;
    [SerializeField] private Slider Slider;

    private float MinVolume = 0.0001f;
    private float StartVolume = 0.8f;
    protected string TunerName;

    protected virtual void Awake()
    {
        Slider.value = StartVolume;
    }

    private void OnEnable()
    {
        Slider.onValueChanged.AddListener(ChangeVolume);
    }

    private void OnDisable()
    {
        Slider.onValueChanged.RemoveListener(ChangeVolume);
    }

    private void ChangeVolume(float volume)
    {
        AudioMixer.SetFloat(TunerName, ConvertVolumeToDecibel(volume));
    }

    private float ConvertVolumeToDecibel(float volume)
    {
        int decibelMultiplier = 20;

        if (volume == 0)
            volume = MinVolume;

        return Mathf.Log10(volume) * decibelMultiplier;
    }
}