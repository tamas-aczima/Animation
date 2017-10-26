using UnityEngine;

public class Queue : MonoBehaviour {

    [SerializeField] private int maxQueueSize;
    [SerializeField] Transform queueStartPosition;
    private Transform[] queuePositions;
    private int front = 0;
    private int rear = 0;

	// Use this for initialization
	void Start () {
        queuePositions = new Transform[maxQueueSize];
        queuePositions[0] = queueStartPosition;
        for (int i = 1; i < queuePositions.Length; i++)
        {
            GameObject nextQueueObject = new GameObject("QueuePosition" + i);
            queuePositions[i] = nextQueueObject.transform;
            queuePositions[i].position = queuePositions[i - 1].transform.position - new Vector3(0, 0, 3);
        }
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public Transform[] QueuePositions
    {
        get { return queuePositions; }
    }

    public int Front
    {
        get { return front; }
        set { front = value; }
    }

    public int Rear
    {
        get { return rear; }
        set { rear = value; }
    }
}
