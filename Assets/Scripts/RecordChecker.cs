using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordChecker : MonoBehaviour
{
	public Text record;
	public bool recordBeaten;
	private float flashTimer;


	void Start()
	{
		UpdateRecord();
	}

	void Update()
	{
		if (recordBeaten)
		{
			flashTimer += Time.deltaTime;
			if (flashTimer >= 2f)
			{
				flashTimer -= 2f;
			}

			record.color = flashTimer < 1f ? Color.white : Color.yellow;
		}
		else
		{
			record.color = Color.white;
		}
	}

	public void UpdateRecord()
	{
		if (PlayerPrefs.GetInt("DistanceRecord", 0) > 0)
		{
			record.enabled = true;
			record.text = string.Format("Record Distance Travelled: {0}m", PlayerPrefs.GetInt("DistanceRecord"));
		}
		else
		{
			record.enabled = false;
		}
	}
}
