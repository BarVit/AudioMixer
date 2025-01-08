using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioSwitcher : MonoBehaviour
{
    [SerializeField] private UnityEngine.Audio.AudioMixer _audioMixer;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Button _button;
    [SerializeField] private Toggle _toggle;

    private Coroutine _smoothlyVolumeChanger;
    private float _minVolume = 0.0001f;
    private float _smoothChangeDuration = 1f;
    private float _lastMasterVolume;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
        _toggle.onValueChanged.AddListener(ToggleAudio);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
        _toggle.onValueChanged.RemoveListener(ToggleAudio);
    }

    private void OnButtonClick()
    {
        _toggle.isOn = !_toggle.isOn;
    }

    private void ToggleAudio(bool enabled)
    {
        if (enabled)
        {
            if (_smoothlyVolumeChanger != null)
                StopCoroutine(_smoothlyVolumeChanger);

            _masterSlider.interactable = true;
            _smoothlyVolumeChanger = StartCoroutine(ChangeVolumeSmoothly(_minVolume, _lastMasterVolume));
        }
        else
        {
            _lastMasterVolume = _masterSlider.value;

            if (_smoothlyVolumeChanger != null)
                StopCoroutine(_smoothlyVolumeChanger);

            _masterSlider.interactable = false;
            _smoothlyVolumeChanger = StartCoroutine(ChangeVolumeSmoothly(_lastMasterVolume, _minVolume));
        }
    }

    private IEnumerator ChangeVolumeSmoothly(float startVolume, float targetVolume)
    {
        float elapsedTime = 0f;
        float volumeDiaposone = Mathf.Abs(targetVolume - startVolume);
        float intermediateVolume;
        float deltaVolume;

        while (elapsedTime < _smoothChangeDuration)
        {
            elapsedTime += Time.deltaTime;
            deltaVolume = Time.deltaTime * volumeDiaposone / _smoothChangeDuration;

            if (targetVolume > _masterSlider.value)
                intermediateVolume = startVolume + elapsedTime * volumeDiaposone / _smoothChangeDuration;
            else
                intermediateVolume = startVolume - elapsedTime * volumeDiaposone / _smoothChangeDuration;

            _masterSlider.value = Mathf.MoveTowards(_masterSlider.value, intermediateVolume, deltaVolume);

            yield return null;
        }
    }
}