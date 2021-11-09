using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// デッキ編成を司る
public class DeckManger : MonoBehaviour
{
    GameManager gameManager;

    public GamePlayerManager player;
    public GamePlayerManager enemy;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    //　デッキの設定
    public void InitDeck()
    {
        if (gameManager.stageNumber == 1)
        {
            Debug.Log("ここはステージ１です");
            player.Init(new List<int> { 6, 2, 3, 4, 5 });
            enemy.Init(new List<int> { 7, 9, 2, 2, 1 });
        }
        else if(gameManager.stageNumber == 2)
        {
            Debug.Log("ここはステージ２です");
            player.Init(new List<int> { 1, 2, 3, 4, 5 });
            enemy.Init(new List<int> { 11, 10, 9, 8, 7 });
        }
        else if (gameManager.stageNumber == 3)
        {
            Debug.Log("ここはステージ３です");
            player.Init(new List<int> { 3, 3, 2, 2, 5, 6 });
            enemy.Init(new List<int> { 1, 1, 2, 2, 3, 3 });
        }
        else
        {
            player.Init(new List<int> { 6, 2, 3, 4, 5 });
            enemy.Init(new List<int> { 7, 9, 2, 2, 1 });
        }

    }
}
