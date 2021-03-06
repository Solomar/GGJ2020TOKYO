﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
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
    private List<Light2D> m_allLight2DInScene = new List<Light2D>();

    [SerializeField]
    private Transform m_cameraZoomedOutPosition;
    private Vector3 m_cameraZoomedVector = new Vector3(0f, 0f, -10.0f);
    private float m_singerFrontLayerAlphaValue;
    private float m_zoomProgress;
    private bool m_zoomingOut;
    private bool m_win;
    [SerializeField]
    private Transform m_playerCharacterTransform;
    [SerializeField]
    private SpriteRenderer m_singerBackSpriteRenderer;
    [SerializeField]
    private SpriteRenderer m_singerFrontSpriteRenderer;
    [SerializeField]
    private CharacterTriggerZone m_windowArea;
    private int m_updatedProgress;
    private int m_objectPlacementCount;

    /// <summary>
    /// Singer sounds lists
    /// </summary>
    /// 
    [SerializeField]
    private AudioSource m_singerAudioSource;
    [SerializeField]
    private AudioSource m_winningAudio;
    [SerializeField]
    private AudioSource m_cleaningAudioSource;
    [SerializeField]
    private AudioClip[] m_badSinging;
    [SerializeField]
    private AudioClip[] m_goodSinging;
    [SerializeField]
    private AudioClip m_winClip;
    private float m_waitTimeBetweenSinging;
    [SerializeField]
    private AudioLoudnessTester m_audioLoudness;

    /// <summary>
    /// UI Related Stuff
    /// </summary>

    [SerializeField]
    private Animator credits;
    [SerializeField]
    private Animator thanks;
    [SerializeField]
    private Animator screenFade;
    [SerializeField]
    private TMPro.TextMeshProUGUI instructions;
    
    [SerializeField]
    private TMPro.TextMeshProUGUI m_progressIndicator;

    public static GameManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject gameManager = new GameObject("GameManager");
                DontDestroyOnLoad(gameManager);
                GameManager gameManagerComponent = gameManager.AddComponent<GameManager>();
                m_instance = gameManagerComponent;
            }
            return m_instance;
        }
    }

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_instance = this;
        }
    }

    void Start()
    {
        m_allObjectLocationInScene = FindObjectsOfType<HouseholdObjectLocation>().ToList();
        m_allLight2DInScene = FindObjectsOfType<Light2D>().ToList();
        m_zoomingOut = false;
        m_zoomProgress = 0.0f;
        m_waitTimeBetweenSinging = 3.0f;
        m_windowArea.AddObserverTriggerEnter(ZoomOut);
        m_windowArea.AddObserverTriggerExit(ZoomIn);
        m_objectPlacementCount = m_allObjectLocationInScene.Count;
        m_progressIndicator.text = " 0/" + m_objectPlacementCount;
        m_updatedProgress = 0;

        StartCoroutine(IntroCoroutine());
    }

    private void OnDestroy()
    {
        m_windowArea.RemoveObserverTriggerEnter(ZoomOut);
        m_windowArea.RemoveObserverTriggerExit(ZoomIn);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Backspace))
            Win();

        if (m_zoomingOut || m_win)
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

        if (!m_win)
        {
            m_waitTimeBetweenSinging -= Time.deltaTime;
            if (m_waitTimeBetweenSinging < 0.0f)
            {
                if (Random.Range(0, m_objectPlacementCount) > m_updatedProgress)
                    PlayBadSound();
                else
                    PlayGoodSound();
            }
        }
    }


    public void Win()
    {
        m_win = true;
        ZoomOut();
    }

    public void ZoomOut()
    {
        m_zoomingOut = true;
        foreach (Light2D light in m_allLight2DInScene)
        {
            if (light.lightType != Light2D.LightType.Global)
                light.enabled = false;
        }
    }

    public void ZoomIn()
    {
        m_zoomingOut = false;
        foreach (Light2D light in m_allLight2DInScene)
        {
            if(light.lightType != Light2D.LightType.Global)
                light.enabled = true;
        }
    }

    public void PlayBadSound()
    {
        m_singerAudioSource.clip = m_badSinging[Random.Range(0, m_badSinging.Length)];
        m_waitTimeBetweenSinging = m_singerAudioSource.clip.length + Random.Range(1.0f, 3.0f);
        m_singerAudioSource.Play();
        StartCoroutine(SingNote(m_singerAudioSource.clip.length));
    }

    public void PlayGoodSound()
    {
        m_singerAudioSource.clip = m_goodSinging[Random.Range(0, m_goodSinging.Length)];
        m_waitTimeBetweenSinging = m_singerAudioSource.clip.length + Random.Range(1.0f, 3.0f);
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

    public void ObjectPlacementUpdate()
    {
        int correctlyPlacedObjects = 0;
        foreach(HouseholdObjectLocation objectLocation in m_allObjectLocationInScene)
        {
            if (objectLocation.HasAssignedObject)
                correctlyPlacedObjects++;
        }
        m_updatedProgress = correctlyPlacedObjects;
        Debug.Log(correctlyPlacedObjects + " of " + m_allObjectLocationInScene.Count);
        m_progressIndicator.text = correctlyPlacedObjects + "/" + m_allObjectLocationInScene.Count;
        if (correctlyPlacedObjects == m_allObjectLocationInScene.Count)
            Success();
    }

    bool doOnce = true;
    private void SingerCameraPan()
    {
        m_singerFrontLayerAlphaValue = Mathf.SmoothStep(0.0f, 1.0f, Mathf.Clamp01(m_zoomProgress));
        m_singerFrontSpriteRenderer.color = new Color(1f, 1f, 1f, m_singerFrontLayerAlphaValue);
        m_singerBackSpriteRenderer.color = new Color(1f, 1f, 1f, m_singerFrontLayerAlphaValue);
        m_progressIndicator.color = new Color(1f, 1f, 1f, 1.0f - m_singerFrontLayerAlphaValue);
        Camera.main.fieldOfView = Mathf.SmoothStep(m_zoomedInFieldOfView, m_zoomedOutFieldOfView, Mathf.Clamp01(m_zoomProgress));
    
        Camera.main.transform.position = Vector3.Lerp(m_playerCharacterTransform.position + m_cameraZoomedVector, m_cameraZoomedOutPosition.transform.position, Mathf.Clamp01(m_zoomProgress));

        if (!m_win)
            m_cleaningAudioSource.volume = Mathf.SmoothStep(1.0f, 0.02f, m_zoomProgress);
        else if(doOnce)
        {
            doOnce = false;
            StartCoroutine(EndingCoroutine());
        }
    }

    private void Success()
    {
        Debug.Log("YOU DID IT!");
        Win();
    }

    private IEnumerator IntroCoroutine()
    {
        yield return new WaitForSeconds(4.0f);

        float fade = 1.0f;
        while (fade != 0)
        {
            fade -= 0.4f * Time.deltaTime;
            instructions.color = new Color(1f, 1f, 1f, fade);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator EndingCoroutine()
    {
        m_cleaningAudioSource.Stop();
        m_winningAudio.Play();
        yield return new WaitForSeconds(7.0f);
        OpenSingerMouth();
        credits.SetTrigger("Appear");
        yield return new WaitForSeconds(64.0f);
        thanks.SetTrigger("Appear");
        CloseSingerMouth();
        while (m_winningAudio.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        screenFade.SetTrigger("GameStart");
        yield return new WaitForSeconds(4.0f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
