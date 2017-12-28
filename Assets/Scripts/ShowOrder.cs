using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowOrder : MonoBehaviour {

    [SerializeField] private Seat seat;
    [SerializeField] private Image seatImage;
    [SerializeField] private Sprite free;
    [SerializeField] private Sprite occupied;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (seat.IsOccupied)
        {
            seatImage.sprite = occupied;
        }
        else
        {
            seatImage.sprite = free;
        }
    }

    private void OnMouseOver()
    {
        if (seat.IsOccupied)
        {
            Debug.Log("show order");
        }      
    }
}
