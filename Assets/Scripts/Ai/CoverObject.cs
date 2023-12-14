using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverObject : MonoBehaviour
{
    private Collider _collider;
    
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        PlayerInputt.OnZoom += OnInputActive;
        PlayerInputt.OnUnZoom += OnInputInActive;
    }

    private void OnDestroy()
    {
        PlayerInputt.OnZoom -= OnInputActive;
        PlayerInputt.OnUnZoom -= OnInputInActive;
    }

    private void OnInputActive() => _collider.enabled = false;
    private void OnInputInActive() => _collider.enabled = true;

}
