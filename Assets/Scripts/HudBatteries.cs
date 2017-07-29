using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
				batteries.Add(Instantiate(battery, transform));
			}
			else if (batteries.Count > 0)
			{
				GameObject removedBattery = batteries[0];
				batteries.Remove(removedBattery);
				Destroy(removedBattery);
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
}
