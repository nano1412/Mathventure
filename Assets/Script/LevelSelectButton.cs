using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [field: SerializeField] public int LevelValue { get; private set; }
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public Button LevelSelectBtn { get; private set; }
    [field:SerializeField] public GameObject Lock { get; private set; }
    [field: SerializeField] public TMP_Text TitleText { get; private set; }

    private void Start()
    {
        Lock.SetActive(true);
        TitleText.text = Title;
    }

    private void Update()
    {
        if (LevelValue > MenuNavigation.current.MaxLevelUnlock)
        {
            DisableButton();
        }
        else
        {
            EnableButton();
        }
    }

    void EnableButton()
    {
        LevelSelectBtn.enabled = true;
        Lock.SetActive(false);
    }

    private void DisableButton()
    {
        LevelSelectBtn.enabled = false;
        Lock.SetActive(true);
    }
}
