using System.Collections.Generic;
using UnityEngine;
using static Utils;

public class LevelCreatorSetter : MonoBehaviour
{
    [field: SerializeField] public Sprite Level1BGSprite { get; private set; }
    [field: SerializeField] public Sprite Level2BGSprite { get; private set; }
    [field: SerializeField] public Sprite Level3BGSprite { get; private set; }
    [field: SerializeField] public Sprite Level4BGSprite { get; private set; }
    [field: SerializeField] public Sprite LevelEndlessBGSprite { get; private set; }

    [field: SerializeField] public List<GameObject> Level1SpawnableItemsInShop { get; private set; }
    [field: SerializeField] public List<GameObject> Level2SpawnableItemsInShop { get; private set; }
    [field: SerializeField] public List<GameObject> Level3SpawnableItemsInShop { get; private set; }
    [field: SerializeField] public List<GameObject> Level4SpawnableItemsInShop { get; private set; }
    [field: SerializeField] public List<GameObject> LevelEndlessSpawnableItemsInShop { get; private set; }

    [field: SerializeField] public CharacterPreview CharacterPreview { get; private set; }

    public void SetLevel1Btn()
    {
        List<OperationEnum> possibleOperators = new List<OperationEnum>();
        possibleOperators.Add(OperationEnum.Plus);
        LevelCreator.current.SetLevelCreator(Level1BGSprite,1, false, Level1SpawnableItemsInShop, possibleOperators);
    }

    public void SetLevel2Btn()
    {
        List<OperationEnum> possibleOperators = new List<OperationEnum>();
        possibleOperators.Add(OperationEnum.Plus);
        possibleOperators.Add(OperationEnum.Minus);
        LevelCreator.current.SetLevelCreator(Level2BGSprite,2, false, Level2SpawnableItemsInShop, possibleOperators);
    }

    public void SetLevel3Btn()
    {
        List<OperationEnum> possibleOperators = new List<OperationEnum>();
        possibleOperators.Add(OperationEnum.Plus);
        possibleOperators.Add(OperationEnum.Minus);
        possibleOperators.Add(OperationEnum.Multiply);
        LevelCreator.current.SetLevelCreator(Level3BGSprite,3, false, Level3SpawnableItemsInShop, possibleOperators);
    }

    public void SetLevel4Btn()
    {
        List<OperationEnum> possibleOperators = new List<OperationEnum>();
        possibleOperators.Add(OperationEnum.Plus);
        possibleOperators.Add(OperationEnum.Minus);
        possibleOperators.Add(OperationEnum.Multiply);
        possibleOperators.Add(OperationEnum.Divide);
        LevelCreator.current.SetLevelCreator(Level4BGSprite,4, false, Level4SpawnableItemsInShop, possibleOperators);
    }

    public void SetLevelEndlessBtn()
    {
        List<OperationEnum> possibleOperators = new List<OperationEnum>();
        possibleOperators.Add(OperationEnum.Plus);
        possibleOperators.Add(OperationEnum.Minus);
        possibleOperators.Add(OperationEnum.Multiply);
        possibleOperators.Add(OperationEnum.Divide);
        possibleOperators.Add(OperationEnum.Buff);
        LevelCreator.current.SetLevelCreator(LevelEndlessBGSprite,5, true, LevelEndlessSpawnableItemsInShop, possibleOperators);
    }
}
