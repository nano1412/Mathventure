using TMPro;
using UnityEngine;

public class GameDataDisplay : MonoBehaviour
{
    [field: SerializeField] private TMP_Text coinsText;
    [field: SerializeField] private TMP_Text levelText;
    [field: SerializeField] private TMP_Text waveText;
    [field: SerializeField] private TMP_Text isEndlessText;
    // Update is called once per frame
    void Update()
    {
        coinsText.text = "เหรียญ: " + InventoryController.current.Coin.ToString();
        levelText.text = "ด่าน: " + GameController.current.Level.ToString();
        waveText.text = "รอบที่: " + GameController.current.Wave.ToString();
        isEndlessText.gameObject.SetActive(GameController.current.IsEndless);
    }
}
