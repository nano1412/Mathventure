using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    public static BuffController current;

    public List<CharacterBuff> CurrentCharacterBuff { get; private set; }
    public List<CardBuff> CurrentCardBuff { get; private set; }

    private void Awake()
    {
        current = this;
    }
}
