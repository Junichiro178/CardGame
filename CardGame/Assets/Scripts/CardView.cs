using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//カードの見た目
public class CardView : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text hpText;
    [SerializeField] Text atText;
    [SerializeField] Text costText;
    [SerializeField] Image iconImage;
    [SerializeField] GameObject selectablePanel;
    [SerializeField] GameObject shieldPanel;
    [SerializeField] GameObject maskPanel;

    public void SetCard(CardModel cardModel)
    {
        nameText.text = cardModel.name;
        hpText.text = cardModel.hp.ToString();
        atText.text = cardModel.at.ToString();
        costText.text = cardModel.cost.ToString();
        iconImage.sprite = cardModel.icon;

        // カードを隠す処理（敵のカードは隠す）
        maskPanel.SetActive(!cardModel.isPlayerCard);

        // シールドアビリティの見た目の処理
        if (cardModel.ability == ABILITY.SHIELD)
        {
            shieldPanel.SetActive(true);
        }
        else
        {
            shieldPanel.SetActive(false);
        }

        // スペルカードの見た目の処理
        if (cardModel.spell != SPELL.NONE)
        {
            hpText.gameObject.SetActive(false);
        }
        else
        {
            // 他のカードの際に処理はなし
        }

    }

    // カードを見えるようにする
    public void Show()
    {
        maskPanel.SetActive(false);
    }

    // 見た目をリフレッシュ
    public void Refresh(CardModel cardModel)
    {
        hpText.text = cardModel.hp.ToString();
        atText.text = cardModel.at.ToString();
    }

    public void ControllSelectablePanel(bool showPanel)
    {
        selectablePanel.SetActive(showPanel);
    }
}
