using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour {

    [SerializeField] private int _maxQueueSize;
    [SerializeField] Transform _queueStartPosition;
    private Transform[] _queuePositions;
    private int _front = 0;
    private int _rear = 0;

	// Use this for initialization
	void Start () {
        _queuePositions = new Transform[_maxQueueSize];
        _queuePositions[0] = _queueStartPosition;
        for (int i = 1; i < _queuePositions.Length; i++)
        {
            GameObject nextQueueObject = new GameObject("QueuePosition" + i);
            _queuePositions[i] = nextQueueObject.transform;
            _queuePositions[i].position = _queuePositions[i - 1].transform.position - new Vector3(0, 0, 3);
        }
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public Transform[] QueuePositions
    {
        get { return _queuePositions; }
    }

    public int Front
    {
        get { return _front; }
        set { _front = value; }
    }

    public int Rear
    {
        get { return _rear; }
        set { _rear = value; }
    }
}
