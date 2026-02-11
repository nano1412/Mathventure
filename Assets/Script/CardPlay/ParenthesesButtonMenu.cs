using TMPro;
using UnityEngine;
using static Utils;

public class ParenthesesButtonMenu : MonoBehaviour
{
    [field: SerializeField] private bool IsFrontParentheses;
    [field: SerializeField] private bool IsMiddleParentheses;
    [field: SerializeField] private bool IsLastParentheses;

    [field:SerializeField] public TMP_Text FrontParenthesesBtnText { get; private set; }
    [field: SerializeField] public TMP_Text MiddleParenthesesBtnText { get; private set; }
    [field: SerializeField] public TMP_Text LastParenthesesBtnText { get; private set; }


    public void ToggleIsFrontParentheses()
    {
        IsFrontParentheses = !IsFrontParentheses;
        IsMiddleParentheses = false;
        CheckParentheses();
        UpdateButtonText();
    }

    public void ToggleIsMiddleParentheses()
    {
        IsMiddleParentheses = !IsMiddleParentheses;
        IsFrontParentheses = false;
        IsLastParentheses = false;
        CheckParentheses();
        UpdateButtonText();
    }

    public void ToggleIsLastParentheses()
    {
        IsLastParentheses = !IsLastParentheses;
        IsMiddleParentheses = false;
        CheckParentheses();
        UpdateButtonText();
    }

    private void CheckParentheses()
    {
        if (IsMiddleParentheses)
        {
            CardPlayGameController.current.ParenthesesMode = ParenthesesMode.DoMiddleOperationFirst;
            return;
        }

        if(IsFrontParentheses && !IsLastParentheses)
        {
            CardPlayGameController.current.ParenthesesMode = ParenthesesMode.DoFrontOperationFirst;
            return;
        }

        if (!IsFrontParentheses && IsLastParentheses)
        {
            CardPlayGameController.current.ParenthesesMode = ParenthesesMode.DoLastOperationFirst;
            return;
        }

        if (IsFrontParentheses && IsLastParentheses)
        {
            CardPlayGameController.current.ParenthesesMode = ParenthesesMode.DoMiddleOperationLast;
            return;
        }

        CardPlayGameController.current.ParenthesesMode = ParenthesesMode.NoParentheses;
        return;
    }

    private void UpdateButtonText()
    {
        string addText = "add Parentheses";
        string removeText = "remove Parentheses";

        if (IsFrontParentheses)
        {
            FrontParenthesesBtnText.text = removeText;
        } else
        {
            FrontParenthesesBtnText.text = addText;
        }

        if (IsMiddleParentheses)
        {
            MiddleParenthesesBtnText.text = removeText;
        }
        else
        {
            MiddleParenthesesBtnText.text = addText;
        }

        if (IsLastParentheses)
        {
            LastParenthesesBtnText.text = removeText;
        }
        else
        {
            LastParenthesesBtnText.text = addText;
        }
    }
}
