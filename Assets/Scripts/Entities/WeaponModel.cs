using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModel : MonoBehaviour
{
    [SerializeField] Transform attachedParent;
    [SerializeField] Vector3 attachPos;
    [SerializeField] Vector3 attachRotation;

    private Transform restParent;
    private Vector3 restPos;
    private Vector3 restRotation;

    private void Awake()
    {
        restParent = transform.parent;
        restPos = transform.localPosition;
        restRotation = transform.localEulerAngles;
    }

    public void AttachWeapon()
    {
        transform.SetParent(attachedParent);
        transform.localPosition = attachPos;
        transform.localEulerAngles = attachRotation;
    }

    public void RestWeapon()
    {
        transform.SetParent(restParent);
        transform.localPosition = restPos;
        transform.localEulerAngles = restRotation;
    }
}
