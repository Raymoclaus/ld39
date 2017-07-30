using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudBatteries : MonoBehaviour
{
	public PlayerCtrl player;
	public GameObject battery;
	public List<GameObject> batteries = new List<GameObject>();
	private int batteryCount = 0;
	private bool isActive;
	private bool wasPaused;


	void Start()
	{
		UpdateBatteryCount();
		SetActivity(false);
	}

	void Update()
	{
		if (batteryCount != player.batteryCount)
		{
			UpdateBatteryCount();
		}

		//hide the battery hud icons when not playing
		//More efficient to write it like this so SetActivity isn't called every frame
		if (player.gameState == 1 && !isActive)
		{
			isActive = true;
			SetActivity(true);
		}
		if (player.gameState != 1 && isActive)
		{
			isActive = false;
			SetActivity(false);
		}

		if (player.isPaused && !wasPaused)
		{
			wasPaused = true;
			SetActivity(false);
		}
		if (!player.isPaused && wasPaused)
		{
			wasPaused = false;
			SetActivity(true);
		}
	}

	private void UpdateBatteryCount()
	{
		batteryCount = player.batteryCount;
		while (batteryCount != batteries.Count)
		{
			if (batteryCount > batteries.Count)
			{
				GameObject newBattery = Instantiate(battery, transform);
				newBattery.transform.localScale = Vector3.one;
				batteries.Add(newBattery);
			}
			else if (batteries.Count > 0)
			{
				GameObject removedBattery = batteries[batteries.Count - 1];
				batteries.Remove(removedBattery);
				StartCoroutine(Disappear(removedBattery));
			}
		}
	}

	private void SetActivity(bool activity)
	{
		foreach (GameObject battery in batteries)
		{
			battery.SetActive(activity);
		}
	}

	public IEnumerator Disappear(GameObject obj)
	{
		float timer = 0f, finish = 1.5f;
		Vector3 originalSize = obj.transform.localScale;

		while (timer < finish)
		{
			timer += Time.deltaTime;
			obj.transform.localScale = originalSize * (finish - timer) / finish;
			yield return 0;
		}
		Destroy(obj);
	}
}
