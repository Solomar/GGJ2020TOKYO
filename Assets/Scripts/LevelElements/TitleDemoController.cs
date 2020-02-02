using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleDemoController : MonoBehaviour
{
    private float t0 = 0;
    // Start is called before the first frame update
    void Start()
    {
        t0 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        t0 += 1F / 60F;
        if (true)
        {
            //GetComponent<UnityEngine.UI.RawImage>().color =
            //    new Color(0F, 0F, 0F, Mathf.Min(t0-6F, 1F));
            if (Input.GetButton("Fire1"))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Scenes/Main Game Scene");
                Destroy(this);
            }
            return;
        }
        //GetComponent<UnityEngine.UI.RawImage>().color =
        //    new Color(0F, 0F, 0F, Mathf.Max(1F - t0, 0F));
    }
}
