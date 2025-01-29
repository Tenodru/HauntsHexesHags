using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public List<Camera> cameras;


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
    }

    public void DisableOtherCameras(Camera cameraToKeep)
    {
        foreach (Camera c in cameras)
        {
            if (c != cameraToKeep)
            {
                c.enabled = false;
            }
        }
    }
}
