using UnityEngine;
using UnityEngine.UI;
using static Utils;

public class CharacterPreview : MonoBehaviour
{
    [field: SerializeField] public OperationEnum operationEnum { get; private set; }
    [field: SerializeField] public Image HeroImg { get; private set; }
    [field:SerializeField] public GameObject TextGameObject { get; private set; }
    [field: SerializeField] public GameObject LockGameObject { get; private set; }

    private void Update()
    {
        if (LevelCreator.current.PossibleOperators.Contains(operationEnum))
        {
            EnableCharacter();
        } else
        {
            DisableCharacter();
        }
    }

    void EnableCharacter()
    {
        HeroImg.color = Color.white;
        TextGameObject.SetActive(true);
        LockGameObject.SetActive(false);
    }

    void DisableCharacter()
    {
        HeroImg.color = Color.black;
        TextGameObject.SetActive(false);
        LockGameObject.SetActive(true);
    }

}
