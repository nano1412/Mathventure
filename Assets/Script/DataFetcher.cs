using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections.Generic;

public static class DataFetcher
{
    private static string baseUrl = "https://app.nocodb.com/api/v2/tables/m3k88oqabf3fdrn/records";
    private static string apiKey = "7T5QdvOgxkFiEtVAIYQg5PiWtw9F0O3myzY5fCJZ";

    public static IEnumerator GetTop8(System.Action<string> callback)
    {
        string url = baseUrl + "?sort=-score&limit=8";

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("xc-token", apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            callback?.Invoke(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError(request.error);
            callback?.Invoke(null);
        }
    }

    public static IEnumerator SubmitScore(string username, int score, System.Action<bool> callback)
    {
        string json = $"{{\"username\":\"{username}\",\"score\":{score}}}";

        UnityWebRequest request = new UnityWebRequest(baseUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("xc-token", apiKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            callback?.Invoke(true);
        }
        else
        {
            Debug.LogError(request.error);
            callback?.Invoke(false);
        }
    }
}

[System.Serializable]
public class LeaderboardResponse
{
    public List<LeaderboardEntryData> list;
}

[System.Serializable]
public class LeaderboardEntryData
{
    public int id;
    public string username;
    public int score;
}