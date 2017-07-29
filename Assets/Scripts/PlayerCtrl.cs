using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
	//movement stuff
	public float speedLimit, speedMplier, speedDecay, reverseSpeed;
	private float hSpeed, vSpeed, currentSpeed;
	private Vector2 move;
	private Vector3 previousPos;
	private float distanceTravelled, distanceTimer;
	private Vector3 rotation = Vector3.zero;
	public float rotationSpeed;
	private Vector2 rotationAffectedMovement;
	public bool Reversing
	{
		get
		{
			return (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)
				|| Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) &&
				vSpeed == 0 && hSpeed == 0;
		}
	}

	//collision stuff
	private float accuracy = 0.01f;
	private BoxCollider2D col;

	//light mechanic stuff
	public int batteryCount;
	public List<BatteryCtrl> batteries = new List<BatteryCtrl>();
	public float startingVision = 20;
	private float vision;
	public Transform visionBlock;

	//animation stuff
	public Transform spr;
	private SpriteRenderer sprRend;
	public Sprite[] sprites = new Sprite[3];

	//game state stuff
	public bool isPaused;
	public int gameState = 0;
	public LevelGenerator levelGen;
	public RecordChecker recordChecker;


	void Start()
	{
		col = GetComponent<BoxCollider2D>();
		sprRend = spr.GetComponent<SpriteRenderer>();
		vision = startingVision;
		levelGen.CreateAll();
	}

	void Update()
	{
		switch (gameState)
		{
		//Menu mode
		case 0:
			{
				CheckMenuInput();
				visionBlock.transform.localScale = Vector3.one;
				break;
			}
		//Gameplay mode
		case 1:
			{
				//take input
				CheckPlayingInput();

				if (!isPaused)
				{
					//movement and collisions
					DetermineMovement();
					ApplyMovement();

					//check distance travelled
					UpdateDistanceTravelled();

					//light mechanic
					UpdateLight();
				}

				//check to see if you are picking up a battery
				CheckForPickups();

				break;
			}
		//Ending
		case 2:
			{
				visionBlock.transform.localScale = Vector3.one;
				if (Input.GetKeyDown(KeyCode.Space))
				{
					gameState = 0;
				}
				break;
			}
		}
	}

	private void CheckMenuInput()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			gameState++;
			vision = startingVision;
			distanceTravelled = 0f;
			distanceTimer = 0f;
			hSpeed = 0;
			vSpeed = 0;
			move = Vector2.zero;
			previousPos = Vector2.zero;
			rotation = Vector3.zero;
			rotationAffectedMovement = Vector2.zero;
			batteryCount = 1;
			recordChecker.recordBeaten = false;
			spr.transform.eulerAngles = Vector3.zero;
			transform.position = Vector3.zero;
		}
	}

	private void UpdateDistanceTravelled()
	{
		distanceTimer += Time.deltaTime;

		if (previousPos != transform.position)
		{
			distanceTravelled += Vector3.Distance(previousPos, transform.position);
			previousPos = transform.position;
		}

		if (distanceTimer >= 30f)
		{
			distanceTimer = 0f;
			SetRecord();
		}
	}

	private void SetRecord()
	{
		if (PlayerPrefs.GetInt("DistanceRecord", 0) < (int)distanceTravelled)
		{
			PlayerPrefs.SetInt("DistanceRecord", (int)distanceTravelled);
			recordChecker.UpdateRecord();
			recordChecker.recordBeaten = true;
		}
	}

	private void UpdateLight()
	{
		//reduce vision
		vision -= Time.deltaTime * currentSpeed * 10f;

		//use battery if run out of vision
		if (vision <= 0)
		{
			if (!UseBattery())
			{
				//end game
				SetRecord();
				gameState++;
				batteries.Clear();
				levelGen.CreateAll();
			}
		}

		//draw vision circle
		visionBlock.localScale = Vector3.MoveTowards(visionBlock.localScale, Vector3.one * (vision / startingVision) * 100F, Time.deltaTime * 80f);
		if (visionBlock.localScale.x < 1f)
		{
			visionBlock.localScale = Vector3.one;
		}
		visionBlock.transform.position = transform.position;
	}

	private void CheckForPickups()
	{
		BatteryCtrl batteryToRemove = null;
		foreach (BatteryCtrl battery in batteries)
		{
			if (col.bounds.Intersects(battery.col.bounds))
			{
				batteryCount++;
				batteryToRemove = battery;
				batteryToRemove.Collect();
			}
		}
		if (batteryToRemove != null)
		{
			batteries.Remove(batteryToRemove);
		}
	}

	private bool UseBattery()
	{
		if (batteryCount > 0)
		{
			batteryCount--;
			vision = startingVision;
			return true;
		}
		return false;
	}

	private void CheckPlayingInput()
	{
		if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
		{
			isPaused = !isPaused;
			visionBlock.transform.localScale = isPaused ? Vector3.one : Vector3.one * (vision / startingVision) * 100F;
		}

		if (!isPaused)
		{
			//movement input
			if (!Reversing)
			{
				if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
				{
					hSpeed -= Time.deltaTime * speedMplier;
				}
				if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
				{
					hSpeed += Time.deltaTime * speedMplier;
				}
			}
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			{
				vSpeed += Time.deltaTime * speedMplier;
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				vSpeed -= Time.deltaTime * speedMplier;
			}

			//rotation control
			if (vSpeed != 0 || hSpeed != 0)
			{
				float angle = Vector2.Angle(Vector2.up, new Vector2(hSpeed, vSpeed));
				if (hSpeed < 0)
				{
					angle = 180 + (180 - angle);
				}
				angle = -angle;
				if (angle - rotation.z > 180f)
				{
					rotation.z += 360;
				}
				if (rotation.z - angle > 180)
				{
					rotation.z -= 360;
				}
				rotation.z = Mathf.MoveTowards(rotation.z, angle, Time.deltaTime * rotationSpeed * currentSpeed);
				spr.transform.eulerAngles = rotation;
			}

			rotationAffectedMovement.x = Mathf.Sin(-rotation.z * Mathf.Deg2Rad) * currentSpeed;
			rotationAffectedMovement.y = Mathf.Cos(-rotation.z * Mathf.Deg2Rad) * currentSpeed;

			//use a battery
			if (Input.GetKeyDown(KeyCode.Space))
			{
				UseBattery();
			}
		}
	}

	private void DetermineMovement()
	{
		//decay speed
		vSpeed = Mathf.MoveTowards(vSpeed, 0, Time.deltaTime * speedDecay);
		hSpeed = Mathf.MoveTowards(hSpeed, 0, Time.deltaTime * speedDecay);

		//limit speed
		Vector2 speedCheck = new Vector2(hSpeed, vSpeed);
		currentSpeed = Vector2.Distance(Vector2.zero, speedCheck);
		if (currentSpeed > speedLimit)
		{
			currentSpeed = speedLimit;
			speedCheck.Normalize();
			speedCheck *= speedLimit;
			hSpeed = speedCheck.x;
			vSpeed = speedCheck.y;
		}

		if (Reversing)
		{
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				rotation.z -= reverseSpeed;
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				rotation.z += reverseSpeed;
			}
			spr.transform.eulerAngles = rotation;
			rotation = spr.transform.eulerAngles;
			float angle = rotation.z;
			if (angle > 0f)
			{
				angle = 180f + (180f - angle);
			}
			angle = Mathf.Abs(angle);
			move.x = -Mathf.Sin(Mathf.Deg2Rad * angle) * Time.deltaTime * reverseSpeed;
			move.y = -Mathf.Cos(Mathf.Deg2Rad * angle) * Time.deltaTime * reverseSpeed;
			vision -= Time.deltaTime;
		}
		else
		{
			//add rotational movement
			move += rotationAffectedMovement;
		}

		rotationAffectedMovement = Vector2.zero;
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
