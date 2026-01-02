using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class ModifierController : MonoBehaviour
{
    public static ModifierController current;
    [field: SerializeField] public GameObject SelectedCharacter { get; private set; }
    [field: SerializeField] public GameObject SelectedConsumable { get; private set; }

    private void Awake()
    {
        current = this;
    }
    
    public void SetCharacter(GameObject character)
    {
        if (character.GetComponent<Character>())
        {
            SelectedCharacter = character;
        }
    }
}
