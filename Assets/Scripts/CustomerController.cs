using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour {
    
    private Transform _targetPosition;
    private Vector3 _moveDirection = Vector3.zero;
    private Animator _animator;
    private Rigidbody _rigidbody;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _rotateSpeed = 100f;
    private int _queuePosition;
    private bool _isNextCustomer = false;
    private bool _startConversation = false;

    // Use this for initialization
    void Start () {
        _moveDirection = _targetPosition.position - transform.position;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {   

        //manage queue
        if (_queuePosition == 0)
        {
            _isNextCustomer = true;
        }
    }

    void FixedUpdate()
    {
        _moveDirection.y = 0;
        _moveDirection.Normalize();

        //move
        if (Vector3.Distance(_targetPosition.position, transform.position) > 0.1)
        {
            Quaternion rotation = Quaternion.LookRotation(_moveDirection);
            _rigidbody.MoveRotation(rotation);
            _rigidbody.MovePosition(transform.position + (transform.forward * _moveSpeed * Time.deltaTime));
        }
        else
        {
            _animator.SetBool("HasReachedDestination", true);
            _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, Quaternion.LookRotation(_targetPosition.forward), _rotateSpeed * Time.deltaTime));
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
