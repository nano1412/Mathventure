using TMPro;
using UnityEngine;

public class cardInDeckDisplay : MonoBehaviour
{
    [field:SerializeField] public TMP_Text CardAmountText { get; private set; }
    // Update is called once per frame
    void Update()
    {
        int currentCardInDeck = CardPlayGameController.current.RoundDeck.cardDatas.Count;
        int maxCardInDeck = CardPlayGameController.current.PersistentDeck.cardDatas.Count;
        CardAmountText.text = currentCardInDeck + "/" + maxCardInDeck;
    }
}
