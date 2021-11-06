using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    CardView view;// 見かけ(view)に関することを操作
    public CardModel model; // データ(model)に関することを操作
    public CardMovement movement; // 移動(movement)に関することを操作
    GameManager gameManager;

    public bool IsSpell
    {
        get { return model.spell != SPELL.NONE; }
    }

    private void Awake()
    {
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();
        gameManager = GameManager.instance;
    }

    public void Init(int cardID, bool isPlayer)
    {
        model = new CardModel(cardID, isPlayer);
        view.SetCard(model);
    }

    // カードの見た目の更新
    public void RefreshView()
    {
        view.Refresh(model);
    }

    // カードの見た目を表示
    public void ShowEnemyCard()
    {
        view.Show();
    }

    // 回復処理
    public void Heal(CardController friendCard)
    {
        model.HealCard(friendCard);
        friendCard.RefreshView();
    }

    //攻撃時の処理
    public void Attack(CardController enemyCard)
    {
        model.Attack(enemyCard);
        SetCanAttack(false);
    }

    //　攻撃可能ステータスの操作
    public void SetCanAttack(bool canAttack)
    {
        model.canAttack = canAttack;
        view.ControllSelectablePanel(canAttack);
    }

    // フィールドに置かれたかどうか
    public void OnField()
    {
        GameManager.instance.ReduceManaCost(model.cost, model.isPlayerCard);
        model.isFieldCard = true;

        // 速攻アビリティの処理
        if (model.ability == ABILITY.QUICK_ATTACK)
        {
            SetCanAttack(true);
        }
    }

    // カードの生存確認
    public void CheckAlive()
    {
        if (model.isAlive)
        {
            RefreshView();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // スペルカードが使用可能な状況かをチェックする
    public bool CanUseSpell()
    {
        switch (model.spell)
        {
            case SPELL.DAMAGE_ENEMY_CARD:
            case SPELL.DAMAGE_ENEMY_CARDS:
                CardController[] enemyCards = GameManager.instance.GetEnemyFieldCards(this.model.isPlayerCard);
                if (enemyCards.Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case SPELL.DAMAGE_ENEMY_HERO:
            case SPELL.HEAL_FRIEND_HERO:
                return true;
            case SPELL.HEAL_FRIEND_CARD:
            case SPELL.HEAL_FRIEND_CARDS:
                CardController[] friendCards = gameManager.GetFriendFieldCards(this.model.isPlayerCard);
                if (friendCards.Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case SPELL.NONE:
                return false;

        }
        return false;
    }


    // スペルカードを使用する
    public void UseSpellTo(CardController target)
    {
        switch (model.spell)
        {
            case SPELL.DAMAGE_ENEMY_CARD:
                // 特定の敵を攻撃する
                if (target == null)
                {
                    return;
                }
                if (target.model.isPlayerCard == model.isPlayerCard)
                {
                    return;
                }
                Attack(target);
                target.CheckAlive();
                break;
            case SPELL.DAMAGE_ENEMY_CARDS:
                // 相手フィールドの全てのカードに攻撃する
                CardController[] enemyCards = GameManager.instance.GetEnemyFieldCards(this.model.isPlayerCard);
                foreach (CardController enemyCard in enemyCards)
                {
                    Attack(enemyCard);
                }
                foreach (CardController enemyCard in enemyCards)
                {
                    enemyCard.CheckAlive();
                }
                break;
            case SPELL.DAMAGE_ENEMY_HERO:
                // 敵ヒーローにダメージを与えるスペル
                gameManager.AttackToHero(this);
                break;
            case SPELL.HEAL_FRIEND_CARD:
                // 味方のカードを回復する（単体）
                if (target == null)
                {
                    return;
                }
                if (target.model.isPlayerCard != model.isPlayerCard)
                {
                    return;
                }
                Heal(target);
                break;
            case SPELL.HEAL_FRIEND_CARDS:
                // 味方のカードを回復する（全体）
                CardController[] friendCards = gameManager.GetFriendFieldCards(this.model.isPlayerCard);
                foreach (CardController friendCard in friendCards)
                {
                    Heal(friendCard);
                }
                break;
            case SPELL.HEAL_FRIEND_HERO:
                // ヒーローを回復する
                gameManager.HealHero(this);
                break;
            case SPELL.NONE:
                // スペルカードじゃない場合は返す
                return;

        }

        GameManager.instance.ReduceManaCost(model.cost, model.isPlayerCard);
        Destroy(this.gameObject);
    }
}
