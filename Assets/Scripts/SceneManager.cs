using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour {

    [SerializeField] private GameObject[] _charecters;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Queue _queue;
    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;
    private float _spawnTimer = 0f;
    private float _spawnTime;
    [SerializeField] private int _maxCustomers = 5;
    private int _numberOfCustomers = 0;

    private List<GameObject> _customers = new List<GameObject>();

	// Use this for initialization
	void Start () {
        _queue = GetComponent<Queue>();
        _spawnTime = GetSpawnTime();
    }
	
	// Update is called once per frame
	void Update () {
        if (_numberOfCustomers >= _maxCustomers) return; 

        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnTime)
        {
            SpawnCustomer();
            _spawnTimer = 0f;
            _spawnTime = GetSpawnTime();
        }

        //manage queue
        foreach (GameObject customer in _customers)
        {
            //if (customer.)
        }
    }

    void SpawnCustomer()
    {
        GameObject customer = Instantiate(_charecters[Random.Range(0, _spawnPoints.Length)], _spawnPoints[Random.Range(0, _spawnPoints.Length)]);
        _customers.Add(customer);
        customer.GetComponent<CustomerController>().TargetPosition = _queue.Queuepositions[_queue.NextAvailableIndex];
        customer.GetComponent<CustomerController>().QueuePosition = _queue.NextAvailableIndex;
        _queue.NextAvailableIndex++;
        _numberOfCustomers++;
    }

    float GetSpawnTime()
    {
        return Random.Range(_minSpawnTime, _maxSpawnTime);
    }
}
