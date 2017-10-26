using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneManager : MonoBehaviour {

    [SerializeField] private GameObject[] characters;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject waitingArea;
    [SerializeField] private Queue queue;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    private float spawnTimer = 0f;
    private float spawnTime;
    [SerializeField] private static int maxCustomers = 8;
    private int numberOfCustomers = 0;
    private int servedCustomers = 0;

    private List<GameObject> customers = new List<GameObject>();

	// Use this for initialization
	void Start () {
        queue = GetComponent<Queue>();
        spawnTime = GetSpawnTime();
    }
	
	// Update is called once per frame
	void Update () {
        //spawn customers
        if (numberOfCustomers < maxCustomers)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnTime)
            {
                SpawnCustomer();
                spawnTimer = 0f;
                spawnTime = GetSpawnTime();
            }
        }
    }

    void SpawnCustomer()
    {
        GameObject customer = Instantiate(characters[Random.Range(0, characters.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)]);
        customers.Add(customer);
        customer.GetComponent<CustomerController>().TargetPosition = queue.QueuePositions[queue.Rear];
        customer.GetComponent<CustomerController>().QueuePosition = queue.Rear;
        queue.Rear++;
        numberOfCustomers++;
    }

    public void ManageQueue()
    {
        servedCustomers++;

        queue.Rear--;
        queue.Front = 0;
        
        customers.RemoveAt(0);
        for (int i = 0; i < customers.Count; i++)
        {
            CustomerController customer = customers.ElementAt(i).GetComponent<CustomerController>();
            customer.TargetPosition = queue.QueuePositions[queue.Front];
            customer.QueuePosition = queue.Front;
            customer.NewTarget();
            queue.Front++;
        }
    }

    float GetSpawnTime()
    {
        return Random.Range(minSpawnTime, maxSpawnTime);
    }
}
