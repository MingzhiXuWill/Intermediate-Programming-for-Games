using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public RectTransform rectTrans;

    // UI

    [SerializeField]
    Image cardArt;
    [SerializeField]
    TextMeshProUGUI text_CardName;
    [SerializeField]
    TextMeshProUGUI text_Cost;

    [HideInInspector]
    public CardManager cardManager;
    [HideInInspector]
    public CardProperty cardProperty;

    [SerializeField]
    Vector3 hoveringScale = new Vector3(2, 2, 2);
    [SerializeField]
    float hoveringY = 200;
    [SerializeField]
    Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1.2f);
    [SerializeField]
    float lerpSpeed = 3;

    [HideInInspector]
    public Vector2 targetPosition = Vector2.zero;
    [HideInInspector]
    public Quaternion targetRotation = new Quaternion();

    //Card Status
    bool mouseRollOver = false;
    bool allowSelect = false;
    bool isDragging = false;


    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        Invoke("AllowSelect", 1);
    }

    void AllowSelect()
    {
        allowSelect = true;
    }

    public void Init(CardProperty cp)
    {
        // Init the card and display all the info.
        cardProperty = cp;
        cardArt.sprite = cp.art;
        text_CardName.text = cp.name;
        text_Cost.text = cp.cost.ToString();
    }

    void Update()
    {
        // Nothing happens || Card moving back but mouse roll over || Not in ready status
        if ((!mouseRollOver && !isDragging) || !allowSelect && mouseRollOver || CardManager.gameStatus != GameStatus.Ready)
        {
            rectTrans.anchoredPosition = Vector2.Lerp(rectTrans.anchoredPosition, targetPosition, Time.deltaTime * lerpSpeed);
            rectTrans.rotation = Quaternion.Slerp(rectTrans.rotation, rectTrans.parent.rotation * targetRotation, Time.deltaTime * lerpSpeed);
        }
        else if (allowSelect && mouseRollOver && !isDragging)
        {
            rectTrans.localScale = hoveringScale;
            rectTrans.anchoredPosition = new Vector2(targetPosition.x, hoveringY);
            rectTrans.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!isDragging)
            {
                isDragging = true;
                cardManager.currentSelectedCard = this;
                Debug.Log("Begin Drag");
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isDragging)
            {
                // move the card so it follows the mouse
                Vector3 globalMousePos;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTrans, eventData.position, eventData.pressEventCamera, out globalMousePos))
                {
                    rectTrans.position = globalMousePos;
                    Debug.Log("Drag");
                }
            }
        }

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isDragging)
            {
                isDragging = !isDragging;
                Debug.Log("End Drag");
                if (CardManager.gameStatus == GameStatus.Ready)
                {
                    //Taking effect of the card
                    if (cardManager.TakingEffectCheck())
                    {
                        Debug.Log("Card not Used");

                    }
                }

                rectTrans.localScale = Vector3.one;
                allowSelect = false;
                Invoke("AllowSelect", 0.5f);
                cardManager.currentSelectedCard = null;
            }   
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardManager.currentSelectedCard == null)
        {
            mouseRollOver = true;
            cardManager.RelocateAllCards(this);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseRollOver = false;
        rectTrans.localScale = Vector3.one;

        cardManager.RelocateAllCards();
    }
}
