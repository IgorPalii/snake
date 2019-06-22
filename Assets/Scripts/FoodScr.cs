using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScr : MonoBehaviour 
{
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Snake")
		{
			Destroy(gameObject);
		}
	}
}
