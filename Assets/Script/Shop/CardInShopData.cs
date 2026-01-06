using TMPro;
using UnityEngine;
using static Utils;

public class CardInShopData : MonoBehaviour
{
    [field:SerializeField] public int FaceValue { get; private set; }
    [field: SerializeField] public int EffectiveValue { get; private set; }

    [field: SerializeField] public TMP_Text FaceValueText { get; private set; }
    [field: SerializeField] public TMP_Text EffectiveValueText { get; private set; }

    [field: SerializeField] public int MinFace { get; private set; } = 0;
    [field: SerializeField] public int MaxFace { get; private set; } = 10;
    [field: SerializeField] public int MinEffective { get; private set; } = 5;
    [field: SerializeField] public int MaxEffective { get; private set; } = 10;

    [field: SerializeField] public bool IsFaceNegativeSpawnable { get; private set; } = false;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameController.current.PossibleOperators.Contains(OperationEnum.Multiply) || GameController.current.PossibleOperators.Contains(OperationEnum.Divide))
        {
            IsFaceNegativeSpawnable = true;
        }

        if (IsFaceNegativeSpawnable)
        {
            MinFace = -MaxFace;
        }
        FaceValue = rnd.Next(MinFace, MaxFace);
        EffectiveValue = rnd.Next(MinEffective, MaxEffective);

        FaceValueText.text = FaceValue.ToString();
        EffectiveValueText.text = EffectiveValue.ToString();
    }

}
