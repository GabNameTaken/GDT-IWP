using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.DesignPatterns;
using TMPro;

public class PopupMessage : Singleton<PopupMessage>
{
    [SerializeField] GameObject _popup;
    [SerializeField] TextMeshProUGUI _msg;
    [SerializeField] float _defaultTime;

    Coroutine _ongoingCoroutine = null;

    public void PopMessage(string text)
    {
        _msg.text = text;

        _popup.SetActive(true);

        if (_ongoingCoroutine != null) 
            StopCoroutine(_ongoingCoroutine);
        _ongoingCoroutine = StartCoroutine(SetInactiveCoroutine(_defaultTime));
    }

    public void PopMessage(string text, float time)
    {
        _msg.text = text;

        _popup.SetActive(true);

        if (_ongoingCoroutine != null) 
            StopCoroutine(_ongoingCoroutine);
        _ongoingCoroutine = StartCoroutine(SetInactiveCoroutine(time));
    }

    IEnumerator SetInactiveCoroutine(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        _popup.SetActive(false);
    }
}
