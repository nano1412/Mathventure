using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverSize : MonoBehaviour ,IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{

    [SerializeField] float scaleOnHover = 1.10f;
    [SerializeField] float scaleOnPress = 0.9f;
    [SerializeField] private Ease scaleEase = Ease.OutBack;
    [SerializeField] private bool scaleAnimations = true;

    private float originalScale;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (scaleAnimations)
            transform.DOScale(scaleOnPress, 0.15f).SetEase(scaleEase);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (scaleAnimations)
            transform.DOScale(scaleOnHover, 0.15f).SetEase(scaleEase);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (scaleAnimations)
            transform.DOScale(originalScale, 0.15f).SetEase(scaleEase);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (scaleAnimations)
            transform.DOScale(scaleOnHover, 0.15f).SetEase(scaleEase);
    }

    void Start()
    {
        originalScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
