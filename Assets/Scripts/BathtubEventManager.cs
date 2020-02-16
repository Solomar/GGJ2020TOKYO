using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathtubEventManager : BaseObjectEventManager
{
    [SerializeField]
    private Animator m_bubbleAnimation;

    public override void EnterEffect()
    {
        Debug.Log("test");
        m_bubbleAnimation.SetTrigger("PupperEnter");
    }
}
