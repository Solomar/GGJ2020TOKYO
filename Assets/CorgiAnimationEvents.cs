using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorgiAnimationEvents : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] m_allClip;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayStep()
    {
        if(!animator.GetBool("Jumping"))
            SoundManager.Instance.PlaySound(m_allClip[Random.Range(0,m_allClip.Length)]);
    }
}
