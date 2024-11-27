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
        WaterSplashFight,
        FishingLinePull,
        FootSteps,
        RodReel,
        WaterSplashBobber,
        WaterSplashPostFish,
        RodCast,
        RodUnequip,
        RodEquip,
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
        {Sound.BushShake, Resources.Load<AudioClip>("Sounds/bush")},
        {Sound.WaterSplashFight, Resources.Load<AudioClip>("Sounds/fish battle")},
        {Sound.FishingLinePull, Resources.Load<AudioClip>("Sounds/reeling")},
        {Sound.FootSteps, Resources.Load<AudioClip>("Sounds/steps")},
        {Sound.WaterSplashBobber, Resources.Load<AudioClip>("Sounds/bobber landing")},
        {Sound.WaterSplashPostFish, Resources.Load<AudioClip>("Sounds/fish out of water")},
        {Sound.RodCast, Resources.Load<AudioClip>("Sounds/rod cast")},
        {Sound.RodUnequip, Resources.Load<AudioClip>("Sounds/rod unequip")},
        {Sound.RodEquip, Resources.Load<AudioClip>("Sounds/rod unequip")},
    };
}
