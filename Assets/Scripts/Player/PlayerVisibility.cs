using System;
using System.Collections;
using System.Collections.Generic;
using CoverShooter;
using UnityEngine;

public class PlayerVisibility : MonoBehaviour
{
    private Renderer[] _meshRenderers;
    
    private void Awake()
    {
        _meshRenderers = GetComponentsInChildren<Renderer>();
        
        PlayerInputt.OnZoom += OnZoom;
        PlayerInputt.OnUnZoom += OnUnZoom;
    }

    private void OnZoom()
    {
        foreach (var v in _meshRenderers)
        {
            if(v)
                v.enabled = false;
        }
    }

    private void OnUnZoom()
    {
        foreach (var v in _meshRenderers)
        {
            if(v)
                v.enabled = true;
        }
    }
}
