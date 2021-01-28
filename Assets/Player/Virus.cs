using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Virus : MonoBehaviour
{
    public NavMeshAgent agent;
    public bool controlled;
    public float stopDistance = 5;
    public float vision = 10;
    public Person target;
    public bool showCircles = false;
    public static bool MoveAttack = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controlled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance))
        {
            controlled = false;
        }

        
        foreach (Person p in FindObjectsOfType<Person>())
        {
            if (Vector3.Distance(p.transform.position, transform.position) < vision && InfectionCondition(p))
            {
                Debug.DrawLine(transform.position, p.transform.position, Color.yellow);

            }

        }
        FindNearest();
        if (target != null)
        {
            Debug.DrawLine(transform.position, target.transform.position, Color.green);
            if (!(Vector3.Distance(target.transform.position, transform.position) < vision && InfectionCondition(target)))
            {
                target = null;

            }
        }
        if (!controlled || MoveAttack) {

            
            if (target != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < vision)
                {
                    NavMeshPath path = new NavMeshPath();
                    agent.CalculatePath(target.transform.position, path);

                    agent.SetPath(path);
                    Debug.DrawLine(transform.position, target.transform.position, Color.blue);
                }
                else
                {
                    target = null;
                }
            }


        }
        /*
        if (target != null)
        {
            
            NavMeshHit hits;
            if (NavMesh.SamplePosition(transform.position, out hits, 100, NavMesh.AllAreas))
            {
                transform.position = hits.position;
            }



        }*/
        NavMeshHit hit;
        if (NavMesh.FindClosestEdge(transform.position, out hit, NavMesh.AllAreas))
        {
            if (showCircles)
            {
                DrawCircle(transform.position, hit.distance, Color.red);
                Debug.DrawRay(hit.position, Vector3.up, Color.red);
            }
        }
    }
    void FindNearest()
    {
        foreach (Person p in FindObjectsOfType<Person>())
        {
            if (Vector3.Distance(p.transform.position, transform.position) < vision && InfectionCondition(p))
            {


                if (target == null)
                {
                    target = p;


                }
                else if ((Vector3.Distance(p.transform.position, transform.position) < Vector3.Distance(target.transform.position, transform.position)))
                {
                    target = p;
                }

            }

        }
    }
    bool InfectionCondition(Person p)
    {
        if(p.state == Person.State.Healthy)
        {
            return true;
        }
        else if (p.state == Person.State.Incubated)
        {
            return false;
        }
        else if (p.state == Person.State.Infected)
        {
            return false;
        }
        else if (p.state == Person.State.Vaccinated)
        {
            return true;
        }
        else if (p.state == Person.State.Immune)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    void DrawCircle(Vector3 center, float radius, Color color)
    {
        Vector3 prevPos = center + new Vector3(radius, 0, 0);
        for (int i = 0; i < 30; i++)
        {
            float angle = (float)(i + 1) / 30.0f * Mathf.PI * 2.0f;
            Vector3 newPos = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Debug.DrawLine(prevPos, newPos, color);
            prevPos = newPos;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Person>() != null)
        {
            if (other.GetComponent<Person>().state == Person.State.Healthy)
            {
                other.GetComponent<Person>().Infect();

                FindObjectOfType<selected_dictionary>().deselectGO(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
