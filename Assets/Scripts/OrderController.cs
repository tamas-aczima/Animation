using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour {

    [SerializeField] private Transform[] burgerPositions;
    [SerializeField] private Transform[] drinkPositions;
    [SerializeField] private float burgerCookTime;
    [SerializeField] private float drinkPrepareTime;
    [SerializeField] private GameObject burgerPrefab;
    [SerializeField] private GameObject drinkPrefab;
    private int orderedBurgers = 0;
    private int prepareBurgers = 0;
    private int readyBurgers = 0;
    private int burgersInInventory = 0;
    private int orderedDrinks = 0;
    private int readyDrinks = 0;
    private int nextIndex = 0;
    private int nextDrinkIndex = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator CookBurger()
    {
        yield return new WaitForSeconds(burgerCookTime);
        Instantiate(burgerPrefab, burgerPositions[nextIndex].position, Quaternion.identity);
        nextIndex++;
        readyBurgers++;
        prepareBurgers--;
    }

    public IEnumerator PrepareDrink()
    {
        yield return new WaitForSeconds(drinkPrepareTime);
        Instantiate(drinkPrefab, drinkPositions[nextDrinkIndex].position, Quaternion.identity);
        nextDrinkIndex++;
        readyDrinks++;
    }

    public int OrderedBurgers
    {
        get { return orderedBurgers; }
        set { orderedBurgers = value; }
    }

    public int PrepareBurgers
    {
        get { return prepareBurgers; }
        set { prepareBurgers = value; }
    }

    public int ReadyBurgers
    {
        get { return readyBurgers; }
        set { readyBurgers = value; }
    }

    public int BurgersInInventory
    {
        get { return burgersInInventory; }
        set { burgersInInventory = value; }
    }
}
