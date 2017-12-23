using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefController : MonoBehaviour {

    [SerializeField] private OrderController orderController;
    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (orderController.ReadyBurgers < orderController.PrepareBurgers)
        {
            animator.SetBool("ShouldCook", true);
        }
        else
        {
            animator.SetBool("ShouldCook", false);
        }
	}
}
