using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
	//movement stuff
	public float speedLimit, speedMplier, speedDecay;
	private float hSpeed, vSpeed;
	private Vector2 move;

	//collision stuff
	private float accuracy = 0.01f;
	private BoxCollider2D col;

	//light mechanic stuff
	private int batteries = 3;

	//animation stuff
	public Transform spr;
	private SpriteRenderer sprRend;
	public Sprite[] sprites = new Sprite[3];


	void Start()
	{
		col = GetComponent<BoxCollider2D>();
		sprRend = spr.GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		CheckInput();
		DetermineMovement();
		ApplyMovement();
	}

	private void CheckInput()
	{
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			hSpeed -= Time.deltaTime * speedMplier;
		}
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			hSpeed += Time.deltaTime * speedMplier;
		}
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{
			vSpeed += Time.deltaTime * speedMplier;
		}
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{
			vSpeed -= Time.deltaTime * speedMplier;
		}

		//limit speed
		Vector2 speedCheck = new Vector2(hSpeed, vSpeed);
		if (Vector2.Distance(Vector2.zero, speedCheck) > speedLimit)
		{
			Debug.Log(speedCheck.x);
			speedCheck.Normalize();
			speedCheck *= speedLimit;
			hSpeed = speedCheck.x;
			vSpeed = speedCheck.y;
		}
	}

	private void DetermineMovement()
	{
		move.x = hSpeed;
		move.y = vSpeed;
	}

	private void ApplyMovement()
	{
		Vector2 gradualMove = Vector2.zero, compare = Vector2.zero;

		//loop to make sure you don't teleport past objects if you're moving fast enough
		while (move != Vector2.zero)
		{
			//make sure you stop when a collision is detected in the direction you're moving
			if (CheckDirection(move.x < 0 ? Vector2.left : Vector2.right))
			{
				move.x = 0;
				hSpeed = 0;
			}
			if (CheckDirection(move.y < 0 ? Vector2.down : Vector2.up))
			{
				move.y = 0;
				vSpeed = 0;
			}

			gradualMove = move;
			move = Vector2.MoveTowards(move, Vector2.zero, accuracy);
			compare = gradualMove - move;
			transform.position += (Vector3)compare;
		}
	}

	private bool CheckDirection(Vector2 direction)
	{
		Vector3 originalPos = transform.position;
		transform.position += (Vector3)direction * accuracy;
		bool check = Physics2D.OverlapArea(col.bounds.min, col.bounds.max, 1 << LayerMask.NameToLayer("Ground"));
		transform.position = originalPos;
		return check;
	}
}
