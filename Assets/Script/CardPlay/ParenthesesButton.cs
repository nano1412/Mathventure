using UnityEngine;
using static Utils;

public class ParenthesesButton : MonoBehaviour
{
    public void DoFrontOperationFirst()
    {
        if (CardPlayGameController.current.parenthesesMode != ParenthesesMode.DoFrontOperationFirst)
        {
            CardPlayGameController.current.parenthesesMode = ParenthesesMode.DoFrontOperationFirst;
        }
        else
        {
            CardPlayGameController.current.parenthesesMode = ParenthesesMode.NoParentheses;
        }
    }
    public void DoMiddleOperationFirst()
    {
        if (CardPlayGameController.current.parenthesesMode != ParenthesesMode.DoMiddleOperationFirst)
        {
            CardPlayGameController.current.parenthesesMode = ParenthesesMode.DoMiddleOperationFirst;
        }
        else
        {
            CardPlayGameController.current.parenthesesMode = ParenthesesMode.NoParentheses;
        }
    }
    public void DoLastOperationFirst()
    {
        if (CardPlayGameController.current.parenthesesMode != ParenthesesMode.DoLastOperationFirst)
        {
            CardPlayGameController.current.parenthesesMode = ParenthesesMode.DoLastOperationFirst;
        }
        else
        {
            CardPlayGameController.current.parenthesesMode = ParenthesesMode.NoParentheses;
        }
    }
    public void DoMiddleOperationLast()
    {
        if (CardPlayGameController.current.parenthesesMode != ParenthesesMode.DoMiddleOperationLast)
        {
            CardPlayGameController.current.parenthesesMode = ParenthesesMode.DoMiddleOperationLast;
        }
        else
        {
            CardPlayGameController.current.parenthesesMode = ParenthesesMode.NoParentheses;
        }
    }
}
