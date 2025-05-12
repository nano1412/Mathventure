using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController current;
    public GameObject playedCardHandle;
    public GameObject cardInHand;
    public GameObject operatorInHand;
    public GameObject playedCardSlots;
    public GameObject playedOperatorSlots;

    private void Awake()
    {
        current = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playedCardSlots = playedCardHandle.transform.Find("NumberCard").gameObject;
        playedOperatorSlots = playedCardHandle.transform.Find("OparetorCard").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool RectOverlaps(RectTransform rt1, RectTransform rt2)
    {
        Rect r1 = GetWorldRect(rt1);
        Rect r2 = GetWorldRect(rt2);
        return r1.Overlaps(r2);
    }

    private static Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];
        return new Rect(bottomLeft, topRight - bottomLeft);
    }
}
