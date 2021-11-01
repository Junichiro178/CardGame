using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ステージセレクトのロジックを管理
public class StageSelectManager : MonoBehaviour
{
    public void PlayStage1()
    {
        SceneManager.LoadScene("Game");
    }
}
