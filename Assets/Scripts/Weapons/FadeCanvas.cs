using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCanvas : MonoBehaviour
{
    private Animator _animator;
    
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = true;
    }

}
