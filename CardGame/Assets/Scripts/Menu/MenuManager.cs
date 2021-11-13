using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    //　ゲーム終了
    public void QuitGame()
    {
        Debug.Log("アプリ終了");
        Application.Quit();
    }

    // バトル降参
    public void SurrenderBattle()
    {
        Debug.Log("降参");
        SceneManager.LoadScene("Menu");
    }

    // 各種SE
    public void OnStageSelectButton()
    {
        SoundManager.instance.PlaySE(1);
    }

    public void OnQuitButton()
    {
        SoundManager.instance.PlaySE(1);
    }

    public void OnBackButton()
    {
        SoundManager.instance.PlaySE(2);
    }

    public void OnChoseStageButton()
    {
        SoundManager.instance.PlaySE(3);
    }
}
