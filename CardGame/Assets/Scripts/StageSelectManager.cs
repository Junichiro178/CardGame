using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ステージセレクトのロジックを管理
public class StageSelectManager : MonoBehaviour
{
    public void PlayStage1()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void PlayStage2()
    {
        SceneManager.LoadScene("Stage2");
    }

    public void PlayStage3()
    {
        SceneManager.LoadScene("Stage3");
    }
}
