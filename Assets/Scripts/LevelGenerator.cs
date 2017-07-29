using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public float size;
	public int density;
	public GameObject asteroid, star, battery;
	public List<GameObject> listToDestroy = new List<GameObject>();

	void Start()
	{
		StartCoroutine(CreateField(asteroid, 1, false, 1f, false));
	}

	private void DestroyAll()
	{
		foreach(GameObject obj in listToDestroy)
		{
			Destroy(obj);
		}
	}

	public void CreateAll()
	{
		DestroyAll();
		StartCoroutine(CreateField(asteroid, density, true, 1f, true));
		StartCoroutine(CreateField(star, 5, true, 1f, true));
		StartCoroutine(CreateField(battery, 1, true, 12f, true));
	}

	private IEnumerator CreateField(GameObject obj, int rounds, bool random, float reducer, bool addToList)
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
					float randomVal = Random.value * 0.95f + 0.05f;
					multiplier *= randomVal;
				}
				pos *= multiplier;
				createdObj.transform.position = pos;
				angle += reducer;
			}
			yield return 0;
		}
	}
}
