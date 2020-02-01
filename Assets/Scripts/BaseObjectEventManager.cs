using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectEventManager : MonoBehaviour
{
    public ObjectNames objectType;

    public virtual void CharacterEnteredTrigger()
    {
        Debug.Log("Base Object Event Trigger");
    }
}
