using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;

    [SerializeField] Button turnEndButton;

    [SerializeField] Text playerHeroHpText;
    [SerializeField] Text enemyHeroHpText;

    [SerializeField] Text playerManaCostText;
    [SerializeField] Text enemyManaCostText;

    [SerializeField] Text timeCountText;

    public void HideResultPanel()
    {
        resultPanel.SetActive(false);
    }

    public void CannotClickTheButton()
    {
        turnEndButton.interactable = false;
    }
    public void CanClickTheButton()
    {
        turnEndButton.interactable = true;
    }

    // マナコストを更新する
    public void ShowManaCost(int playerManaCost, int enemyManaCost)
    {
        playerManaCostText.text = playerManaCost.ToString();
        enemyManaCostText.text = enemyManaCost.ToString();
    }

    // カウントダウンを表示する
    public void UpdateTime(int timeCount)
    {
        timeCountText.text = timeCount.ToString();
    }

    // HeroのHPを更新する
    public void ShowHeroHp(int playerHeroHp, int enemyHeroHp)
    {
        playerHeroHpText.text = playerHeroHp.ToString();
        enemyHeroHpText.text = enemyHeroHp.ToString();
    }

    //対戦結果表示画面
    public void ShowResultPanel(int heroHp)
    {
        resultPanel.SetActive(true);
        if (heroHp <= 0)
        {
            resultText.text = "YOU LOSE";
        }
        else
        {
            resultText.text = "YOU WIN!";
        }

    }

}
