using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Btn_Exit : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerExitHandler, IDeselectHandler
{
    [SerializeField]
    private Animator m_toggle;

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_toggle.SetBool("isExitHighlighted", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_toggle.SetBool("isExitHighlighted", false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        m_toggle.SetBool("isExitHighlighted", true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        m_toggle.SetBool("isExitHighlighted", false);
    }
}
