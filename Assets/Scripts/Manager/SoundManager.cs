using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    Dictionary<string, AudioClip> bgmClipDict = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> sfxClipDict = new Dictionary<string, AudioClip>();

    [SerializeField] List<AudioClip> bgmClipList;
    [SerializeField] List<AudioClip> sfxClipList;

    [SerializeField] List<string> bgmNameList;
    [SerializeField] List<string> sfxNameList;

    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioSource sfxSource;

    void SetBgmDict()
    {
        for (int i = 0; i < bgmClipList.Count; i++)
        {
            bgmClipDict.Add(bgmNameList[i], bgmClipList[i]);
        }
    }

    void SetSfxDict()
    {
        for (int i = 0; i < sfxClipList.Count; i++)
        {
            sfxClipDict.Add(sfxNameList[i], sfxClipList[i]);
        }
    }

    public void PlayBgm(string bgmName)
    {
        bgmSource.clip = bgmClipDict[bgmName];
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySfx(string sfxName)
    {
        sfxSource.PlayOneShot(sfxClipDict[sfxName]);
    }
    // Start is called before the first frame update
    void Awake()
    {
        SetBgmDict();
        SetSfxDict();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
