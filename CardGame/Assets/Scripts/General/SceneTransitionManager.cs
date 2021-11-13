using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{

    // シーンを切り替えるする
    // コールバック関数で、FadeIOManagerにつなげる
    public void CallBackLoadAnothrScene(string sceneName)
    {
        FadeIOManager.instance.FadeOutToIn(() => LoadAnotherScene(sceneName));
    }

    // シーンの切り替えはここで行う
    public void LoadAnotherScene(string sceneName)
    {
        SoundManager.instance.PlayBGM(sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
