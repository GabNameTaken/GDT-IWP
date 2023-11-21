using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    Camera mainCamera;
    public static CameraManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        mainCamera = Camera.main;
    }

    

    [SerializeField] List<CameraTransform> transformList;
    private Dictionary<CAMERA_POSITIONS, CameraTransformData> cameraDict;
    public Dictionary<CAMERA_POSITIONS, CameraTransformData> CameraDict { get { return cameraDict ?? (cameraDict = transformList.ToDictionary(camera => camera.pos, camera => camera.data)); } }

    public void MoveCamera(GameObject entityObject, CAMERA_POSITIONS pos, float speed)
    {
        mainCamera.transform.SetParent(entityObject.transform);
        CameraTransformData transformData = CameraDict[pos];
        mainCamera.transform.DOLocalMove(transformData.position, speed, false);
        mainCamera.transform.DOLocalRotateQuaternion(transformData.rotation, speed);
        mainCamera.DOFieldOfView(transformData.fovAngle, speed);
    }
}
