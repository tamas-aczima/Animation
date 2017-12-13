using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IPadController : MonoBehaviour {

    private CharacterController controller;
    [SerializeField] private GameObject iPad;
    [SerializeField] private SceneManager sceneManager;
    [SerializeField] private OrderController orderController;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas tablesCanvas;
    [SerializeField] private Button orderBurgerButton;
    [SerializeField] private Button orderDrinkButton;
    [SerializeField] private Button showTablesButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Text timerText;
    [SerializeField] private Text customersInQueue;
    [SerializeField] private Text customersWaitingForOrder;
    [SerializeField] private Text numberOfOrderedBurgers;
    [SerializeField] private Text numberOfReadyBurgers;
    [SerializeField] private Text numberOfBurgersInInventory;

    private float minutes;
    private float seconds;

	// Use this for initialization
	void Start () {
        iPad.gameObject.SetActive(false);
        controller = GetComponent<CharacterController>();
        orderBurgerButton.onClick.AddListener(OrderBurger);
        orderDrinkButton.onClick.AddListener(OrderDrink);
        showTablesButton.onClick.AddListener(ShowTables);
        backButton.onClick.AddListener(BackToMainScreen);
	}
	
	// Update is called once per frame
	void Update () {
        ToggleIPad();
        UpdateTimer();
        UpdateCustomersInQueue();
        UpdateCustomersWaitingForOrder();
        UpdateOrderedBurgers();
        UpdateReadyBurgers();
        UpdateBurgersInInventory();
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

    void UpdateOrderedBurgers()
    {
        numberOfOrderedBurgers.text = "Burgers Ordered: " + orderController.OrderedBurgers;
    }

    void UpdateReadyBurgers()
    {
        numberOfReadyBurgers.text = "Burgers Ready: " + orderController.ReadyBurgers;
    }

    void UpdateBurgersInInventory()
    {
        numberOfBurgersInInventory.text = "Burgers In inventory: " + orderController.BurgersInInventory;
    }

    void OrderBurger()
    {
        StartCoroutine(orderController.CookBurger());
    }

    void OrderDrink()
    {
        StartCoroutine(orderController.PrepareDrink());
    }

    void ShowTables()
    {
        mainCanvas.gameObject.SetActive(false);
        tablesCanvas.gameObject.SetActive(true);
    }

    void BackToMainScreen()
    {
        mainCanvas.gameObject.SetActive(true);
        tablesCanvas.gameObject.SetActive(false);
    }
}
