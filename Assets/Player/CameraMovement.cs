using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraMovement : MonoBehaviour
{
    selected_dictionary dict;

    // Start is called before the first frame update
    void Start()
    {
        dict = FindObjectOfType<selected_dictionary>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                foreach(var entry in dict.selectedTable)
                {
                    GameObject g = entry.Value;
                    if (g.GetComponent<Virus>() != null)
                    {
                        NavMeshAgent a = g.GetComponent<NavMeshAgent>();
                        NavMeshPath path = new NavMeshPath();
                        a.CalculatePath(hit.point, path);
                        if (path.status == NavMeshPathStatus.PathPartial)
                        {

                        }
                        else
                        {
                            //a.SetPath(path);
                            a.SetDestination(hit.point);
                        }
                    }
                }
            }
        }
    }
}
