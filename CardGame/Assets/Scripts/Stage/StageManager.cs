using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    // メニューボタンの音
    public void OnMenuButton()
    {
        SoundManager.instance.PlaySE(5);
    }

    // ターンエンドボタンの音ん
    public void OnTurnEndButton()
    {
        SoundManager.instance.PlaySE(7);
    }

    // サレンダーボタンの音
    public void OnSurrenderButton()
    {
        SoundManager.instance.PlaySE(4);
    }

    // リターンボタンの音
    public void OnReturnButton()
    {
        SoundManager.instance.PlaySE(1);
    }

    // リスタートボタンの音
    public void OnRestartButton()
    {
        SoundManager.instance.PlaySE(7);
    }
}
