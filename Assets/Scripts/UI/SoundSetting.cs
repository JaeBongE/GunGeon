using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    public enum enumSound
    {
        MasterVolume, BGMVolume, SFXVolume
    }

    [SerializeField] List<Slider> optionSlide;//0 ¸¶½ºÅÍ, 1 BGM, 2 SFX
    [SerializeField] AudioMixer audioMixer;
    private float masterValue;
    private float bgmValue;
    private float sfxValue;

    private void Awake()
    {
        initSlider();
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("setMaster"))
        {
            //masterValue = PlayerPrefs.GetFloat("setMaster");
            //audioMixer.SetFloat(enumSound.MasterVolume.ToString(), Mathf.Log10(masterValue) * 20);
            //optionSlide[0].value = masterValue;

            optionSlide[0].value = PlayerPrefs.GetFloat("setMaster");
        }
        if (PlayerPrefs.HasKey("setBGM"))
        {
            //bgmValue = PlayerPrefs.GetFloat("setBGM");
            //audioMixer.SetFloat(enumSound.BGMVolume.ToString(), Mathf.Log10(bgmValue) * 20);
            //optionSlide[1].value = bgmValue;

            optionSlide[1].value = PlayerPrefs.GetFloat("setBGM");
        }
        if (PlayerPrefs.HasKey("setSFX"))
        {
            //sfxValue = PlayerPrefs.GetFloat("setSFX");
            //audioMixer.SetFloat(enumSound.SFXVolume.ToString(), Mathf.Log10(sfxValue) * 20);
            //optionSlide[2].value = sfxValue;

            optionSlide[2].value = PlayerPrefs.GetFloat("setSFX");
        }
    }

    private void initSlider()
    {
        optionSlide[0].onValueChanged.AddListener(volume => setVolume(enumSound.MasterVolume, volume));
        optionSlide[1].onValueChanged.AddListener(volume => setVolume(enumSound.BGMVolume, volume));
        optionSlide[2].onValueChanged.AddListener(volume => setVolume(enumSound.SFXVolume, volume));
    }

    private void setVolume(enumSound _value, float _volume)
    {
        audioMixer.SetFloat(_value.ToString(), Mathf.Log10(_volume) * 20);

        if (_value == enumSound.MasterVolume)
        {
            PlayerPrefs.SetFloat("setMaster", _volume);
        }
        if (_value == enumSound.BGMVolume)
        {
            PlayerPrefs.SetFloat("setBGM", _volume);
        }

        if (_value == enumSound.SFXVolume)
        {
            PlayerPrefs.SetFloat("setSFX", _volume);
        }
    }
}

