using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCanvas : MonoBehaviour
{
    private Animator _animator;
    private static readonly int FadeCanva = Animator.StringToHash("FadeCanva");

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetTrigger(FadeCanva);
    }

}
