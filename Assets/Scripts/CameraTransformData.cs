using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CAMERA_POSITIONS
{
    HIGH_BACK,
    LOW_BACK,
    LOW_FRONT_SELF,
    HIGH_FRONT_SELF,
    PLAYER_TEAM_BACK,
    PLAYER_TEAM_FRONT,
    NONE,
}

[CreateAssetMenu(menuName = "Camera/Transform")]
public class CameraTransformData : ScriptableObject
{
    [SerializeField] Vector3 _position;
    public Vector3 position => _position;

    [SerializeField] Quaternion _rotation;
    public Quaternion rotation => _rotation;

    [SerializeField] float _fovAngle = 90f;
    public float fovAngle => _fovAngle;
}

[System.Serializable]
public class CameraTransform
{
    public CameraTransformData data;
    public CAMERA_POSITIONS pos;
}
