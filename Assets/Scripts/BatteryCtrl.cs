using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCtrl : MonoBehaviour
{
	public BoxCollider2D col;
	public GameObject sparkle;
	public LevelGenerator levelGen;

	void Start()
	{
		levelGen = FindObjectOfType<LevelGenerator>();

		CheckCollisions();

		FindObjectOfType<PlayerCtrl>().batteries.Add(this);
	}

	private void CheckCollisions()
	{
		float angle = Vector2.Angle(Vector2.up, -transform.position + Vector3.up);
		Vector2 direction = new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));

		while (Physics2D.OverlapArea(col.bounds.min, col.bounds.max, 1 << LayerMask.NameToLayer("Ground")))
		{
			transform.position += (Vector3)direction;
		}
	}

	public void Collect()
	{
		for (int i = 0; i < 20; i++)
		{
			GameObject sparkleClone = Instantiate(sparkle);
			sparkleClone.transform.position = transform.position;
		}
		levelGen.RecreateBattery();
		Destroy(gameObject);
	}
}
