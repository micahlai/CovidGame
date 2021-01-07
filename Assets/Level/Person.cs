using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public enum State { Healthy, Infected, Immune, Vaccinated};
    public State state;

    [Range(6, 100)]
    public int age = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
