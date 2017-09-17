//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class WaterEnter : MonoBehaviour {
	public GameObject PlayerREF;
	public Animator anim;

	// Use this for initialization
	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject == PlayerREF) 
		{
			anim.SetBool ("Swim", true);
		}
	}

	void OnTriggerExit(Collider other) 
	{
		if (other.gameObject == PlayerREF) 
		{
			anim.SetBool ("Swim", false);
		}
	}
}
