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
                cardTrans.anchoredPosition3D = new Vector3(600, 100, 0);

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
    public void RelocateAllCards(CardController selectedCard = null)
    {
        if (cards_Hand.Count > 0)
        {
            //Player cards
            if (gameStatus == GameStatus.End)
            {
                for (int i = 0; i < cards_Hand.Count; i++)
                {
                    //Round end, move all the cards to the left corner
                    cards_Hand[i].targetPosition = new Vector2(-Screen.width * 0.6f, -100);
                    cards_Hand[i].targetRotation = Quaternion.Euler(Vector3.zero);
                }
            }
            else if (cards_Hand.Count == 1)
            {
                cards_Hand[0].targetPosition = Vector2.zero;
                cards_Hand[0].targetRotation = Quaternion.Euler(Vector3.zero);
            }
            else
            {
                //Calculate interval
                float cardInterval = (Screen.width - 700) / (cards_Hand.Count - 1);
                //Between 30-150
                cardInterval = Mathf.Clamp(cardInterval, 30, 150);
                float middleIndex = (cards_Hand.Count - 1) * 0.5f;

                //Get current selected card's index
                int selectedCardIndex = 0;
                if (selectedCard != null) selectedCardIndex = cards_Hand.IndexOf(selectedCard);

                for (int i = 0; i < cards_Hand.Count; i++)
                {
                    CardController cardController = cards_Hand[i];
                    //Get card anchor position
                    Vector2 targetPosition = new Vector2((i - middleIndex) * cardInterval, Mathf.Abs(middleIndex - i) * cardInterval * -0.2f);
                    if (selectedCard != null)
                    {
                        //Move away from selected card
                        if (i < selectedCardIndex)
                        {
                            targetPosition.x -= 20000 / cardInterval;
                        }
                        else if (i > selectedCardIndex)
                        {
                            targetPosition.x += 20000 / cardInterval;
                        }
                    }
                    cardController.targetPosition = targetPosition;
                    cardController.targetRotation = Quaternion.Euler(0, 0, (middleIndex - i) * cardInterval * 0.08f);
                }
            }
        }
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
        foreach (CardController cardController in cards_Hand)
        {
            Destroy(cardController.gameObject);
        }

        cards_Hand.Clear();

        RoundStart();
    }

    public bool TakingEffectCheck()
    {
        if (currentSelectedCard != null)
        {
            CardProperty cardProperty = currentSelectedCard.cardProperty;
            if (currentSelectedCard.rectTrans.anchoredPosition.y > 300 && currentEnergy >= currentSelectedCard.cardProperty.cost)
            {
                switch (cardProperty.name)
                {
                    case "Minotaur":
                        print("Applying Special Ability 1");
                        break;
                    case "Dark Protector":
                        print("Applying Special Ability 2");
                        break;
                    case "The Prophecy":
                        print("Applying Special Ability 3");
                        break;
                    case "Space Tunnel":
                        print("Applying Special Ability 4");
                        break;
                    case "Space Secret Army":
                        print("Applying Special Ability 5");
                        break;
                }

                // Used the card energy
                currentEnergy -= currentSelectedCard.cardProperty.cost;

                text_Message.text = "You just used \"" + currentSelectedCard.cardProperty.name + "\" Card";
                text_Energy.text = currentEnergy.ToString();

                MoveCardToDiscardPile(currentSelectedCard);

                return true;
            }  
        }
    return false;
    }

    void MoveCardToDiscardPile(CardController cardController)
    {
        cards_DiscardPile.Add(cardController.cardProperty);
        cards_Hand.Remove(cardController);

        Destroy(cardController.gameObject);

        RelocateAllCards();
    }
}
