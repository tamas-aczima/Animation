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
    private bool _isConversationStarted = false;
    private bool _isCustomerServerd = false;
    private GameObject _waitingArea;

    // Use this for initialization
    void Start () {
        NewTarget();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _waitingArea = GameObject.Find("WaitingArea");
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
            _animator.SetBool("HasReachedDestination", false);
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

    public void NewTarget()
    {
        _moveDirection = _targetPosition.position - transform.position;
    }

    public void WaitForOrder()
    {
        Vector3 randomPoint = Random.insideUnitSphere * 4;
        randomPoint.y = 0;
        GameObject randomPointObject = new GameObject();
        randomPointObject.transform.position = _waitingArea.transform.position + randomPoint;
        _targetPosition = randomPointObject.transform;
        NewTarget();
    }

    public Transform TargetPosition
    {
        get { return _targetPosition; }
        set { _targetPosition = value; }
    }

    public Animator Animator
    {
        get { return _animator; }
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

    public bool IsConversationStarted
    {
        get { return _isConversationStarted; }
        set { _isConversationStarted = value; }
    }

    public bool IsCustomerServed
    {
        get { return _isCustomerServerd; }
        set { _isCustomerServerd = value; }
    }
}
