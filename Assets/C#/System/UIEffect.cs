using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Text Switching")]
    public GameObject normalText;   
    public GameObject highlightText;

    [Header("Click Scale Effect")]
    public float clickScale = 0.95f;
    public float animationSpeed = 15f;

    private Vector3 defaultScale;
    private Vector3 targetScale;

    void Start()
    {
        defaultScale = transform.localScale;
        targetScale = defaultScale;

        if (normalText != null) normalText.SetActive(true);
        if (highlightText != null) highlightText.SetActive(false);
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (normalText != null) normalText.SetActive(false);
        if (highlightText != null) highlightText.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (normalText != null) normalText.SetActive(true);
        if (highlightText != null) highlightText.SetActive(false);

        targetScale = defaultScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        targetScale = defaultScale * clickScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetScale = defaultScale;
    }
}