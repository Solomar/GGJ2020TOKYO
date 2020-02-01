using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundTestingStuffScript : MonoBehaviour
{
    public AudioClip OnClickAudioClip;
    public AudioMixerGroup audioMixerGroup;

    // Delegates
    public delegate void OnClick();
    public event OnClick onClick = null;
    public void AddObserverPress(OnClick methode) { onClick = new OnClick(methode); }
    public void RemoveObserverPress(OnClick methode) { onClick -= methode; }
 
    void Start()
    {
        AddObserverPress(PlayAudioClip);
    }

    private void OnDestroy()
    {
        RemoveObserverPress(PlayAudioClip);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null && hit.transform == this.transform)
            {
                onClick();
            }
        }
    }

    private void PlayAudioClip()
    {
        SoundManager.Instance.PlaySound(OnClickAudioClip, 1.0f, audioMixerGroup);
    }
}
