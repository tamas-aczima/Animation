using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour {
    
    private Transform targetPosition;
    private Vector3 moveDirection = Vector3.zero;
    private Animator animator;
    private Rigidbody _rigidbody;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotateSpeed = 100f;
    private int queuePosition;
    private bool isNextCustomer = false;
    private bool isConversationStarted = false;
    private bool isCustomerServerd = false;
    private GameObject waitingArea;
    private float angryWaitingTime = 10f;
    private float angryWaitingTimer = 0f;

    // Use this for initialization
    void Start () {
        NewTarget();
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        waitingArea = GameObject.Find("WaitingArea");
    }
	
	// Update is called once per frame
	void Update () {   

        //manage queue
        if (queuePosition == 0)
        {
            isNextCustomer = true;
        }
        Debug.Log(angryWaitingTimer);
    }

    void FixedUpdate()
    {
        moveDirection.y = 0;
        moveDirection.Normalize();

        //move
        if (Vector3.Distance(targetPosition.position, transform.position) > 0.1)
        {
            animator.SetBool("HasReachedDestination", false);
            Quaternion rotation = Quaternion.LookRotation(moveDirection);
            _rigidbody.MoveRotation(rotation);
            _rigidbody.MovePosition(transform.position + (transform.forward * moveSpeed * Time.deltaTime));
        }
        else
        {
            animator.SetBool("HasReachedDestination", true);
            _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, Quaternion.LookRotation(targetPosition.forward), rotateSpeed * Time.deltaTime));
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                angryWaitingTimer += Time.deltaTime;
            }
            
            if (angryWaitingTimer >= angryWaitingTime)
            {
                angryWaitingTimer = 0f;
                animator.SetTrigger("IsAngry");
            }
        }        
    }

    public void NewTarget()
    {
        moveDirection = targetPosition.position - transform.position;
    }

    public void WaitForOrder()
    {
        Vector3 randomPoint = Random.insideUnitSphere * 4;
        randomPoint.y = 0;
        GameObject randomPointObject = new GameObject();
        randomPointObject.transform.position = waitingArea.transform.position + randomPoint;
        targetPosition = randomPointObject.transform;
        NewTarget();
    }

    public Transform TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }

    public Animator Animator
    {
        get { return animator; }
    }

    public int QueuePosition
    {
        get { return queuePosition; }
        set { queuePosition = value; }
    }
    
    public bool IsNextCustomer
    {
        get { return isNextCustomer; }
        set { isNextCustomer = value; }
    }

    public bool IsConversationStarted
    {
        get { return isConversationStarted; }
        set { isConversationStarted = value; }
    }

    public bool IsCustomerServed
    {
        get { return isCustomerServerd; }
        set { isCustomerServerd = value; }
    }
}
