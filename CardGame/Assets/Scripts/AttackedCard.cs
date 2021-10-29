using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 攻撃される側
public class AttackedCard : MonoBehaviour, IDropHandler
{
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        /* 攻撃 */
        // attackerカードを選択
        CardController attacker = eventData.pointerDrag.GetComponent<CardController>();
        // defenderカードを選択
        CardController defender = GetComponent<CardController>();

        // 敵のフィールドにシールドアビリティカードがある場合他のカードを攻撃できない
        CardController[] enemyFieldCards = GameManager.instance.GetEnemyFieldCards(attacker.model.isPlayerCard);
        if (Array.Exists(enemyFieldCards, card => card.model.ability == ABILITY.SHIELD) && defender.model.ability != ABILITY.SHIELD)
        {
            return;
        }
        else
        {
            //elseは通常処理
        }

        // nullチェック
        if (attacker == null || defender == null)
        {
            return;
        }
        else
        {
            // 味方には攻撃しない
            if (attacker.model.isPlayerCard == defender.model.isPlayerCard)
            {
                return;
            }
            else
            {
                // フラグが異なる時は処理を継続
            }

            if (attacker.model.canAttack)
            {
                // attackerとdefenderを戦わせる
                GameManager.instance.CardsBattle(attacker, defender);
            }
            else
            {
                //falseのときは戦わない
            }
        }

    }
}
