using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTest : MonoBehaviour
{
    public GameObject go;
    public Color BOW;
    private Renderer PillarRenderer;
    // Start is called before the first frame update
    void Start()
    {
        PillarRenderer = go.GetComponent<Renderer>();
        PillarRenderer.material.SetColor("_Color", Color.black);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
