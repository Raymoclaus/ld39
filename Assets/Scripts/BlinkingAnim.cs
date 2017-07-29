using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingAnim : MonoBehaviour
{
	public Image img;
	public SpriteRenderer sprRend;
	private float timer = 0f;

	void Update()
	{
		timer += Time.deltaTime;
		if (timer >= 2f)
		{
			timer -= 2f;
		}
		bool isOn = timer <= 1f;

		if (img != null)
		{
			img.enabled = isOn;
		}
		if (sprRend != null)
		{
			sprRend.enabled = isOn;
		}
	}
}
