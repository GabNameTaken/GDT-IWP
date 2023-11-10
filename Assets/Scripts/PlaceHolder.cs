using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolder : MonoBehaviour
{
    private void Awake()
    {
        transform.SetParent(null);
        Destroy(gameObject);
    }
}
