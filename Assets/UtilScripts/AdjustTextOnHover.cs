using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AdjustTextOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Color hoverColor;
    private Color baseColor;
    private TMPro.TMP_Text buttonText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonText = this.transform.GetComponentInChildren<TMP_Text>();
        baseColor = buttonText.color;
    }

    void changeTextColor(bool hovered)
    {
        if (hovered)
        {
            buttonText.color = hoverColor;
        }
        else
        {
            buttonText.color = baseColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        changeTextColor(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        changeTextColor(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        changeTextColor(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        changeTextColor(false);
    }
}
