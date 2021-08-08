using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//メモ
//ジャンプ：A
//ブースト：L
//回避：


public class PlayerController : MonoBehaviour
{
	//移動方向
	private Vector3 vel;

	//移動速度
	[SerializeField] private float MOVE_SPEED = 5.0f;

	//カメラ
	[SerializeField] private PlayerFollowCamera refCamera;

	private Quaternion rotation;

	private Rigidbody rb;

	//接地フラグ
	private bool isLanding;

	//体力
	public int life;
	//最大体力
	[SerializeField] private int LIFE_MAX = 100;

	//回避のスピード
	[SerializeField] private float AVOID_SPEED = 25.0f;

	//回避のフラグ
	private bool isAvoid = false;

	//回避のタイマー
	private float avoidTimer = 0.0f;

	//回避時間
	[SerializeField] private float AVOID_TIME = 1.0f;

	// Start is called before the first frame update
	void Start()
	{
		rb = this.GetComponent<Rigidbody>();
		isLanding = false;

		//体力の初期化
		life = LIFE_MAX;

		Physics.gravity = new Vector3(0.0f, -15.0f, 0.0f);
	}

	// Update is called once per frame
	void Update()
	{
		//移動
		Move();
		//ジャンプ
		Jump();
	}

	private void OnTriggerEnter(Collider other)
	{
		//接地フラグを立てる
		isLanding = true;
	}

	private void OnTriggerExit(Collider other)
	{
		//接地フラグを消す
		isLanding = false;
	}

	//移動
	private void Move()
	{
		if (isAvoid == false)
		{
			vel = Vector3.zero;

			vel.x = -Input.GetAxis("Horizontal");
			vel.z = Input.GetAxis("Vertical");
		}

		//カメラの前方をプレイヤーの移動方向にする
		var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

		vel = horizontalRotation * vel.normalized * MOVE_SPEED * Time.deltaTime;

		//LB入力で回避フラグを立てる
		if (Input.GetButtonDown("Avoid") && isLanding == true)
			isAvoid = true;

		if(isAvoid == true)
		{
			avoidTimer += Time.deltaTime;

			if(avoidTimer >= AVOID_TIME)
			{
				avoidTimer = 0.0f;
				isAvoid = false;
			}
		}

		//いずれかの方向に移動している場合
		if (vel.magnitude > 0.0f)
		{
			//移動方向に回転
			transform.rotation = Quaternion.LookRotation(vel, Vector3.up);
			//移動量を座標に加算
			//通常移動
			if(isAvoid == false)
				transform.position += vel;
			else
				transform.position += vel * AVOID_SPEED;

		}
	}

	//ジャンプ
	private void Jump()
	{
		//スペースキーでジャンプ
		if (Input.GetButtonDown("Jump") && isLanding == true)
			rb.AddForce(0.0f, 15.0f, 0.0f, ForceMode.Impulse);

	}

	//回避
	private void Avoidance()
	{

	}
}
