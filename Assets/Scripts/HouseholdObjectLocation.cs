using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseholdObjectLocation : MonoBehaviour
{
    public ObjectNames assignedObject;

    private bool hasAssignedObject;
    public bool HasAssignedObject { get { return hasAssignedObject; } }

    private void Awake()
    {
        hasAssignedObject = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GetComponent<Collider2D>()
        if (collision.CompareTag("ObjectiveObject"))
        {
            if (collision.gameObject.GetComponent<BaseObjectEventManager>().objectType == assignedObject)
            {
                hasAssignedObject = true;
                GameManager.Instance.ObjectPlacedCorrectly();
            }
        }
    }
}
