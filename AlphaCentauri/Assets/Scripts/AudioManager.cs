using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioManager;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    AudioSource audioSource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Destroy(gameObject); }
    }
    public enum Sound
    {
        BushShake,
        AmbienceWind,
        AmbienceCricket,
        WaterSplashFight,
        FishingLinePull,
        FootSteps,
        RodReel,
        WaterSplashBobber,
        WaterSplashPostFish,
        RodWoosh,
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySoundMain(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    public void PlaySound(AudioClip audioClip, AudioSource audioSource)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    public void StopSound()
    {
        audioSource.Stop();
    }
    public void PlaySFX(AudioClip audioClip)
    {
        Debug.Log(audioClip);
        Debug.Log("Playing SFX");
        audioSource.PlayOneShot(audioClip);
    }
}
public static class AudioRegistry
{
    public static Dictionary<Sound, AudioClip> Sounds = new Dictionary<Sound, AudioClip>(){
        {Sound.BushShake, Resources.Load<AudioClip>("Sounds/BushShake")},
        {Sound.AmbienceWind, Resources.Load<AudioClip>("Sounds/AmbienceWind")},
        {Sound.AmbienceCricket, Resources.Load<AudioClip>("Sounds/AmbienceCricket")},
        {Sound.WaterSplashFight, Resources.Load<AudioClip>("Sounds/WaterSplashFight")},
        {Sound.FishingLinePull, Resources.Load<AudioClip>("Sounds/FishingLinePull")},
        {Sound.FootSteps, Resources.Load<AudioClip>("Sounds/FootSteps")},
        {Sound.RodReel, Resources.Load<AudioClip>("Sounds/RodReel")},
        {Sound.WaterSplashBobber, Resources.Load<AudioClip>("Sounds/WaterSplashBobber")},
        {Sound.WaterSplashPostFish, Resources.Load<AudioClip>("Sounds/WaterSplashPostFish")},
        {Sound.RodWoosh, Resources.Load<AudioClip>("Sounds/RodWoosh")},
    };
}
