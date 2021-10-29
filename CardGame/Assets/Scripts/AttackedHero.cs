using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Heroへの攻撃
public class AttackedHero : MonoBehaviour, IDropHandler
{
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        /* 攻撃 */
        // attackerカードを選択
        CardController attacker = eventData.pointerDrag.GetComponent<CardController>();

        // 敵のフィールドにシールドアビリティカードがある場合、Heroを攻撃できない
        CardController[] enemyFieldCards = GameManager.instance.GetEnemyFieldCards(attacker.model.isPlayerCard);
        if (Array.Exists(enemyFieldCards, card => card.model.ability == ABILITY.SHIELD))
        {
            return;
        }
        else
        {
            //elseは通常処理
        }

        // nullチェック
        if (attacker == null)
        {
            return;
        }
        else
        {
            if (attacker.model.canAttack)
            {
                // attackerがHeroに攻撃する
                GameManager.instance.AttackToHero(attacker);
                //攻撃した際にHeroのHPを確認する
                GameManager.instance.CheckHeroHp();
            }
            else
            {
                //falseのときは戦わない
            }
        }

    }
}
