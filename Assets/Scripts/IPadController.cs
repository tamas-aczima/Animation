using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IPadController : MonoBehaviour {

    [SerializeField] private GameObject iPad;
    [SerializeField] private SceneManager sceneManager;
    [SerializeField] private Text timerText;
    [SerializeField] private Text customersInQueue;
    [SerializeField] private Text customersWaitingForOrder;
    private CharacterController controller;
    private float minutes;
    private float seconds;

	// Use this for initialization
	void Start () {
        iPad.gameObject.SetActive(false);
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        ToggleIPad();
        UpdateTimer();
        UpdateCustomersInQueue();
        UpdateCustomersWaitingForOrder();
	}

    void ToggleIPad()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !iPad.activeSelf)
        {
            //enable ipad
            iPad.gameObject.SetActive(true);

            //disable movement
            controller.enabled = false;

            //unlock mouse and make cursor visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

        }
        else if (Input.GetKeyDown(KeyCode.Tab) && iPad.activeSelf)
        {
            //disable ipad
            iPad.gameObject.SetActive(false);

            //enable movement
            controller.enabled = true;

            //lock mouse and make cursor invisible
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void UpdateTimer()
    {
        minutes = Mathf.Floor(sceneManager.Timer / 60);
        seconds = sceneManager.Timer % 60;
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    void UpdateCustomersInQueue()
    {
        customersInQueue.text = "Customers In Queue: " + sceneManager.CustomersInQueue;
    }

    void UpdateCustomersWaitingForOrder()
    {
        customersWaitingForOrder.text = "Customers Waiting For Order: " + sceneManager.ServedCustomers;
    }
}
