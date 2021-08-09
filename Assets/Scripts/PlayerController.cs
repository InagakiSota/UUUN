using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

//����
//�W�����v�FA
//����FL
//�E���g�F


public class PlayerController : MonoBehaviour
{
	//�ړ�����
	private Vector3 m_vel;

	//�ړ����x�̒萔
	[SerializeField] private float MOVE_SPEED = 5.0f;
	//�ړ����x�̕ϐ�
	private float m_moveSpeed = 0.0f;

	//�ړ��ʂ̓��͎󂯎��
	private Vector3 m_inputVel = Vector3.zero;

	private Rigidbody m_rb;

	//�ڒn�t���O
	private bool m_isLanding;

	//�W�����v��
	[SerializeField] private float JUMP_FORCE = 15.0f;

	//�W�����v���̈ړ���
	private Vector3 m_jumpVel;

	//�̗�
	public int m_hp;
	//�ő�̗�
	[SerializeField] private int HP_MAX = 100;

	//����Ŏ󂯂�_���[�W��
	[SerializeField] private int AVOID_DAMAGE = 10;

	//����̃X�s�[�h
	[SerializeField] private float AVOID_SPEED = 25.0f;

	//����̃t���O
	private bool m_isAvoid = false;

	//����̃^�C�}�[
	private float m_avoidTimer = 0.0f;

	//�������
	[SerializeField] private float AVOID_TIME = 1.0f;

	//���p�̈ړ���
	private Vector3 m_avoidVel;


	//�J�����̃^�C�}�[
	private float m_cameraTimer;
	//�J�����̃��Z�b�g�ɂ����鎞��
	[SerializeField] private float CAMERA_RESET_TIME;

	//�̗͂̃e�L�X�g
	[SerializeField] Text m_hpText;

	//�E���g
	private float m_ult;

	//�E���g�̃Q�[�W
	[SerializeField] Text m_ultText;

	//�E���g�̍ő�l
	[SerializeField] private float ULT_MAX;

	//�E���g1�������̃t���O
	private bool m_isUlt1Use;

	//���˂̃t���O
	private bool m_isShot;


	// Start is called before the first frame update
	void Start()
	{
		m_rb = this.GetComponent<Rigidbody>();
		m_isLanding = false;

		//�̗͂̏�����
		m_hp = HP_MAX;

		Physics.gravity = new Vector3(0.0f, -150.0f, 0.0f);

		m_isUlt1Use = false;

		m_moveSpeed = MOVE_SPEED;

		m_isShot = false;
	}

	// Update is called once per frame
	void Update()
	{
		//�ړ�
		Move();
		//�W�����v
		Jump();
		//�E���g1�̏���
		Ult1();
		//�V���b�g
		Shot();

		//�e�L�X�g�X�V
		m_hpText.text = "HP:" + m_hp.ToString();
		m_ultText.text = "Ult:" + m_ult.ToString();

	}

	private void FixedUpdate()
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		//�ڒn�t���O�𗧂Ă�
		m_isLanding = true;

		m_jumpVel = Vector3.zero;
	}

	private void OnTriggerExit(Collider other)
	{
		//�ڒn�t���O������
		m_isLanding = false;

	}

	//�ړ�
	private void Move()
	{
		m_inputVel = Vector3.zero;

		m_inputVel.x = -Input.GetAxis("Horizontal");
		m_inputVel.z = Input.GetAxis("Vertical");


		//�J�����̑O�����v���C���[�̈ړ������ɂ���
		var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

		m_vel = horizontalRotation.normalized * m_inputVel.normalized * m_moveSpeed * Time.deltaTime;

		//m_vel = m_isLanding == true ? m_vel : m_vel / 1.5f;

		//LB���͂ŉ���t���O�𗧂Ă�
		if (Input.GetButtonDown("Avoid") &&
			m_isLanding == true && 
			m_isAvoid == false &&
			m_hp > AVOID_DAMAGE)
		{
			m_isAvoid = true;
			//�ړ��ʂ�ۑ�
			m_avoidVel = m_vel * AVOID_SPEED;
			//�̗͂����炷
			if(m_isUlt1Use == false)
				m_hp -= AVOID_DAMAGE;
		}

		//����t���O�����������莞�ԉ��
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

		//�����ꂩ�̕����Ɉړ����Ă���ꍇ
		if (m_vel.magnitude > 0.0f)
		{
			Vector3 vel = (m_isAvoid == true ? m_avoidVel : m_vel);

			//��]�X�s�[�h
			var rotationSpeed = 600 * Time.deltaTime;

			//�ړ��ʂ����W�ɉ��Z
			//m_rb.MovePosition(m_rb.position + vel);
			if(m_isLanding == true)
				m_rb.AddForce(vel * 1100.0f, ForceMode.Force);
			else
				m_rb.AddForce(vel * 500.0f, ForceMode.Force);


			Debug.Log(vel);
			//�ړ������ɉ�]
			//var targetRotatin = Quaternion.LookRotation(vel, Vector3.up);

			var targetRotatin = m_isShot == false ? 
				Quaternion.LookRotation(vel, Vector3.up) : 
				Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotatin, rotationSpeed);
		}

	}

	//�W�����v
	private void Jump()
	{
		//�X�y�[�X�L�[�ŃW�����v
		if (Input.GetButtonDown("Jump") && m_isLanding == true && m_isAvoid == false)
		{
			m_rb.AddForce(0.0f, JUMP_FORCE, 0.0f, ForceMode.Impulse);
			//�W�����v�����u�Ԃ̈ړ��ʂ��擾
			m_jumpVel = m_vel;
		}

	}

	//�E���g1�̏���
	private void Ult1()
	{

		//�E���g�Q�[�W���}�b�N�X�Ȃ�E���g����
		if (Input.GetButtonDown("Ult") && m_ult >= ULT_MAX && m_isUlt1Use == false)
		{
			m_isUlt1Use = true;
			//�̗͔���
			m_hp = m_hp / 2;
			//�ړ����xUP
			m_moveSpeed = MOVE_SPEED * 1.5f;
		}

		//�E���g�������̓Q�[�W�����炷
		if (m_isUlt1Use == true)
		{
			m_ult -= Time.deltaTime;
			
			if (m_ult <= 0.0f)
			{
				//�̗͂����ɖ߂�
				m_hp *= 2;
				m_ult = 0.0f;
				m_isUlt1Use = false;
				//�ړ����x�����ɖ߂�
				m_moveSpeed = MOVE_SPEED;
			}
		}
		//�E���g�̑���
		if (m_isUlt1Use == false)
		{
			m_ult += Time.deltaTime;
			if (m_ult >= ULT_MAX)
				m_ult = ULT_MAX;
		}

	}

	//�V���b�g
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