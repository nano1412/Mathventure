using UnityEngine;

public class Consumable : Item
{
    public override bool UseItem()
    {
        string name = ItemType.ToString();

        if (name.StartsWith("Card_")) return HandleUseIemWithCard();
        if (name.StartsWith("Hero_")) return HandleUseIemWithHero();
        if (name.StartsWith("Enemy_")) return HandleUseIemWithEnemy();

        return false;
    }

    bool HandleUseIemWithCard()
    {
        Debug.Log("not imprement yet");
        return false;
    }


    bool HandleUseIemWithHero()
    {
        Debug.Log("not imprement yet");
        return false;
    }

    bool HandleUseIemWithEnemy()
    {
        Debug.Log("not imprement yet");
        return false;
    }
}
