using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObjectEventManager : MonoBehaviour
{
    public ObjectNames objectType;

    public void CharacterEnteredTrigger()
    {

        Debug.Log(this.GetType());
        EnterEffect();
    }

    virtual public void EnterEffect()
    {

    }
}
