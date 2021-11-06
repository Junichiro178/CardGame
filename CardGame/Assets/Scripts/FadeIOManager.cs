using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// フェードインとフェードアウトを管理する
public class FadeIOManager : MonoBehaviour
{
    // シングルトン化（シーンの切り替えの際にオブジェクトが破壊されないように）
    public static FadeIOManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // CanvasGroupを取得する
    public CanvasGroup canvasGroup;

    // フェードアウト
    public void FadeOut()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1, 0.75f)
            .OnComplete(() => canvasGroup.blocksRaycasts = false);
    }

    // フェードイン
    public void FadeIn()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(0, 0.75f)
            .OnComplete(() => canvasGroup.blocksRaycasts = false);
    }

    // フェードアウトしてからフェードインする
    public void FadeOutToIn(TweenCallback action)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1, 0.75f)
            .OnComplete(() =>
            {
                action();
                FadeIn();
            });
    }

}
