using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selection_component : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Virus>() != null)
        {
            GetComponent<Renderer>().material.SetFloat("Intensity", 1);
        }
    }

    private void OnDestroy()
    {
        if (GetComponent<Virus>() != null)
        {
            GetComponent<Renderer>().material.SetFloat("Intensity", 0);
        }
    }
}
