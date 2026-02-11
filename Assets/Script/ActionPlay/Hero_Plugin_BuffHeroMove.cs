using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class Hero_Plugin_BuffHeroMove : MonoBehaviour
{
    [field: SerializeField] public Move PlusBuffMove { get; private set; }
    [field: SerializeField] public Move MinusBuffMove { get; private set; }
    [field: SerializeField] public Move MultiplyBuffMove { get; private set; }
    [field: SerializeField] public Move DivideBuffMove { get; private set; }

    public Move GetMoveByOperator(OperationEnum operationEnum)
    {
        switch (operationEnum)
        {
            case OperationEnum.Plus:
                return PlusBuffMove;
            case OperationEnum.Minus:
                return MinusBuffMove;
            case OperationEnum.Multiply:
                return MultiplyBuffMove;
            case OperationEnum.Divide:
                return DivideBuffMove;
            default:
                return null;
        }
    }
}
