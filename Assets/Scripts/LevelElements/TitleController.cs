using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    private bool gameStartCalled;
    [SerializeField]
    private Animator fadeControl;
    [SerializeField]
    private AudioClip startGameSound;

    private void Start()
    {
        gameStartCalled = false;
    }

    void Update()
    {
        if (Input.anyKey && !gameStartCalled)
        {
            gameStartCalled = true;
            fadeControl.SetTrigger("GameStart");
            SoundManager.Instance.PlaySound(startGameSound);
        }
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
