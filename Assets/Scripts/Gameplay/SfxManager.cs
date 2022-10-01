using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance { get; private set; }

    [SerializeField] private AudioSource prefab;


    [SerializeField] private AudioClip hitWallSfx;
    [SerializeField] private AudioClip hitIslandSfx;
    [SerializeField] private AudioClip failSfx;
    [SerializeField] private AudioClip buttonSfx;

    private List<AudioSource> poolingAudioSource = new List<AudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void OnDestroy()
    {
        Instance = null;
    }
    public void PlaySfx(SfxName name)
    {
        if (!AccountData.useSfx) { return; }
        AudioSource source = null;
        switch (name)
        {
            case SfxName.ButtonClick:
                source = GetAudioSource();
                source.PlayOneShot(buttonSfx);
                break;
            case SfxName.WallHit:
                source = GetAudioSource();
                source.PlayOneShot(hitWallSfx);
                break;
            case SfxName.IslandHit:
                source = GetAudioSource();
                source.PlayOneShot(hitIslandSfx);
                break;
            case SfxName.FailHit:
                source = GetAudioSource();
                source.PlayOneShot(failSfx);
                break;
            default: break;
        }
    }
    private AudioSource GetAudioSource()
    {
        AudioSource source = null;
        foreach (var s in poolingAudioSource)
        {
            if (!s.isPlaying)
            {
                source = s;
            }
        }
        if (source == null)
        {
            source = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            poolingAudioSource.Add(source);
        }
        return source;
    }

    public enum SfxName
    {
        WallHit,
        IslandHit,
        FailHit,
        ButtonClick
    }
}
