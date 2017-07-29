using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCtrl : MonoBehaviour
{
	public float size;
	private float previousSize;

	void Start()
	{
		size = Random.value + 1f;
	}

	void Update()
	{
		if (previousSize != size)
		{
			UpdateSize();
			previousSize = size;
		}
	}

	private void UpdateSize()
	{
		transform.localScale = Vector3.one * size;
	}
}
