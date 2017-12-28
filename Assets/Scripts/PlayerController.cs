using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour {

    [SerializeField] private SceneManager sceneManager;
    [SerializeField] private OrderController orderController;
    [SerializeField] private Image conversationBox;
    [SerializeField] private Text conversationText;
    [SerializeField] private Button question1Button;
    [SerializeField] private Button question2Button;
    [SerializeField] private Button question3Button;
    [SerializeField] private Button nextButton;
    [SerializeField] private Text customerNameText;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private GameObject burgerPrefab;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private bool isBehindCounter = false;
    private bool question1Asked = false;
    private bool question2Asked = false;
    private bool question3Asked = false;
    private CustomerController customer = null;
    private bool noBurger = true;
    private new Camera camera;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float pitchLimit = 40.0f;

    private List<string> conversationStarters = new List<string>
    {
        "Hi, can I order please?",
        "Can you take my order please?",
        "Hi, I would like to order."
    };

    private List<string> question1PositiveAnswers = new List<string>
    {
        "Of course, thanks.",
        "Yes please.",
        "A burger would be perfect.",
        "You bet.",
        "Yeah, I'm starving."
    };

    private List<string> question1NegativeAnswers = new List<string>
    {
        "No thanks.",
        "I'll check the menu first.",
        "No thanks, I've already had lunch.",
        "Maybe later.",
        "I'm good for now, thanks."
    };

    private List<string> question2PositiveAnswers = new List<string>
    {
        "Sure, thanks.",
        "Yes please.",
        "A coke would be perfect.",
        "I would like a glass of coke.",
        "Of course."
    };

    private List<string> question2NegativeAnswers = new List<string>
    {
        "No thanks, maybe later.",
        "I'm not that thirsty.",
        "No thank you."
    };

    // Use this for initialization
    void Start () {
        camera = transform.Find("Main Camera").GetComponent<Camera>();

        //lock mouse to game
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();

        //hide UI
        conversationBox.gameObject.SetActive(false);
        HideQuestions();

        //add listener to buttons
        question1Button.onClick.AddListener(() => Question1());
        question2Button.onClick.AddListener(() => Question2());
        question3Button.onClick.AddListener(() => Question3());
        nextButton.onClick.AddListener(() => NextQuestion());
    }
	
	// Update is called once per frame
	void Update () {

        if (controller.enabled)
        {
            //move
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            controller.Move(moveDirection * walkSpeed * Time.deltaTime);

            //rotate
            yaw += rotateSpeed * Input.GetAxis("Mouse X");
            pitch -= rotateSpeed * Input.GetAxis("Mouse Y");
            if (pitch >= pitchLimit)
            {
                pitch = pitchLimit;
            }
            else if (pitch <= -pitchLimit)
            {
                pitch = -pitchLimit;
            }
            transform.eulerAngles = new Vector3(0, yaw, 0);
            camera.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        }

        //interact
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 10))
            {
                if (hit.collider.gameObject.GetComponent<CustomerController>() != null)
                {
                    //only interact with first customer and only from behind counter
                    customer = hit.collider.gameObject.GetComponent<CustomerController>();
                    if (isBehindCounter && customer != null && customer.IsNextCustomer)
                    {
                        //disable movement
                        controller.enabled = false;

                        //unlock mouse and make cursor visible
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.lockState = CursorLockMode.Confined;
                        Cursor.visible = true;

                        //show conversation box
                        if (!customer.IsConversationStarted)
                        {
                            conversationBox.gameObject.SetActive(true);
                            customer.IsConversationStarted = true;
                            customerNameText.text = hit.collider.gameObject.name;
                            customer.Animator.SetInteger("TalkNum", UnityEngine.Random.Range(1, 4));
                            customer.Animator.SetBool("IsBeingServed", true);
                        }
                    }
                }
                
                if (hit.collider.gameObject.GetComponent<Burger>() != null && hit.collider.gameObject.GetComponent<Burger>().CanBePicked)
                {
                    orderController.BurgersInInventory++;
                    orderController.ReadyBurgers--;
                    Destroy(hit.collider.gameObject);
                }

                if (hit.collider.gameObject.name.StartsWith("Plate") && orderController.BurgersInInventory > 0 && hit.collider.transform.parent.GetComponent<Seat>().Customer.HasOrderedBurger)
                {
                    Debug.Log(hit.collider.transform.parent.GetComponent<Seat>().Customer.HasOrderedBurger);
                    Instantiate(burgerPrefab, hit.collider.gameObject.transform.position + new Vector3(-0.15f, 0, 0), Quaternion.identity);
                    hit.collider.transform.parent.GetComponent<Seat>().Customer.HasBurger = true;
                }
            }
        }
	}

    

    private void NextQuestion()
    {
        //hide conversation box
        conversationBox.gameObject.SetActive(false);

        //clear text
        conversationText.text = "";

        //show questions if they haven't been asked
        if (!question1Asked)
        {
            question1Button.gameObject.SetActive(true);
        }
        if (!question2Asked)
        {
            question2Button.gameObject.SetActive(true);
        }
        if (!question3Asked)
        {
            question3Button.gameObject.SetActive(true);
        }

        //finish order
        if (question1Asked && question2Asked && question3Asked)
        {
            customer.IsCustomerServed = true;
            customer.Animator.SetBool("IsBeingServed", false);
            controller.enabled = true;
            customer.WaitForOrder();
            customer = null;
            sceneManager.ManageQueue();
            ResetConversation();
        }
    }

    //Question buttons
    private void Question1()
    {
        HideQuestions();
        conversationBox.gameObject.SetActive(true);
        question1Asked = true;

        int i = UnityEngine.Random.Range(0, 2);
        if (i == 0)
        {
            conversationText.text = question1NegativeAnswers.ElementAt(UnityEngine.Random.Range(0, question1NegativeAnswers.Count));
        }
        else
        {
            conversationText.text = question1PositiveAnswers.ElementAt(UnityEngine.Random.Range(0, question1PositiveAnswers.Count));
            orderController.OrderedBurgers++;
            customer.HasOrderedBurger = true;
            noBurger = false;
        }
    }

    private void Question2()
    {
        HideQuestions();
        conversationBox.gameObject.SetActive(true);
        question2Asked = true;
        int i = UnityEngine.Random.Range(0, 2);
        if (i == 0 && !noBurger)
        {
            conversationText.text = question2NegativeAnswers.ElementAt(UnityEngine.Random.Range(0, question2NegativeAnswers.Count));
        }
        else
        {
            conversationText.text = question2PositiveAnswers.ElementAt(UnityEngine.Random.Range(0, question2PositiveAnswers.Count));
        }
    }

    private void Question3()
    {
        HideQuestions();
        if (question1Asked && question2Asked)
        {
            question3Asked = true;
            conversationBox.gameObject.SetActive(true);
            conversationText.text = "Okay.";
        }
        else
        {
            conversationBox.gameObject.SetActive(true);
            conversationText.text = "I haven't ordered yet.";
        }        
    }

    private void HideQuestions()
    {
        question1Button.gameObject.SetActive(false);
        question2Button.gameObject.SetActive(false);
        question3Button.gameObject.SetActive(false);
    }

    private void ResetConversation()
    {
        conversationText.text = conversationStarters.ElementAt(UnityEngine.Random.Range(0, conversationStarters.Count));
        question1Asked = false;
        question2Asked = false;
        question3Asked = false;
        noBurger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "CounterCollider")
        {
            isBehindCounter = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isBehindCounter = false;
    }
}
