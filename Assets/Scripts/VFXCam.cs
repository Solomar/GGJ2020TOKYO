using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VFXCam : MonoBehaviour
{
    [SerializeField]
    Material pixelMat;
    Camera cam;

    //private void Start()
    //{
    //    // get this camera comp
    //    cam = gameObject.GetComponent<Camera>();
    //}

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, pixelMat);
        Debug.Log("Called");
    }
}
