using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Switch : MonoBehaviour
{
    protected abstract void ClickEvent();

    protected void OnMouseDown()
    {
        ClickEvent();
    }
}
