using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    public void DisableMainCamera()
    {
        mainCamera.gameObject.SetActive(false);
    }

    internal void EnableMainCamera()
    {
        mainCamera.gameObject.SetActive(true);
    }
}