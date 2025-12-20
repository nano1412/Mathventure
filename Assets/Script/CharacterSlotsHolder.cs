using System.Collections.Generic;
using UnityEngine;

public class CharacterSlotsHolder : MonoBehaviour
{
    [field: SerializeField] public List<Transform> CharacterSlots { get; private set; }
    private List<GameObject> characters = new();
    public IReadOnlyList<GameObject> Characters => characters;

    private void Start()
    {
        foreach(Transform characterSlot in CharacterSlots)
        {
            if(characterSlot.childCount > 0)
            {
                this.characters.Add(characterSlot.GetChild(0).gameObject);
            }
        }
    }

    public void SpawnCharacters(List<GameObject> characters)
    {
        int spawnCount = Mathf.Min(characters.Count, CharacterSlots.Count);
        this.characters.Clear();

        if (characters.Count > CharacterSlots.Count)
        {
            Debug.LogWarning(
                $"characters count ({characters.Count}) is more than CharacterSlots ({CharacterSlots.Count}). Only spawning {spawnCount}."
            );
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Transform slot = CharacterSlots[i];

            for (int j = slot.childCount - 1; j >= 0; j--)
            {
                Destroy(slot.GetChild(j).gameObject);
            }

            if (characters[i] == null)
            {
                continue;
            }

            GameObject instance = Instantiate(characters[i], slot, false);
            this.characters.Add(instance);
        }
    }

    public void UpdateCharactersPosition()
    {
        characters.RemoveAll(c => c == null);
        // Clear slots
        foreach (var slot in CharacterSlots)
        {
            if (slot.transform.childCount > 0)
                slot.transform.GetChild(0).SetParent(null);
        }


        // Reassign based on characters list
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].transform.SetParent(CharacterSlots[i], false);
        }
    }

    public List<GameObject> GetFirstCharacterAsList()
    {
        return characters.Count >= 1
        ? new List<GameObject> { characters[0] }
        : new List<GameObject>();
    }

    public List<GameObject> GetFirstTwoCharactersAsList()
    {
        return characters.Count >= 2
        ? new List<GameObject> { characters[0], characters[1] }
        : new List<GameObject>();
    }

    public List<GameObject> GetLastCharacterAsList()
    {
        return characters.Count >= 1
        ? new List<GameObject> { characters[^1] }
        : new List<GameObject>();
    }

    public List<GameObject> GetAllCharactersAsList()
    {
        return characters.Count >= 1
        ? new List<GameObject>(characters)
        : new List<GameObject>();
    }
}