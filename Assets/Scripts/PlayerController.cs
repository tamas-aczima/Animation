using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour {

    [SerializeField] private Image _conversationBox;
    [SerializeField] private Button _question1Button;
    private CharacterController _controller;
    private Vector3 _moveDirection = Vector3.zero;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _rotateSpeed;
    private bool _isBehindCounter = false;

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        _controller = GetComponent<CharacterController>();

        _conversationBox.gameObject.SetActive(false);
        _question1Button.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
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

        //interact
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            if (Physics.Raycast(transform.position, forward, out hit, 10))
            {
                if (_isBehindCounter && hit.collider.gameObject.GetComponent<CustomerController>() != null && hit.collider.gameObject.GetComponent<CustomerController>().IsNextCustomer)
                {
                    hit.collider.gameObject.GetComponent<CustomerController>().StartConversation = true;
                    _conversationBox.gameObject.SetActive(true);
                    _question1Button.gameObject.SetActive(true);
                }

            }
        }
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
