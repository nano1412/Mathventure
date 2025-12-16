using UnityEngine;
using static Utils;

public class ParenthesesButton : MonoBehaviour
{
    public void DoFrontOperationFirst()
    {
        if (CardPlayGameController.current.ParenthesesMode != ParenthesesMode.DoFrontOperationFirst)
        {
            CardPlayGameController.current.ParenthesesMode = ParenthesesMode.DoFrontOperationFirst;
        }
        else
        {
            CardPlayGameController.current.ParenthesesMode = ParenthesesMode.NoParentheses;
        }
    }
    public void DoMiddleOperationFirst()
    {
        if (CardPlayGameController.current.ParenthesesMode != ParenthesesMode.DoMiddleOperationFirst)
        {
            CardPlayGameController.current.ParenthesesMode = ParenthesesMode.DoMiddleOperationFirst;
        }
        else
        {
            CardPlayGameController.current.ParenthesesMode = ParenthesesMode.NoParentheses;
        }
    }
    public void DoLastOperationFirst()
    {
        if (CardPlayGameController.current.ParenthesesMode != ParenthesesMode.DoLastOperationFirst)
        {
            CardPlayGameController.current.ParenthesesMode = ParenthesesMode.DoLastOperationFirst;
        }
        else
        {
            CardPlayGameController.current.ParenthesesMode = ParenthesesMode.NoParentheses;
        }
    }
    public void DoMiddleOperationLast()
    {
        if (CardPlayGameController.current.ParenthesesMode != ParenthesesMode.DoMiddleOperationLast)
        {
            CardPlayGameController.current.ParenthesesMode = ParenthesesMode.DoMiddleOperationLast;
        }
        else
        {
            CardPlayGameController.current.ParenthesesMode = ParenthesesMode.NoParentheses;
        }
    }
}
