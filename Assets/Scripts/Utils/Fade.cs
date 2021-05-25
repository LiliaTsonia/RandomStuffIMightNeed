using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private Canvas _faderObj;
    [SerializeField] private Image _faderImg;
    [SerializeField] private Text _faderTxt;
    [SerializeField] private float _fadeSpeed = .02f;
    [SerializeField] private Color _fadeTransparency = new Color(0, 0, 0, .04f);

    public IEnumerator FadeOut(Action callback)
    {
        _faderObj.enabled = true;

        while (_faderImg.color.a < 1 && _faderTxt.color.a < 1)
        {
            _faderImg.color += _fadeTransparency;
            _faderTxt.color += _fadeTransparency;
            yield return new WaitForSeconds(_fadeSpeed);
        }

        callback?.Invoke();
        //ActivateScene(); //Activate the scene when the fade ends
    }

    public IEnumerator FadeIn()
    {
        while (_faderImg.color.a > 0 && _faderTxt.color.a > 0)
        {
            _faderImg.color -= _fadeTransparency;
            _faderTxt.color -= _fadeTransparency;
            yield return new WaitForSeconds(_fadeSpeed);
        }

        _faderObj.enabled = false;
    }
}

