using TMPro;
using UnityEngine;

[System.Serializable]
public class stageDescript
{
    public string title;
    public string description;
}

public class StageDescription : MonoBehaviour
{
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject description;
    [Header("Stage Descriptions")]
    [SerializeField] private stageDescript[] descriptions;


    void Update()
    {
        stageDescript stageData = descriptions[LevelCreator.current.Level - 1];
        if (stageData != null)
        {
            title.GetComponent<TextMeshProUGUI>().text = stageData.title;
            description.GetComponent<TextMeshProUGUI>().text = stageData.description;
        }
    }
}
