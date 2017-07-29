using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCtrl : MonoBehaviour
{
	public Sprite[] sprites;
	public float rotation, speedRange;
	public SpriteRenderer sprRend;

	void Start()
	{
		rotation = Random.value * speedRange - speedRange / 2f;
		UpdateSprite();
	}

	void Update()
	{
		transform.Rotate(0, 0, rotation * Time.deltaTime);
	}

	private void UpdateSprite()
	{
		sprRend.sprite = sprites[Random.Range(0, sprites.Length)];
	}
}
