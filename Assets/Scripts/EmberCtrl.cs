using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmberCtrl : MonoBehaviour
{
	public Vector2 direction;
	private float lifeSpan = 0.2f, speed;


	void Start()
	{
		speed = Random.value * 3f;
		if (direction.x == 0f)
		{
			direction.x = Random.value * 0.1f - 0.05f;
		}
		if (direction.y == 0f)
		{
			direction.y = Random.value * 0.1f - 0.05f;
		}
		direction.x = Mathf.MoveTowards(direction.x, direction.x * 3f, Random.value);
		direction.y = Mathf.MoveTowards(direction.y, direction.y * 3f, Random.value);
		direction.Normalize();
		StartCoroutine(Life());
	}

	private IEnumerator Life()
	{
		while (lifeSpan > 0f)
		{
			lifeSpan -= Time.deltaTime;
			transform.position += (Vector3)direction * speed * lifeSpan;
			yield return 0;
		}
		Destroy(gameObject);
	}
}
