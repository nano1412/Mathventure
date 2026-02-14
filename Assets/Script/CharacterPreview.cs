using UnityEngine;
using UnityEngine.UI;

public class CharacterPreview : MonoBehaviour
{
    [field:Header("Plus"), SerializeField]
    public Image PlusHeroImg { get; private set; }
    [field:SerializeField] public GameObject PlusTextGameObject { get; private set; }
    [field: SerializeField] public GameObject PlusLockGameObject { get; private set; }

    [field: Header("Minus"), SerializeField]
    public Image MinusHeroImg { get; private set; }
    [field: SerializeField] public GameObject MinusTextGameObject { get; private set; }
    [field: SerializeField] public GameObject MinusLockGameObject { get; private set; }

    [field: Header("Multiply"), SerializeField]
    public Image MultiplyHeroImg { get; private set; }
    [field: SerializeField] public GameObject MultiplyTextGameObject { get; private set; }
    [field: SerializeField] public GameObject MultiplyLockGameObject { get; private set; }

    [field: Header("Divide"), SerializeField]
    public Image DivideHeroImg { get; private set; }
    [field: SerializeField] public GameObject DivideTextGameObject { get; private set; }
    [field: SerializeField] public GameObject DivideLockGameObject { get; private set; }

    [field: Header("Buff"), SerializeField]
    public Image BuffHeroImg { get; private set; }
    [field: SerializeField] public GameObject BuffTextGameObject { get; private set; }
    [field: SerializeField] public GameObject BuffLockGameObject { get; private set; }

}
