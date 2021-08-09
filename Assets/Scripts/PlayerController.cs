using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

//メモ
//ジャンプ：A
//回避：L
//ウルト：


public class PlayerController : MonoBehaviour
{
	//移動方向
	private Vector3 m_vel;

	//移動速度の定数
	[SerializeField] private float MOVE_SPEED = 5.0f;
	//移動速度の変数
	private float m_moveSpeed = 0.0f;

	//移動量の入力受け取り
	private Vector3 m_inputVel = Vector3.zero;

	private Rigidbody m_rb;

	//接地フラグ
	private bool m_isLanding;

	//ジャンプ力
	[SerializeField] private float JUMP_FORCE = 15.0f;

	//ジャンプ中の移動量
	private Vector3 m_jumpVel;

	//体力
	public int m_hp;
	//最大体力
	[SerializeField] private int HP_MAX = 100;

	//回避で受けるダメージ量
	[SerializeField] private int AVOID_DAMAGE = 10;

	//回避のスピード
	[SerializeField] private float AVOID_SPEED = 25.0f;

	//回避のフラグ
	private bool m_isAvoid = false;

	//回避のタイマー
	private float m_avoidTimer = 0.0f;

	//回避時間
	[SerializeField] private float AVOID_TIME = 1.0f;

	//回避用の移動量
	private Vector3 m_avoidVel;


	//カメラのタイマー
	private float m_cameraTimer;
	//カメラのリセットにかかる時間
	[SerializeField] private float CAMERA_RESET_TIME;

	//体力のテキスト
	[SerializeField] Text m_hpText;

	//ウルト
	private float m_ult;

	//ウルトのゲージ
	[SerializeField] Text m_ultText;

	//ウルトの最大値
	[SerializeField] private float ULT_MAX;

	//ウルト1発動中のフラグ
	private bool m_isUlt1Use;

	//発射のフラグ
	private bool m_isShot;


	// Start is called before the first frame update
	void Start()
	{
		m_rb = this.GetComponent<Rigidbody>();
		m_isLanding = false;

		//体力の初期化
		m_hp = HP_MAX;

		Physics.gravity = new Vector3(0.0f, -150.0f, 0.0f);

		m_isUlt1Use = false;

		m_moveSpeed = MOVE_SPEED;

		m_isShot = false;
	}

	// Update is called once per frame
	void Update()
	{
		//移動
		Move();
		//ジャンプ
		Jump();
		//ウルト1の処理
		Ult1();
		//ショット
		Shot();

		//テキスト更新
		m_hpText.text = "HP:" + m_hp.ToString();
		m_ultText.text = "Ult:" + m_ult.ToString();

	}

	private void FixedUpdate()
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		//接地フラグを立てる
		m_isLanding = true;

		m_jumpVel = Vector3.zero;
	}

	private void OnTriggerExit(Collider other)
	{
		//接地フラグを消す
		m_isLanding = false;

	}

	//移動
	private void Move()
	{
		m_inputVel = Vector3.zero;

		m_inputVel.x = -Input.GetAxis("Horizontal");
		m_inputVel.z = Input.GetAxis("Vertical");


		//カメラの前方をプレイヤーの移動方向にする
		var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

		m_vel = horizontalRotation.normalized * m_inputVel.normalized * m_moveSpeed * Time.deltaTime;

		//m_vel = m_isLanding == true ? m_vel : m_vel / 1.5f;

		//LB入力で回避フラグを立てる
		if (Input.GetButtonDown("Avoid") &&
			m_isLanding == true && 
			m_isAvoid == false &&
			m_hp > AVOID_DAMAGE)
		{
			m_isAvoid = true;
			//移動量を保存
			m_avoidVel = m_vel * AVOID_SPEED;
			//体力を減らす
			if(m_isUlt1Use == false)
				m_hp -= AVOID_DAMAGE;
		}

		//回避フラグが立ったら一定時間回避
		if (m_isAvoid == true)
		{
			m_avoidTimer += Time.deltaTime;

			if(m_avoidTimer >= AVOID_TIME)
			{
				m_avoidTimer = 0.0f;
				m_isAvoid = false;
				m_rb.velocity = Vector3.zero;
			}
		}
		else
		{
			m_avoidVel = Vector3.zero;
		}

		//いずれかの方向に移動している場合
		if (m_vel.magnitude > 0.0f)
		{
			Vector3 vel = (m_isAvoid == true ? m_avoidVel : m_vel);

			//回転スピード
			var rotationSpeed = 600 * Time.deltaTime;

			//移動量を座標に加算
			//m_rb.MovePosition(m_rb.position + vel);
			if(m_isLanding == true)
				m_rb.AddForce(vel * 1100.0f, ForceMode.Force);
			else
				m_rb.AddForce(vel * 500.0f, ForceMode.Force);


			Debug.Log(vel);
			//移動方向に回転
			//var targetRotatin = Quaternion.LookRotation(vel, Vector3.up);

			var targetRotatin = m_isShot == false ? 
				Quaternion.LookRotation(vel, Vector3.up) : 
				Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotatin, rotationSpeed);
		}

	}

	//ジャンプ
	private void Jump()
	{
		//スペースキーでジャンプ
		if (Input.GetButtonDown("Jump") && m_isLanding == true && m_isAvoid == false)
		{
			m_rb.AddForce(0.0f, JUMP_FORCE, 0.0f, ForceMode.Impulse);
			//ジャンプした瞬間の移動量を取得
			m_jumpVel = m_vel;
		}

	}

	//ウルト1の処理
	private void Ult1()
	{

		//ウルトゲージがマックスならウルト発動
		if (Input.GetButtonDown("Ult") && m_ult >= ULT_MAX && m_isUlt1Use == false)
		{
			m_isUlt1Use = true;
			//体力半減
			m_hp = m_hp / 2;
			//移動速度UP
			m_moveSpeed = MOVE_SPEED * 1.5f;
		}

		//ウルト発動中はゲージを減らす
		if (m_isUlt1Use == true)
		{
			m_ult -= Time.deltaTime;
			
			if (m_ult <= 0.0f)
			{
				//体力を元に戻す
				m_hp *= 2;
				m_ult = 0.0f;
				m_isUlt1Use = false;
				//移動速度を元に戻す
				m_moveSpeed = MOVE_SPEED;
			}
		}
		//ウルトの増加
		if (m_isUlt1Use == false)
		{
			m_ult += Time.deltaTime;
			if (m_ult >= ULT_MAX)
				m_ult = ULT_MAX;
		}

	}

	//ショット
	private void Shot()
	{
		if (Input.GetAxis("Shot") > 0.0f)
		{
			m_isShot = true;
		}
		else m_isShot = false;

		Debug.Log(m_isShot);
	}


}