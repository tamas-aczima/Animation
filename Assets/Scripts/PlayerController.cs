using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour {

    [SerializeField] private Image _conversationBox;
    [SerializeField] private Text _conversationText;
    [SerializeField] private Button _question1Button;
    [SerializeField] private Button _question2Button;
    [SerializeField] private Button _question3Button;
    [SerializeField] private Button _nextButton;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _rotateSpeed;
    private CharacterController _controller;
    private Vector3 _moveDirection = Vector3.zero;
    private bool _isBehindCounter = false;
    private bool _question1Asked = false;
    private bool _question2Asked = false;
    private bool _question3Asked = false;
    private int _burgerCount = 0;
    private CustomerController _customer = null;

    private List<string> _question1PositiveAnswers = new List<string>
    {
        "Of course, thanks.",
        "Yes please.",
        "A burger would be perfect.",
        "You bet.",
        "Yeah, I'm starving."
    };

    private List<string> _question1NegativeAnswers = new List<string>
    {
        "No thanks.",
        "I'll check the menu first.",
        "No thanks, I've already had lunch.",
        "Maybe later.",
        "I'm good for now, thanks"
    };

    // Use this for initialization
    void Start () {
        //lock mouse to game
        Cursor.lockState = CursorLockMode.Locked;
        _controller = GetComponent<CharacterController>();

        //hide UI
        _conversationBox.gameObject.SetActive(false);
        HideQuestions();

        //add listener to buttons
        _question1Button.onClick.AddListener(() => Question1());
        _question2Button.onClick.AddListener(() => Question2());
        _question3Button.onClick.AddListener(() => Question3());
        _nextButton.onClick.AddListener(() => NextQuestion());
    }
	
	// Update is called once per frame
	void Update () {

        if (_controller.enabled)
        {
            //move
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _moveDirection = transform.TransformDirection(_moveDirection);
            _controller.Move(_moveDirection * _walkSpeed * Time.deltaTime);

            //rotate
            if (Input.GetAxis("Mouse X") < 0)
            {
                transform.Rotate(Vector3.up * -_rotateSpeed);
            }
            if (Input.GetAxis("Mouse X") > 0)
            {
                transform.Rotate(Vector3.up * _rotateSpeed);
            }
        }

        //interact
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
            {
                //only interact with first customer and only from behind counter
                _customer = hit.collider.gameObject.GetComponent<CustomerController>();
                if (_isBehindCounter && _customer != null && _customer.IsNextCustomer)
                {
                    //disable movement
                    _controller.enabled = false;
                    
                    //unlock mouse and make cursor visible
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;

                    //show conversation box
                    if (!_customer.IsConversationStarted)
                    {
                        _conversationBox.gameObject.SetActive(true);
                        _customer.IsConversationStarted = true;
                        _customer.Animator.SetInteger("TalkNum", UnityEngine.Random.Range(1, 4));
                        _customer.Animator.SetBool("IsBeingServed", true);
                    }  
                }

            }
        }
	}

    private void NextQuestion()
    {
        //hide conversation box
        _conversationBox.gameObject.SetActive(false);

        //clear text
        _conversationText.text = "";

        //show questions if they haven't been asked
        if (!_question1Asked)
        {
            _question1Button.gameObject.SetActive(true);
        }
        if (!_question2Asked)
        {
            _question2Button.gameObject.SetActive(true);
        }
        if (!_question3Asked)
        {
            _question3Button.gameObject.SetActive(true);
        }

        //finish order
        if (_question1Asked && _question2Asked && _question3Asked)
        {
            _customer.IsCustomerServed = true;
            _customer.Animator.SetBool("IsBeingServed", false);
            _controller.enabled = true;
            _customer.WaitForOrder();
            _customer = null;
            GameObject.Find("SceneManager").GetComponent<SceneManager>().ManageQueue();
            ResetConversation();
        }
    }

    //Question buttons
    private void Question1()
    {
        HideQuestions();
        _conversationBox.gameObject.SetActive(true);
        _question1Asked = true;

        int i = UnityEngine.Random.Range(0, 2);
        if (i == 0)
        {
            _conversationText.text = _question1NegativeAnswers.ElementAt(UnityEngine.Random.Range(0, _question1NegativeAnswers.Count));
        }
        else
        {
            _conversationText.text = _question1PositiveAnswers.ElementAt(UnityEngine.Random.Range(0, _question1NegativeAnswers.Count));
            _burgerCount++;
        }
    }

    private void Question2()
    {
        HideQuestions();
        _conversationBox.gameObject.SetActive(true);
        _question2Asked = true;
        _conversationText.text = "No";
    }

    private void Question3()
    {
        HideQuestions();
        if (_question1Asked && _question2Asked)
        {
            _question3Asked = true;
            _conversationBox.gameObject.SetActive(true);
            _conversationText.text = "Okay";
        }
        else
        {
            _conversationBox.gameObject.SetActive(true);
            _conversationText.text = "I haven't ordered yet";
        }        
    }

    private void HideQuestions()
    {
        _question1Button.gameObject.SetActive(false);
        _question2Button.gameObject.SetActive(false);
        _question3Button.gameObject.SetActive(false);
    }

    private void ResetConversation()
    {
        _conversationText.text = "Can I order?";
        _question1Asked = false;
        _question2Asked = false;
        _question3Asked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "CounterCollider")
        {
            _isBehindCounter = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _isBehindCounter = false;
    }
}
