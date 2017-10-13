using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour {
    
    private Transform _targetPosition;
    private Vector3 _moveDirection = Vector3.zero;
    private Animator _animator;
    [SerializeField] private float _speed = 2f;
    private int _queuePosition;
    private bool _isNextCustomer = false;
    private bool _startConversation = false;

    // Use this for initialization
    void Start () {
        _moveDirection = _targetPosition.position - transform.position;
        _animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        //rotate
        //float desiredAngle = _targetPosition.eulerAngles.y;
        //Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        //transform.Rotate(rotation * _speed * Time.deltaTime);

        //move
        _moveDirection.Normalize();

        if (Vector3.Distance(_targetPosition.position, transform.position) > 1)
        {
            transform.Translate(_moveDirection * _speed * Time.deltaTime);
        }
        else
        {
            _animator.SetBool("HasReachedDestination", true);
        }

        //manage queue
        if (_queuePosition == 0)
        {
            _isNextCustomer = true;
        }
    }

    public Transform TargetPosition
    {
        get { return _targetPosition; }
        set { _targetPosition = value; }
    }

    public int QueuePosition
    {
        get { return _queuePosition; }
        set { _queuePosition = value; }
    }
    
    public bool IsNextCustomer
    {
        get { return _isNextCustomer; }
        set { _isNextCustomer = value; }
    }

    public bool StartConversation
    {
        get { return _startConversation; }
        set { _startConversation = value; }
    }
}
