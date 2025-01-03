using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AudioMixer : MonoBehaviour
{
    [SerializeField] private UnityEngine.Audio.AudioMixer _audioMixer;
    [SerializeField] private Slider _masterSlider;

    private float _minVolume = 0.0001f;
    private float _startVolume = 0.4f;
    private float _smoothChangeDuration = 1f;
    private float _lastMasterVolume;
    private string _masterVolumeName = "Master Volume";
    private string _musicVolumeName = "Music Volume";
    private string _effectsVolumeName = "Effects Volume";

    private void Start()
    {
        _audioMixer.SetFloat(_masterVolumeName, ConvertVolumeToDecibel(_startVolume));
        _audioMixer.SetFloat(_musicVolumeName, ConvertVolumeToDecibel(_startVolume));
        _audioMixer.SetFloat(_effectsVolumeName, ConvertVolumeToDecibel(_startVolume));
    }

    public void ToggleAudio(bool enabled)
    {
        if (enabled)
        {
            StartCoroutine(ChangeVolumeSmoothly(_masterSlider, _minVolume, _lastMasterVolume));
        }
        else
        {
            _lastMasterVolume = _masterSlider.value;
            StartCoroutine(ChangeVolumeSmoothly(_masterSlider, _lastMasterVolume, _minVolume));
        }
    }

    public void ChangeMasterVolume(float volume)
    {
        _audioMixer.SetFloat(_masterVolumeName, ConvertVolumeToDecibel(volume));
    }

    public void ChangeMusicVolume(float volume)
    {
        _audioMixer.SetFloat(_musicVolumeName, ConvertVolumeToDecibel(volume));
    }

    public void ChangeEffectsVolume(float volume)
    {
        _audioMixer.SetFloat(_effectsVolumeName, ConvertVolumeToDecibel(volume));
    }

    private float ConvertVolumeToDecibel(float volume)
    {
        int decibelMultiplier = 20;

        if (volume == 0)
            volume = _minVolume;

        return Mathf.Log10(volume) * decibelMultiplier;
    }

    private IEnumerator ChangeVolumeSmoothly(Slider slider, float startVolume, float targetVolume)
    {
        float elapsedTime = 0f;
        float volumeDiaposone = Mathf.Abs(targetVolume - startVolume);
        float intermediateVolume;
        float deltaVolume;

        while (elapsedTime < _smoothChangeDuration)
        {
            elapsedTime += Time.deltaTime;
            deltaVolume = Time.deltaTime * volumeDiaposone / _smoothChangeDuration;

            if (targetVolume > slider.value)
                intermediateVolume = elapsedTime * volumeDiaposone / _smoothChangeDuration;
            else
                intermediateVolume = startVolume - elapsedTime * volumeDiaposone / _smoothChangeDuration;

            slider.value = Mathf.MoveTowards(_masterSlider.value, intermediateVolume, deltaVolume);

            yield return null;
        }
    }
}