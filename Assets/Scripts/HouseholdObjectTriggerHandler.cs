using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseholdObjectTriggerHandler : MonoBehaviour
{
    private BaseObjectEventManager baseObjectEventManager;

    // Delegates
    public delegate void OnCharacterTrigger();
    public event OnCharacterTrigger onCharacterTrigger = null;
    public void AddObserver(OnCharacterTrigger methode) { onCharacterTrigger = new OnCharacterTrigger(methode); }
    public void RemoveObserver(OnCharacterTrigger methode) { onCharacterTrigger -= methode; }

    void Start()
    {
        baseObjectEventManager = GetComponentInParent<BaseObjectEventManager>();
        AddObserver(baseObjectEventManager.CharacterEnteredTrigger);
    }

    private void OnDestroy()
    {
        RemoveObserver(baseObjectEventManager.CharacterEnteredTrigger);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        //GetComponent<Collider2D>()
        if (collision.CompareTag("Character"))
        {
            Debug.Log("teststisfa");
            onCharacterTrigger();
        }
    }
}
