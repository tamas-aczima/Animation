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
    private int customersInQueue = 0;
    private int nextAvailableChair = 0;

    //for ipad
    private float timer;

    private List<GameObject> customers = new List<GameObject>();

    private List<Seat> chairs = new List<Seat>();

	// Use this for initialization
	void Start () {
        queue = GetComponent<Queue>();
        chairs = FindObjectsOfType<Seat>().ToList();
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

        //increase timer
        timer += Time.deltaTime;
    }

    void SpawnCustomer()
    {
        GameObject customer = Instantiate(characters[Random.Range(0, characters.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)]);
        customer.name = customer.name.Replace("(Clone)", "");
        customers.Add(customer);
        customer.GetComponent<CustomerController>().TargetPosition = queue.QueuePositions[queue.Rear];
        customer.GetComponent<CustomerController>().QueuePosition = queue.Rear;
        queue.Rear++;
        numberOfCustomers++;
        customersInQueue++;
    }

    public void ManageQueue()
    {
        servedCustomers++;
        customersInQueue--;

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

    public float Timer
    {
        get { return timer; }
    }

    public int ServedCustomers
    {
        get { return servedCustomers; }
    }

    public int CustomersInQueue
    {
        get { return customersInQueue; }
    }

    public List<Seat> Chairs
    {
        get { return chairs; }
    }

    public int NextAvailableChair
    {
        get { return nextAvailableChair; }
        set { nextAvailableChair = value; }
    }
}
