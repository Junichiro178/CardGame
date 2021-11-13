using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    //　シングルトン
    public static SoundManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // 音声を再生するスピーカー
    public AudioSource audioSourceSE;
    public AudioSource audioSourceBGM;

    // 音源
    public AudioClip[] audioClipsBGM; // BGMの素材、番号で振り分け
    public AudioClip[] audioClipsSE; // SEの素材、番号で振り分け

    // BGMを鳴らす
    public void PlayBGM(string sceneName)
    {
        audioSourceBGM.Stop();
        switch (sceneName)
        {
            default:
            case "LoginScene":
                audioSourceBGM.clip = audioClipsBGM[0];
                break;
            case "Menu":
                audioSourceBGM.clip = audioClipsBGM[1];
                break;
            case "Stage1":
                audioSourceBGM.clip = audioClipsBGM[2];
                break;
            case "Stage2":
                audioSourceBGM.clip = audioClipsBGM[3];
                break;
            case "Stage3":
                audioSourceBGM.clip = audioClipsBGM[4];
                break;
        }
        audioSourceBGM.Play();
    }

    // 音を鳴らす
    public void PlaySE(int SoundNumber)
    {
        audioSourceSE.PlayOneShot(audioClipsSE[SoundNumber]);
    }
}
