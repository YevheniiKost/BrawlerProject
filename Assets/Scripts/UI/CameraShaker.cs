using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraShaker : MonoBehaviour
{

    private CinemachineVirtualCamera _camera;
    private float _shakeTimer;

    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        EventAggregator.Subscribe<ShakeCamera>(OnShakeCameraHandler);
    }

    private void OnDestroy()
    {
        EventAggregator.Unsubscribe<ShakeCamera>(OnShakeCameraHandler);
    }

    private void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin noise =
            _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                noise.m_AmplitudeGain = 0;

            }
        }
    }

    private void OnShakeCameraHandler(object arg1, ShakeCamera data)
    {
        ShakeCamera(data.Intencity, data.Time);
    }

    private void ShakeCamera(float intencity, float time)
    {
        CinemachineBasicMultiChannelPerlin noise =
            _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        noise.m_AmplitudeGain = intencity;
        _shakeTimer = time;

    }
}
