using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue : MonoBehaviour {

    [SerializeField] private int _maxQueueSize;
    [SerializeField] Transform _queueStartPosition;
    private Transform[] _queuePositions;
    private int _nextAvailableIndex;

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
        _nextAvailableIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public Transform[] Queuepositions
    {
        get { return _queuePositions; }
    }

    public int NextAvailableIndex
    {
        get { return _nextAvailableIndex; }
        set { _nextAvailableIndex = value; }
    }
}
