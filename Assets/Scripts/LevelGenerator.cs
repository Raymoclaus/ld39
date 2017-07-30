using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public float size;
	public int density;
	public GameObject asteroid, star, battery;
	public List<GameObject> listToDestroy = new List<GameObject>();
	public Transform player;

	void Start()
	{
		StartCoroutine(CreateField(asteroid, 1, false, 1f, false, 0f));
	}

	public void DestroyAll()
	{
		foreach(GameObject obj in listToDestroy)
		{
			Destroy(obj);
		}
	}

	public void CreateAll()
	{
		StartCoroutine(CreateField(asteroid, density, true, 1f, true, 0.96f));
		StartCoroutine(CreateField(star, 4, true, 1f, true, 0.98f));
		StartCoroutine(CreateField(battery, 1, true, 29f, true, 0.96f));
		StartingBattery();
	}

	private IEnumerator CreateField(GameObject obj, int rounds, bool random, float reducer, bool addToList, float centerDispersion)
	{
		for (int i = 0; i < rounds; i++)
		{
			float angle = 0;
			GameObject createdObj;
			Vector2 pos;

			while (angle < 360)
			{
				createdObj = Instantiate(obj, transform);
				if (addToList)
				{
					listToDestroy.Add(createdObj);
				}
				pos = Vector2.zero;
				pos.x = Mathf.Sin(Mathf.Deg2Rad * angle);
				pos.y = Mathf.Cos(Mathf.Deg2Rad * angle);
				float multiplier = size;
				if (random)
				{
					float randomVal = Random.value;
					randomVal *= randomVal;
					randomVal = 1f - randomVal;
					randomVal = randomVal * centerDispersion * 0.98f + (1f - centerDispersion);
					multiplier *= randomVal;
				}
				pos *= multiplier;
				createdObj.transform.position = pos;
				angle += reducer;
			}
			yield return 0;
		}
	}

	public void RecreateBattery()
	{
		GameObject newBattery = Instantiate(battery, transform);
		listToDestroy.Add(newBattery);

		float angle = Vector2.Angle(Vector2.up, player.position);
		if (player.position.x < 0)
		{
			angle = 180f + (180f - angle);
		}
		angle += angle <= 180 ? 180 : -180;

		Vector2 pos = new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
		float multiplier = size * (Random.value * 0.96f + 0.04f);
		pos *= multiplier;
		newBattery.transform.position = pos;
	}

	private void StartingBattery()
	{
		GameObject newBattery = Instantiate(battery, transform);
		listToDestroy.Add(newBattery);
		newBattery.transform.position = Vector3.up * 3F;
	}
}
