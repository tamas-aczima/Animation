using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour {

    private Transform target;

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
}
