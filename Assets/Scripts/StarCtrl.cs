using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCtrl : MonoBehaviour
{
	public Animator anim;

	void Start()
	{
		transform.localScale = new Vector3(2f * Random.value + 1f, 2f * Random.value + 1f, 1f);
		anim.speed = Random.value * 1f;
	}
}
