using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class LeaderBoardManager : MonoBehaviour
{
    public Transform entriesParent;
    public GameObject entryPrefab;
    public TMP_InputField usernameInput;
    public TMP_Text playerScore; 
    public Button submitButton;

    private void Start()
    {
        submitButton.interactable = !LevelCreator.current.IsScoreHasBeenSummit;

        playerScore.text = "Score:" + LevelCreator.current.PlayerFinalScore;

        StartCoroutine(DataFetcher.GetTop8(OnDataReceived));
    }

    public void OnSubmitClicked()
    {
        string username = usernameInput.text;
        int score = LevelCreator.current.PlayerFinalScore;

        submitButton.interactable = false;

        StartCoroutine(DataFetcher.SubmitScore(username, score, (success) =>
        {
            if (success)
            {
                StartCoroutine(DataFetcher.GetTop8(OnDataReceived));
                LevelCreator.current.SetIsScoreHasBeenSummit(true);
            }
        }));
    }

    void OnDataReceived(string json)
    {
        if (string.IsNullOrEmpty(json)) return;

        ClearEntries();

        LeaderboardResponse response = JsonUtility.FromJson<LeaderboardResponse>(json);

        if (response.list == null) return;

        int count = Mathf.Min(10, response.list.Count);

        for (int i = 0; i < count; i++)
        {
            CreateEntry(response.list[i], i + 1);
        }
    }

    void CreateEntry(LeaderboardEntryData data, int rank)
    {
        GameObject obj = Instantiate(entryPrefab, entriesParent);
        obj.GetComponent<LeaderboardEntryUI>().Setup(rank, data.username, data.score);
    }

    void ClearEntries()
    {
        foreach (Transform child in entriesParent)
            Destroy(child.gameObject);
    }
}
