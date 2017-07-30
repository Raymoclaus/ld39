using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour
{
	public PlayerCtrl player;
	//HUD elements
	public GameObject spaceToStart, controls, end, record, pause, spaceToContinue, PToContinue, EscToQuit;
	private int state = -1;

	void Update()
	{
		if (state != player.gameState)
		{
			state = player.gameState;

			spaceToStart.SetActive(state == 0);
			record.SetActive(state != 1);
			controls.SetActive(state == 0);
			end.SetActive(state == 2);
			spaceToContinue.SetActive(state == 2);
		}

		pause.SetActive(player.isPaused);
		PToContinue.SetActive(player.isPaused);
		EscToQuit.SetActive(player.isPaused || state == 0);
	}
}
