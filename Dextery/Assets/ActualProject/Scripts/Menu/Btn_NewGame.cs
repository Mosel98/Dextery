using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//script by Tamara 
public class Btn_NewGame : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerExitHandler, IDeselectHandler
{
    [SerializeField]
    private Animator m_toggle;

    // check if button is highlighted or not --> for animation

    public void OnDeselect(BaseEventData eventData)
    {
        m_toggle.SetBool("isNewHighlighted", false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_toggle.SetBool("isNewHighlighted", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_toggle.SetBool("isNewHighlighted", false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        m_toggle.SetBool("isNewHighlighted", true);
    }

}
