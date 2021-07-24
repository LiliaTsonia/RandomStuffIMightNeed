using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonView : MonoBehaviour, IView
{
    public virtual void Init(params object[] args)
    {
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
