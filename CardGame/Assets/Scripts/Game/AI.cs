using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] UIManager uiManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    //敵ターンの処理
    public IEnumerator EnemyTurn()
    {
        // ターンエンドボタンはクリックできない
        uiManager.CannotClickTheButton();

        yield return new WaitForSeconds(2.0f);

        Debug.Log("敵のターン");     

        // 敵のカードを攻撃可能にする
        CardController[] enemyFieldCardList = gameManager.enemyFieldTransform.GetComponentsInChildren<CardController>();
        gameManager.SettingCanAttackView(enemyFieldCardList, true);

        yield return new WaitForSeconds(1);

        /* 場にカードを出す */
        // 手札のカードリストを取得
        CardController[] handCardList = gameManager.enemyHandTransform.GetComponentsInChildren<CardController>();

        // 手札にマナコスト以下のカードがあれば、出し続ける
        // 条件：モンスターカードならコストのみ
        // 条件：スペルならコストと、使用可能かどうか（CanUseSpell）
        while (Array.Exists(handCardList, card => (card.model.cost < gameManager.enemy.manaCost) && (!card.IsSpell || (card.IsSpell && card.CanUseSpell()))))
        {
            //　マナコスト以下のカードリストを取得
            CardController[] handCardListLowerThanMana = Array.FindAll(handCardList, card => (card.model.cost < gameManager.enemy.manaCost) && (!card.IsSpell || (card.IsSpell && card.CanUseSpell())));
            // 場に出すカードを選択
            CardController selectCard = handCardListLowerThanMana[0];
            // カードを選択したらカードを表面にする
            selectCard.ShowEnemyCard();
            //スペルカードなら使用する
            if (selectCard.IsSpell)
            {
                StartCoroutine(ActivateSpell(selectCard));               
            }
            else
            {
                // カードを移動
                StartCoroutine(selectCard.movement.MoveToField(gameManager.enemyFieldTransform));
                //敵のカードがフィールドに置かれた場合の処理
                selectCard.OnField();
            }
            // カードを出した後の状態の手札を取得する
            yield return new WaitForSeconds(1);
            handCardList = gameManager.enemyHandTransform.GetComponentsInChildren<CardController>();
        }

        yield return new WaitForSeconds(1);

        /* 攻撃 */
        // フィールドのカードリストを取得
        CardController[] enemyfieldCardList = gameManager.enemyFieldTransform.GetComponentsInChildren<CardController>();

        // 攻撃可能カードがあれば、繰り返し攻撃を行う
        while (Array.Exists(enemyfieldCardList, card => card.model.canAttack))
        {
            // 攻撃可能カードを取得
            CardController[] enemyCanAttackCardList = Array.FindAll(enemyfieldCardList, card => card.model.canAttack);
            CardController[] playerFieldCardList = gameManager.playerFieldTransform.GetComponentsInChildren<CardController>();
            // attackerカードを選択
            CardController attacker = enemyCanAttackCardList[0];

            if (playerFieldCardList.Length > 0)
            {
                // defenderカードを選択
                // シールドカードがあれば、そのカードを攻撃する
                if (Array.Exists(playerFieldCardList, card => card.model.ability == ABILITY.SHIELD))
                {
                    playerFieldCardList = Array.FindAll(playerFieldCardList, card => card.model.ability == ABILITY.SHIELD);
                }
                else
                {
                    // elseは通常処理
                }

                CardController defender = playerFieldCardList[0];
                // attackerとdefenderを戦わせる
                StartCoroutine(attacker.movement.MoveToTarget(defender.transform));
                yield return new WaitForSeconds(0.51f);
                gameManager.CardsBattle(attacker, defender);
            }
            else
            {
                StartCoroutine(attacker.movement.MoveToTarget(gameManager.playerHero));
                yield return new WaitForSeconds(0.25f);
                gameManager.AttackToHero(attacker);
                yield return new WaitForSeconds(0.5f);
                gameManager.CheckHeroHp();
            }
            enemyfieldCardList = gameManager.enemyFieldTransform.GetComponentsInChildren<CardController>();
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);

        gameManager.ChangeTurn();
    }

    // スペルカードを使用する
    IEnumerator ActivateSpell(CardController card)
    {
        CardController target = null;

        // カードの移動先
        Transform movePosition = null;
        switch(card.model.spell)
        {
            case SPELL.DAMAGE_ENEMY_CARD:
                target = gameManager.GetEnemyFieldCards(card.model.isPlayerCard)[0];
                movePosition = target.transform;
                break;
            case SPELL.HEAL_FRIEND_CARD:
                target = gameManager.GetFriendFieldCards(card.model.isPlayerCard)[0];
                movePosition = target.transform;
                break;
            case SPELL.DAMAGE_ENEMY_CARDS:
                movePosition = gameManager.playerFieldTransform;
                break;
            case SPELL.HEAL_FRIEND_CARDS:
                movePosition = gameManager.enemyFieldTransform;
                break;
            case SPELL.DAMAGE_ENEMY_HERO:
                movePosition = gameManager.playerHero;
                break;
            case SPELL.HEAL_FRIEND_HERO:
                movePosition = gameManager.enemyHero;
                break;
        }

        // カードを移動
        StartCoroutine(card.movement.MoveToField(movePosition));
        yield return new WaitForSeconds(0.25f);

        // スペルを使う
        card.UseSpellTo(target);
    }
}
