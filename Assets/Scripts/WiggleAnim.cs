using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleAnim : MonoBehaviour
{
	private float wiggleAngle = 15f, timer, finishTime, wiggleTime = 0.2f;

	void Start()
	{
		SetNewRandom();
	}

	void Update()
	{
		timer += Time.deltaTime;
		if (timer >= finishTime)
		{
			timer = 0f;
			SetNewRandom();
			StartCoroutine(Wiggle());
		}
	}

	private IEnumerator Wiggle()
	{
		float variance = 0f, counter = 0f, multiplier = 1f, swayTime = 0f, swayFinish = 0.1f;
		Vector3 rot = transform.eulerAngles;

		while (counter < wiggleTime)
		{
			counter += Time.deltaTime;
			swayTime += Time.deltaTime;
			if (swayTime >= swayFinish)
			{
				swayTime -= swayFinish;
				multiplier *= -1;
			}
			variance = counter / wiggleTime * wiggleAngle;

			rot.z = Mathf.Lerp(rot.z, variance * multiplier, swayTime / swayFinish);
			transform.eulerAngles = rot;
			yield return 0;
		}
		counter = 0f;

		while (counter < wiggleTime)
		{
			counter += Time.deltaTime;
			swayTime += Time.deltaTime;
			if (swayTime >= swayFinish)
			{
				swayTime -= swayFinish;
				multiplier *= -1;
			}
			variance = (wiggleTime - counter) / wiggleTime * wiggleAngle;

			rot.z = Mathf.Lerp(rot.z, variance * multiplier, swayTime / swayFinish);
			transform.eulerAngles = rot;
			yield return 0;
		}
		transform.eulerAngles = Vector3.zero;
	}

	private void SetNewRandom()
	{
		finishTime = Random.Range(2f, 4f);
	}
}
