using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour {

    [SerializeField] private GameObject[] _charecters;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private GameObject _waitingArea;
    [SerializeField] private Queue _queue;
    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;
    private float _spawnTimer = 0f;
    private float _spawnTime;
    [SerializeField] private static int _maxCustomers = 8;
    private int _numberOfCustomers = 0;
    private int _servedCustomers = 0;

    private List<GameObject> _customers = new List<GameObject>();

	// Use this for initialization
	void Start () {
        _queue = GetComponent<Queue>();
        _spawnTime = GetSpawnTime();
    }
	
	// Update is called once per frame
	void Update () {
        //spawn customers
        if (_numberOfCustomers < _maxCustomers)
        {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _spawnTime)
            {
                SpawnCustomer();
                _spawnTimer = 0f;
                _spawnTime = GetSpawnTime();
            }
        }
    }

    void SpawnCustomer()
    {
        GameObject customer = Instantiate(_charecters[Random.Range(0, _spawnPoints.Length)], _spawnPoints[Random.Range(0, _spawnPoints.Length)]);
        _customers.Add(customer);
        customer.GetComponent<CustomerController>().TargetPosition = _queue.QueuePositions[_queue.Rear];
        customer.GetComponent<CustomerController>().QueuePosition = _queue.Rear;
        _queue.Rear++;
        _numberOfCustomers++;
    }

    public void ManageQueue()
    {
        _servedCustomers++;

        _queue.Rear--;
        _queue.Front = 0;
        
        _customers.RemoveAt(0);
        for (int i = 0; i < _customers.Count; i++)
        {
            CustomerController customer = _customers.ElementAt(i).GetComponent<CustomerController>();
            customer.TargetPosition = _queue.QueuePositions[_queue.Front];
            customer.QueuePosition = _queue.Front;
            customer.NewTarget();
            _queue.Front++;
        }
    }

    float GetSpawnTime()
    {
        return Random.Range(_minSpawnTime, _maxSpawnTime);
    }
}
