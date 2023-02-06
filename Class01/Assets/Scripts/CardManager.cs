using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public enum GameStatus { Start, Ready, End }

public class CardManager : MonoBehaviour
{
    // Start: start of the round, cards are drawing
    // Ready: cards in position, ready for player to drag
    // End: player press the end turn button, moving all cards to discard pile

    public static GameStatus gameStatus;

    [SerializeField]
    CardController cardPrefab;

    [SerializeField]
    TextMeshProUGUI text_Message;

    [SerializeField]
    TextMeshProUGUI text_Energy;

    CardDatabase cardDatabase;

    RectTransform cardGroup;

    // 3 lists for cards;
    List<CardController> cards_Hand = new List<CardController>();
    List<CardProperty> cards_DiscardPile = new List<CardProperty>();
    List<CardProperty> cards_DrawPile = new List<CardProperty>();

    bool isDrawingCards;
    int cardDrawingIndex = 0;

    

    float cardDrawingTimer = 0;
    float cardDrawingTimerTotal = 0.2f;

    [HideInInspector]
    public CardController currentSelectedCard;

    private int _drawCardNum = 0;
    public int DrawCardNum
    {
        get
        {
            return _drawCardNum;
        }
        set
        {
            _drawCardNum = Mathf.Max(1, value);
            Debug.Log("Player's draw card number changed");
        }
    }

    int currentEnergy = -1;
    int defaultEnergy = 23;

    void Start()
    {
        RoundStart();

        cardGroup = GameObject.Find("CardGroup").GetComponent<RectTransform>();

        cardDatabase = GameObject.Find("CardDatabase").GetComponent<CardDatabase>();

        foreach (CardProperty cardProperty in cardDatabase.cardData)
        {
            cards_DrawPile.Add(cardProperty);
        }

        _drawCardNum = 5;
    }

    void Update()
    {
        if (isDrawingCards)
        {
            if (cardDrawingTimer > cardDrawingTimerTotal) {
                if (cards_DrawPile.Count == 0) {
                    foreach (CardProperty cp in cards_DiscardPile) {
                        cards_DrawPile.Add(cp);
                    }
                    cards_DiscardPile.Clear();
                }
                CardProperty cardProperty = cards_DrawPile[Random.Range(0, cards_DrawPile.Count)];

                cardDrawingTimer = 0
                    ;
                CardController card = Instantiate(cardPrefab, cardGroup);

                card.Init(cardProperty);


                //Starting point
                RectTransform cardTrans = card.GetComponent<RectTransform>();
                cardTrans.anchoredPosition3D = new Vector3(Random.Range(-600, 600), 100, 0);

                card.cardManager = this;

                //Update the lists
                cards_Hand.Add(card);
                cards_DrawPile.Remove(cardProperty);

                RelocateAllCards();

                cardDrawingIndex++;
                if (cardDrawingIndex >= DrawCardNum)
                {
                    gameStatus = GameStatus.Ready;

                    isDrawingCards = false;
                }
            }
            else
            {
                cardDrawingTimer += Time.deltaTime;
            }
        }
    }

    //relocate cards
    public void RelocateAllCards(CardController seletedCard = null)
    {

    }

    public void RoundStart()
    {
        gameStatus = GameStatus.Start;
        cards_Hand.Clear();

        cardDrawingIndex = 0;
        isDrawingCards = true;

        text_Message.text = currentEnergy == -1 ? "Welcome" : "Next Round";

        currentEnergy = defaultEnergy;
        text_Energy.text = currentEnergy.ToString();
    }

    public void RoundEnd()
    {
        if (gameStatus == GameStatus.Ready)
        {
            gameStatus = GameStatus.End;
            if (cards_Hand.Count > 0)
            {
                foreach (CardController cardController in cards_Hand)
                {
                    cards_DiscardPile.Add(cardController.cardProperty);
                }
            }
            RelocateAllCards();
            Invoke("CleanHandsAndStartNextRound", 0.6f);
        }
    }

    void CleanHandsAndStartNextRound()
    {

    }

    public bool TakingEffectCheck()
    {
        return false;
    }

    void MoveCardToDiscardPile(CardController cardController)
    {

    }
}
