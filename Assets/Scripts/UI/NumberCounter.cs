using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberCounter : MonoBehaviour
{
    [SerializeField] TMP_Text numberText;

    //private int _value;
    //public int value
    //{
    //    get { return _value; }
    //    set { 
    //        StartCount(value); 
    //        _value = value; 
    //    }
    //}
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    float duration = 3f;
    float countDuration = 0.4f;
    float countFPS = 30f;
    Coroutine countingCoroutine;
    public void StartCount(int newValue, Color color)
    {
        if (countingCoroutine != null)
            StopCoroutine(countingCoroutine);

        numberText.color = color;

        gameObject.SetActive(true);
        countingCoroutine = StartCoroutine(Count(newValue));
    }

    IEnumerator Count(int newValue)
    {
        WaitForSeconds wait = new WaitForSeconds(1f / countFPS);
        int previousValue = 0;
        int countValue;

        if (newValue - previousValue < 0)
            countValue = Mathf.FloorToInt((newValue - previousValue) / (countFPS * countDuration));
        else
            countValue = Mathf.CeilToInt((newValue - previousValue) / (countFPS * countDuration));

        if (previousValue < newValue)
        {
            while (previousValue < newValue)
            {
                previousValue += countValue;
                if (previousValue > newValue)
                    previousValue = newValue;
                numberText.SetText(previousValue.ToString());

                yield return wait;
            }
        }
        else
        {
            while (previousValue > newValue)
            {
                previousValue -= countValue;
                if (previousValue < newValue)
                    previousValue = newValue;
                numberText.SetText(previousValue.ToString());

                yield return wait;
            }
        }

        yield return new WaitForSeconds(duration);

        gameObject.SetActive(false);
    }
}
