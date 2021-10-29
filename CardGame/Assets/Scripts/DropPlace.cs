using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// ドロップされる場所の処理
public class DropPlace : MonoBehaviour, IDropHandler
{
    public enum TYPE
    {
        HAND,
        FIELD,
    }
    public TYPE type;
        
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        // ドロップ先が正常化チェック
        if (type == TYPE.HAND)
        {
            return;
        }
        else
        {
            // ドロップ先がフィールドの場合は処理をけいぞl区
        }

        CardController card = eventData.pointerDrag.GetComponent<CardController>();
        if (card != null)
        {

            if (!card.movement.isDraggable)
            {
                return;
            }
            else
            {
                // カードを動かせる場合、処理は継続
            }

            //　スペルカードはフィールドには置かない
            if (card.IsSpell)
            {
                return;
            }

            card.movement.defaultParent = this.transform;

            if (card.model.isFieldCard)
            {
                return;
            }
            else
            {
                // カードがフィールドのカードじゃない場合、処理は継続
            }

            card.OnField();
        }
    }
}
