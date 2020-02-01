using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Very basic game manager lol
public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    private List<HouseholdObjectLocation> m_allObjectLocationInScene = new List<HouseholdObjectLocation>();

    public static GameManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject gameManager = new GameObject("GameManager");
                GameManager gm = gameManager.AddComponent<GameManager>();
                m_instance = gm;
            }
            return m_instance;
        }
    }

    void Start()
    {
        m_allObjectLocationInScene = FindObjectsOfType<HouseholdObjectLocation>().ToList();
    }

    public void ObjectPlacedCorrectly()
    {
        foreach(HouseholdObjectLocation objectLocation in m_allObjectLocationInScene)
        {
            if (!objectLocation.HasAssignedObject)
                break;
        }
        Success();
    }

    void Success()
    {
        Debug.Log("YOU DID IT!");
    }
}
