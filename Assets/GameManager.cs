using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Very basic game manager lol
public class GameManager : MonoBehaviour
{
    public Sprite m_openedMouthSinger;
    public Sprite m_closedMouthSinger;

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


    [SerializeField]
    private CharacterTriggerZone m_windowArea;

    /// <summary>
    /// Singer sounds lists
    /// </summary>
    /// 
    [SerializeField]
    private AudioSource m_singerAudioSource;
    [SerializeField]
    private AudioSource m_cleaningAudioSource;
    [SerializeField]
    private AudioClip[] m_automatoneClips;
    [SerializeField]
    private AudioClip[] m_coughingClips;
    [SerializeField]
    private AudioClip[] m_badSingingClips;
    [SerializeField]
    private AudioClip[] m_badPitchClip;
    private float m_waitTimeBetweenSinging;

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
        m_waitTimeBetweenSinging = 3.0f;
        m_windowArea.AddObserverTriggerEnter(ZoomOut);
        m_windowArea.AddObserverTriggerExit(ZoomIn);
    }

    private void OnDestroy()
    {
        m_windowArea.RemoveObserverTriggerEnter(ZoomOut);
        m_windowArea.RemoveObserverTriggerExit(ZoomIn);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            m_zoomingOut = !m_zoomingOut;

        if (Input.GetMouseButtonUp(1))
            OpenSingerMouth();

        if (Input.GetMouseButtonUp(2))
            CloseSingerMouth();

        if (m_zoomingOut)
        {
            m_zoomProgress += Time.deltaTime / 2.0f;
            SingerCameraPan();
        }
        else if(m_zoomProgress > 0.0f)
        {
            m_zoomProgress = Mathf.Clamp01(m_zoomProgress - Time.deltaTime);
            SingerCameraPan();
        }
        else
        {
            Camera.main.transform.position = m_playerCharacterTransform.position + m_cameraZoomedVector;
        }

        m_waitTimeBetweenSinging -= Time.deltaTime;
        if(m_waitTimeBetweenSinging < 0.0f)
        {
            m_waitTimeBetweenSinging = Random.Range(3.0f, 5.0f);
            PlayBadSound();
        }
    }

    public void ZoomOut()
    {
        m_zoomingOut = true;
    }

    public void ZoomIn()
    {
        m_zoomingOut = false;
    }

    public void PlayBadSound()
    {
        switch(Random.Range(0,4))
        {
            case 0:
                m_singerAudioSource.clip = m_automatoneClips[Random.Range(0, m_automatoneClips.Length)];
                break;
            case 1:
                m_singerAudioSource.clip = m_coughingClips[Random.Range(0, m_coughingClips.Length)];
                break;
            case 2:
                m_singerAudioSource.clip = m_badPitchClip[Random.Range(0, m_badPitchClip.Length)];
                break;
            case 3:
                m_singerAudioSource.clip = m_badSingingClips[Random.Range(0, m_badSingingClips.Length)];
                break;
        }
        m_singerAudioSource.Play();
        StartCoroutine(SingNote(m_singerAudioSource.clip.length));
    }

    private IEnumerator SingNote(float waitTime)
    {
        OpenSingerMouth();
        yield return new WaitForSeconds(waitTime);
        CloseSingerMouth();
    }

    public void OpenSingerMouth()
    {
        m_singerFrontSpriteRenderer.sprite = m_openedMouthSinger;
        m_singerBackSpriteRenderer.sprite = m_openedMouthSinger;
    }

    public void CloseSingerMouth()
    {
        m_singerFrontSpriteRenderer.sprite = m_closedMouthSinger;
        m_singerBackSpriteRenderer.sprite = m_closedMouthSinger;
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

    private void SingerCameraPan()
    {
        m_singerFrontLayerAlphaValue = Mathf.SmoothStep(0.0f, 1.0f, Mathf.Clamp01(m_zoomProgress));
        m_singerFrontSpriteRenderer.color = new Color(1f, 1f, 1f, m_singerFrontLayerAlphaValue);
        m_singerBackSpriteRenderer.color = new Color(1f, 1f, 1f, m_singerFrontLayerAlphaValue);
        Camera.main.fieldOfView = Mathf.SmoothStep(m_zoomedInFieldOfView, m_zoomedOutFieldOfView, Mathf.Clamp01(m_zoomProgress));
    
        Camera.main.transform.position = Vector3.Lerp(m_playerCharacterTransform.position + m_cameraZoomedVector, m_cameraZoomedOutPosition.transform.position, Mathf.Clamp01(m_zoomProgress));

        m_cleaningAudioSource.volume = Mathf.SmoothStep(1.0f, 0.02f, m_zoomProgress);
    }

    private void Success()
    {
        Debug.Log("YOU DID IT!");
    }
}
