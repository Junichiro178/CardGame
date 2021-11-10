using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject canvas;

    public GamePlayerManager player;
    public GamePlayerManager enemy;
    public DeckManger deckManager;

    [SerializeField] AI enemyAI;
    [SerializeField] UIManager uiManager;

    public Transform playerHandTransform,
                     playerFieldTransform,
                     enemyHandTransform,
                     enemyFieldTransform;

    [SerializeField] CardController cardPrefab;
    public Transform playerHero;
    public Transform enemyHero;

    public GameObject enemyTurnDisplay;
    public GameObject playerTurnDisplay;

    // ステージの番号
    public int stageNumber;

    //　プレイヤーのターンかどうか
    public bool isPlayerTurn;

    // 時間管理
    int timeCount;

    //シングルトン化（どこからでもアクセスできる）
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        StartGame();
    }

    //ゲーム開始
    void StartGame()
    {
        // デッキを生成
        deckManager.InitDeck();

        // 時間の設定
        timeCount = 20;

        uiManager.HideResultPanel();
        uiManager.ShowHeroHp(player.heroHp, enemy.heroHp);
        uiManager.ShowManaCost(player.manaCost, enemy.manaCost);
        uiManager.UpdateTime(timeCount);
        InitHand();
        isPlayerTurn = true;
        TurnCalculation();
    }

    // 再度ゲームを行う
    public void Restart()
    {
        // HandとFieldのカードを削除
        foreach (Transform card in playerHandTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in enemyHandTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in playerFieldTransform)
        {
            Destroy(card.gameObject);
        }
        foreach (Transform card in enemyFieldTransform)
        {
            Destroy(card.gameObject);
        }

        // デッキを生成
        player.deck = new List<int>() { 10, 2, 3, 4, 5, 1, 2, 3, 4, 5 };
        enemy.deck = new List<int>() { 5, 3, 4, 4, 5, 1, 2, 3, 5, 4 };

        StartGame();
    }

    //手札を配る
    void InitHand()
    {
        for (int i = 0; i < 3; i++)
        {
            GiveCardToHand(player.deck, playerHandTransform);
            GiveCardToHand(enemy.deck, enemyHandTransform);
        }
    }

    //手札にカードをデッキから配る
    void GiveCardToHand(List<int> deck, Transform hand)
    {
        // デッキが0枚なら返す
        if (deck.Count == 0)
        {
            return;
        }
        else
        {
            int cardID = deck[0];
            deck.RemoveAt(0);
            CreateCard(cardID, hand);
        }
    }

    //カードを生成する
    void CreateCard(int cardID, Transform hand)
    {
        CardController card = Instantiate(cardPrefab, hand, false);
        if (hand.name == "PlayerHand")
        {
            card.Init(cardID, true);
        }
        else
        {
            card.Init(cardID, false);
        }
    }

    //ターンを切り替える
    void TurnCalculation()
    {
        StopAllCoroutines();
        StartCoroutine(CountDown());
        if (isPlayerTurn)
        {
            StartCoroutine(PlayerTurn());
            StartCoroutine(DisplayTurnController(playerTurnDisplay));
        }
        else
        {
            StartCoroutine(enemyAI.EnemyTurn());
            StartCoroutine(DisplayTurnController(enemyTurnDisplay));
        }
    }

    // ターンの残り秒数を数える
    IEnumerator CountDown()
    {
        // 秒数リセット
        timeCount = 20;
        uiManager.UpdateTime(timeCount);

        // カウントダウン0以上の時は繰り返し処理
        while (timeCount > 0)
        {
            yield return new WaitForSeconds(1);
            timeCount--;
            uiManager.UpdateTime(timeCount);
        }

        ChangeTurn();
    }

    // 相手プレイヤーのフィールドのカードを全て取得する
    public CardController[] GetEnemyFieldCards(bool isPlayer)
    {
        if (isPlayer)
        {
            return enemyFieldTransform.GetComponentsInChildren<CardController>();
        }
        else
        {
            return playerFieldTransform.GetComponentsInChildren<CardController>();
        }
    }

    // 味方プレイヤーのフィールドのカードを全て取得する
    public CardController[] GetFriendFieldCards(bool isPlayer)
    {
        if (isPlayer)
        {
            return playerFieldTransform.GetComponentsInChildren<CardController>();
        }
        else
        {
            return enemyFieldTransform.GetComponentsInChildren<CardController>();
        }
    }

    //ターンを切り替える
    public void ChangeTurn()
    {
        isPlayerTurn = !isPlayerTurn;

        // 全てのカードをドラッグ不可にする
        CardController[] playerFieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        SettingCanAttackView(playerFieldCardList, false);
        CardController[] enemyFieldCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();
        SettingCanAttackView(enemyFieldCardList, false);

        if (isPlayerTurn)
        {
            player.IncreaseManaCost();
            GiveCardToHand(player.deck, playerHandTransform);
        }
        else
        {
            enemy.IncreaseManaCost();
            GiveCardToHand(enemy.deck, enemyHandTransform);
        }
        uiManager.ShowManaCost(player.manaCost, enemy.manaCost);
        TurnCalculation();
    }

    // 攻撃可能時のオーラをセットする
    public void SettingCanAttackView(CardController[] fieldCardList, bool canAttack)
    {
        foreach (CardController card in fieldCardList)
        {
            card.SetCanAttack(canAttack);
        }
    }

    // ターンのはじめにそのターンがどのプレイヤーのターンかを表示する
    IEnumerator DisplayTurnController(GameObject turnController)
    {
        GameObject instance = (GameObject)Instantiate(turnController,
                                      new Vector3(0.0f, 0.0f, 0.0f),
                                      Quaternion.identity);
        instance.transform.SetParent(canvas.transform, false);

        yield return new WaitForSeconds(2.0f);

        Destroy(instance);
    }

    //プレイヤーのターンの処理
    IEnumerator PlayerTurn()
    {
        yield return new WaitForSeconds(2.0f);

        Debug.Log("プレイヤーのターン");

        // ターンエンドボタンをクリック可能にする
        uiManager.CanClickTheButton();

        // フィールドのカードを攻撃可能にする
        CardController[] playerFieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        SettingCanAttackView(playerFieldCardList, true);
    }


    // カード同士の対戦
    public void CardsBattle(CardController attacker, CardController defender)
    {
        Debug.Log("CardsBattle");
        Debug.Log("アタッカーHP:" + attacker.model.hp);
        Debug.Log("ディフェンダーHP:" + defender.model.hp);

        attacker.Attack(defender);
        defender.Attack(attacker);
        Debug.Log("アタッカーHP:" + attacker.model.hp);
        Debug.Log("ディフェンダーHP:" + defender.model.hp);
        attacker.CheckAlive();
        defender.CheckAlive();
    }

    // マナコストを減らす処理
    public void ReduceManaCost(int cost, bool isPlayerCard)
    {
        if (isPlayerCard)
        {
            player.manaCost -= cost;
        }
        else
        {
            enemy.manaCost -= cost;
        }

        uiManager.ShowManaCost(player.manaCost, enemy.manaCost);
    }

    // Heroへの攻撃処理
    public void AttackToHero(CardController attacker)
    {
        if (attacker.model.isPlayerCard)
        {
            enemy.heroHp -= attacker.model.at;
        }
        else
        {
            player.heroHp -= attacker.model.at;
        }

        attacker.SetCanAttack(false);
        uiManager.ShowHeroHp(player.heroHp, enemy.heroHp);
    }

    // Heroへの回復処理
    public void HealHero(CardController healCard)
    {
        if (healCard.model.isPlayerCard)
        {
            player.heroHp += healCard.model.at;
        }
        else
        {
            enemy.heroHp += healCard.model.at;
        }

        uiManager.ShowHeroHp(player.heroHp, enemy.heroHp);
    }

    // HeroのHpを確認する
    public void CheckHeroHp()
    {
        if (player.heroHp <= 0 || enemy.heroHp <= 0)
        {
            ShowResultPanel(player.heroHp);
        }
        else
        {
            // Hpが生存している場合は処理はなし
        }
    }

    //対戦結果表示画面
    void ShowResultPanel(int heroHp)
    {
        StopAllCoroutines();
        uiManager.ShowResultPanel(heroHp);
    }
}
