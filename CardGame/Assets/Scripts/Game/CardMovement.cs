using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

// カードの動き
public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform defaultParent;

    // ドラッグ可能かどうかのフラグ
    public bool isDraggable;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        // カードのコストとManaCostを比較
        CardController card = GetComponent<CardController>();
        if (card.model.isPlayerCard && GameManager.instance.isPlayerTurn && !card.model.isFieldCard && card.model.cost <= GameManager.instance.player.manaCost)
        {
            isDraggable = true;
        }
        else if (card.model.isPlayerCard && GameManager.instance.isPlayerTurn && card.model.isFieldCard && card.model.canAttack)
        {
            isDraggable = true;
        }
        else
        {
            isDraggable = false;
        }

        if (!isDraggable)
        {
            return;
        }
        else
        {
            // ドラッグ可能ならそのまま処理を続ける
        }

        defaultParent = transform.parent;
        transform.SetParent(defaultParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }
        else
        {
            // ドラッグ可能ならそのまま処理を続ける
        }

        transform.position = eventData.position;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable)
        {
            return;
        }
        else
        {
            // ドラッグ可能ならそのまま処理を続ける
        }

        transform.SetParent(defaultParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public IEnumerator MoveToField(Transform field)
    {
        // 一度親をCanvasに変更する
        transform.SetParent(defaultParent.parent);
        // DOTweenでカードをfieldに移動
        transform.DOMove(field.position, 0.25f);
        yield return new WaitForSeconds(0.25f);
        defaultParent = field;
        transform.SetParent(defaultParent);
    }
    public IEnumerator MoveToTarget(Transform target)
    {
        // 現在の位置と並びを取得
        Vector3 currentPosition = transform.position;
        int siblingIndex = transform.GetSiblingIndex();
        // 一度親をCanvasに変更する
        transform.SetParent(defaultParent.parent);
        // DOTweenでカードを攻撃対象に移動
        transform.DOMove(target.position, 0.25f);
        yield return new WaitForSeconds(0.25f);
        // DOTweenで元の位置に戻る
        transform.DOMove(currentPosition, 0.25f);
        yield return new WaitForSeconds(0.25f);
        if (this != null)
        {
            transform.SetParent(defaultParent);
            transform.SetSiblingIndex(siblingIndex);
        }
    }

    void Start()
    {
        defaultParent = transform.parent;
    }
}
