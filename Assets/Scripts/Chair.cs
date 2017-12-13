using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chair : MonoBehaviour {

    private Transform target;
    private bool isOccupied = false;

	// Use this for initialization
	void Start () {
        target = transform.Find("Target").transform;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public Transform Target
    {
        get { return target; }
    }

    public bool IsOccupied
    {
        get { return isOccupied; }
        set { isOccupied = value; }
    }
}
