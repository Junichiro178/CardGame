using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerManager : MonoBehaviour
{
    // デッキ
    public List<int> deck = new List<int>();

    // プレイヤーのHP
    public int heroHp;

    // マナコスト
    public int manaCost;
    public int defaultManaCost;

    public void Init(List<int> cardDeck)
    {
        this.deck = cardDeck;
        heroHp = 10;
        manaCost = 5;
        defaultManaCost = 5;
    }

    // 毎ターンマナコストを増やす
    public void IncreaseManaCost()
    {
        defaultManaCost++;
        manaCost = defaultManaCost;
    }
}
