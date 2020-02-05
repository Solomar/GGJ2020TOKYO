using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTriggerZone : MonoBehaviour
{
    public delegate void OnCharacterTrigger();
    public event OnCharacterTrigger onCharacterTriggerEnter = null;
    public void AddObserverTriggerEnter(OnCharacterTrigger methode) { onCharacterTriggerEnter = new OnCharacterTrigger(methode); }
    public void RemoveObserverTriggerEnter(OnCharacterTrigger methode) { onCharacterTriggerEnter -= methode; }

    public event OnCharacterTrigger onCharacterTriggerExit = null;
    public void AddObserverTriggerExit(OnCharacterTrigger methode) { onCharacterTriggerExit = new OnCharacterTrigger(methode); }
    public void RemoveObserverTriggerExit(OnCharacterTrigger methode) { onCharacterTriggerExit -= methode; }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            onCharacterTriggerEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            onCharacterTriggerExit();
        }
    }
}

