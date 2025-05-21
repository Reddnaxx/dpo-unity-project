using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

using YG;

namespace _00_Scripts.UI
{
  public class SettingsUI : UIFadeScreen
  {
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;

    private void Start()
    {
      SetSliders();

      _audioMixer.SetFloat("Master", YG2.saves.masterVolume);
      _audioMixer.SetFloat("Music", YG2.saves.musicVolume);
      _audioMixer.SetFloat("Effects", YG2.saves.effectsVolume);
    }

    private void SetSliders()
    {
      masterSlider.value = YG2.saves.masterVolume;
      musicSlider.value = YG2.saves.musicVolume;
      effectsSlider.value = YG2.saves.effectsVolume;
    }

    public void OnMasterChange(float value)
    {
      YG2.saves.masterVolume = value;

      _audioMixer.SetFloat("Master", value);
      masterSlider.value = value;
    }

    public void OnMusicChange(float value)
    {
      YG2.saves.musicVolume = value;

      _audioMixer.SetFloat("Music", value);
      musicSlider.value = value;
    }

    public void OnEffectsChange(float value)
    {
      YG2.saves.effectsVolume = value;

      _audioMixer.SetFloat("Effects", value);
      effectsSlider.value = value;
    }
  }
}
