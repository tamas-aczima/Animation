using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CustomerController : MonoBehaviour {
    
    private SceneManager sceneManager = null;
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
    private float angryWaitingTime = 0f;
    private float angryWaitingTimer = 0f;
    private GameObject wayPoint;

    // Use this for initialization
    void Start () {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        NewTarget();
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        waitingArea = GameObject.Find("WaitingArea");
        wayPoint = GameObject.Find("WayPoint");
        GetAngryWaitingTime();
    }
	
	// Update is called once per frame
	void Update () {   

        //manage queue
        if (queuePosition == 0)
        {
            isNextCustomer = true;
        }

        if (targetPosition == wayPoint.transform && Vector3.Distance(transform.position, targetPosition.position) < 0.5)
        {
            targetPosition = sceneManager.Chairs.ElementAt(sceneManager.NextAvailableChair).GetComponent<Chair>().Target;
            sceneManager.NextAvailableChair++;
            NewTarget();
            Debug.Log(targetPosition.position);
        }
    }

    void FixedUpdate()
    {
        moveDirection.y = 0;
        moveDirection.Normalize();

        //move
        if (Vector3.Distance(targetPosition.position, transform.position) > 0.2)
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
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !isCustomerServerd)
            {
                angryWaitingTimer += Time.deltaTime;
            }
            
            if (angryWaitingTimer >= angryWaitingTime)
            {
                angryWaitingTimer = 0f;
                animator.SetTrigger("IsAngry");
            }

            if (targetPosition.gameObject.name.StartsWith("Target"))
            {
                targetPosition.parent.gameObject.GetComponent<Chair>().IsOccupied = true;
                animator.SetTrigger("SitDown");
                //targetPosition = targetPosition.transform.parent.Find("Sitting").transform;
            }
        }        
    }

    public void NewTarget()
    {
        moveDirection = targetPosition.position - transform.position;
    }

    public void WaitForOrder()
    {
        targetPosition = wayPoint.transform;
        NewTarget();
    }

    public void GetAngryWaitingTime()
    {
        angryWaitingTime = Random.Range(10f, 20f);
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
