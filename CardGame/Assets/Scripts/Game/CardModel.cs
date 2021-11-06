using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel
{
    // カードデータとそのものとその処理
    public string name;
    public int hp;
    public int at;
    public int cost;
    public Sprite icon;
    public ABILITY ability;
    public SPELL spell;

    // カードが生存してるかどうか
    public bool isAlive;

    // カードが攻撃可能かどうか
    public bool canAttack;

    // カードがフィールドのカードかどうか
    public bool isFieldCard;

    // プレイヤーのカードかどうか
    public bool isPlayerCard;

    public CardModel(int cardID, bool isPlayer)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card"+ cardID);
        name = cardEntity.name;
        hp = cardEntity.hp;
        at = cardEntity.at;
        cost = cardEntity.cost;
        icon = cardEntity.icon;
        ability = cardEntity.ability;
        spell = cardEntity.spell;

        isAlive = true;
        isPlayerCard = isPlayer;
    }

    // 回復関数
    void RecoveryHP(int point)
    {
        hp += point;
    }
    public void HealCard(CardController card)
    {
        //　将来は値の表記も消したい
        card.model.RecoveryHP(at);
    }

    // ダメージ関数
    void Damage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0; 
            isAlive = false;
        }
    }

    //攻撃関数
    public void Attack(CardController card)
    {
        card.model.Damage(at);
    }
}