using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonToggle : Button
{
    public GameObject Label1;
    public GameObject Label2;
    public bool isToggled;

    public UnityEvent<bool> OnToggle;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        isToggled = !isToggled;
        Label1.SetActive(!isToggled);
        Label2.SetActive(isToggled);
        OnToggle?.Invoke(isToggled);
    }
}
