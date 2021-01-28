using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Person : MonoBehaviour
{
    public enum State { Healthy, Incubated, Infected, Immune, Vaccinated};
    public State state;
    [Range(0, 100)]
    public float chanceOfIncubation = 70;
    public Vector2 incubationPeriodRange;
    public float incubationPeriod;
    public float spawnRange = 10.0f;
    public float virusSpawnFreq = 4;
    public float recoveryTime = 10;
    [Range(0,100)]
    public float deathChance = 50;
    public Vector2 immunityWearoffTimeChance;
    public float immunityWearoffTime;
    float timer;
    float timer2;
    

    public GameObject virus;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Healthy;
    }

    // Update is called once per frame
    void Update()
    {
        Renderer r = GetComponent<Renderer>();
        if(state == State.Healthy)
        {
            r.material.color = Color.green;
        }
        else if(state == State.Incubated)
        {
            r.material.color = Color.yellow;
            timer += Time.deltaTime;
            if(timer >= incubationPeriod)
            {
                timer = 0;
                timer2 = 0;
                state = State.Infected;
            }
        }
        else if (state == State.Infected)
        {
            timer += Time.deltaTime;
            timer2 += Time.deltaTime;
            if (timer > virusSpawnFreq)
            {
                timer = 0;
                SpawnVirus();
            }
            if(timer2 > recoveryTime)
            {
                timer = 0;
                timer2 = 0;
                if(Random.Range(0, 100) < deathChance)
                {
                    Destroy(gameObject);
                }
                else
                {
                    state = State.Immune;
                    immunityWearoffTime = Random.Range(immunityWearoffTimeChance.x, immunityWearoffTimeChance.y);
                }
            }
            r.material.color = Color.red;
        }
        else if (state == State.Immune)
        {
            r.material.color = Color.gray;
            timer += Time.deltaTime;
            if(timer > immunityWearoffTime)
            {
                timer = 0;
                state = State.Healthy;
            }
            
        }
        else if (state == State.Vaccinated)
        {
            r.material.color = Color.blue;
        }
        
    }
    
    void SpawnVirus()
    {
        Vector3 point;
        if (RandomPoint(transform.position, spawnRange, out point))
        {
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
            point.y = 0.53f;
            Instantiate(virus, point, Quaternion.identity);
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
    
    
    public void Infect()
    {
        if (Random.Range(0, 100) <= chanceOfIncubation)
        {
            if (state == State.Healthy)
            {
                state = State.Incubated;
                incubationPeriod = Random.Range(incubationPeriodRange.x, incubationPeriodRange.y);
            }
        }
    }
}
