using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Very basic game manager lol
public class GameManager : MonoBehaviour
{
    public float m_zoomedInFieldOfView;
    public float m_zoomedOutFieldOfView;

    private static GameManager m_instance;
    private List<HouseholdObjectLocation> m_allObjectLocationInScene = new List<HouseholdObjectLocation>();

    [SerializeField]
    private Transform m_cameraZoomedOutPosition;
    private Vector3 m_cameraZoomedVector = new Vector3(0f, 0f, -10.0f);
    private float m_singerFrontLayerAlphaValue;
    private float m_zoomProgress;
    private bool m_zoomingOut;
    [SerializeField]
    private Transform m_playerCharacterTransform;
    [SerializeField]
    private SpriteRenderer m_singerBackSpriteRenderer;
    [SerializeField]
    private SpriteRenderer m_singerFrontSpriteRenderer;

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
        m_zoomingOut = false;
        m_zoomProgress = 0.0f;
        ZoomOut();
    }

    private void Update()
    {
        if(m_zoomingOut)
        {
            m_zoomProgress += Time.deltaTime;

            // Alpha Value Change
            m_singerFrontLayerAlphaValue = Mathf.Lerp(0.0f, 1.0f, Mathf.Clamp01(m_zoomProgress - 0.1f));
            m_singerFrontSpriteRenderer.color = new Color(1f, 1f, 1f, m_singerFrontLayerAlphaValue);

            Camera.main.fieldOfView = Mathf.Lerp(m_zoomedInFieldOfView, m_zoomedOutFieldOfView, Mathf.Clamp01(m_zoomProgress - 0.1f));
            Camera.main.transform.position = Vector3.Lerp(m_playerCharacterTransform.position + m_cameraZoomedVector, m_cameraZoomedOutPosition.transform.position, Mathf.Clamp01(m_zoomProgress));
        }
        else if(m_zoomProgress > 0.0f)
        {

        }
    }

    public void ZoomOut()
    {
        m_zoomingOut = true;
        
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

    private void Success()
    {
        Debug.Log("YOU DID IT!");
    }
}
