using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour
{
	public PlayerCtrl player;
	//HUD elements
	public GameObject spaceToStart, controls, end, record;
	private int state = -1;

	void Update()
	{
		if (state != player.gameState)
		{
			state = player.gameState;

			spaceToStart.SetActive(state == 0);
			record.SetActive(state != 1);
		}
	}
}
