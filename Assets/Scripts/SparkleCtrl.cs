using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkleCtrl : MonoBehaviour
{
	private float timer = 0f, speed, startingSpeed;
	private Vector2 direction;
	private Vector3 startingSize;

	void Start()
	{
		float angle = Random.value * 360f;
		direction = new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
		startingSpeed = Random.value * 10f;
		startingSize = transform.localScale;
	}

	void Update()
	{
		timer += Time.deltaTime;

		if (timer >= 2f)
		{
			Destroy(gameObject);
		}

		speed = Mathf.Lerp(startingSpeed, 0f, timer / 2f);
		transform.position += (Vector3)direction * speed * Time.deltaTime;
		transform.Rotate(Vector3.forward * speed * Time.deltaTime * 10f);

		transform.localScale = startingSize * (2f - timer) / 2f;
	}
}
