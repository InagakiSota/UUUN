using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using SoftGear.Strix.Unity.Runtime;

//����
//�W�����v�FA
//����FL
//�E���g�F


public class PlayerController : StrixBehaviour
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

	//�W�����v���͂̃t���O
	private bool m_isJumpInput;

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

	//�����̓����蔻��p�̃I�u�W�F�N�g
	[SerializeField] private GameObject m_footCollider;
	//�����̓����蔻��̃X�N���v�g
	private FootCollider m_footColliderScript;

	//���O�̃e�L�X�g
	[SerializeField] Text m_nameText;

	//UI�̃L�����o�X
	[SerializeField] Canvas m_canvas;

	//�`�[���̗񋓑�
	public enum eTEAM
	{
		A,
		B,
	};
	//�`�[���̕ϐ�
	[SerializeField] eTEAM m_team;
		



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

		m_isJumpInput = false;

		m_footColliderScript = m_footCollider.GetComponent<FootCollider>();


	}

	// Update is called once per frame
	void Update()
	{
		if (!isLocal)
			return;

		//�ړ�
		Move();
		//�W�����v
		Jump();
		//�E���g1�̏���
		Ult1();
		//�V���b�g
		Shot();

		//�ڒn�t���O�𑫌��̓����蔻�肩��擾����
		m_isLanding = m_footColliderScript.GetIsLanding();

		//�ڒn���̓W�����v���̈ړ��ʂ��[���ɂ���
		if (m_isLanding == true)
			m_jumpVel = Vector3.zero;

		//�e�L�X�g�X�V
		m_hpText.text = "HP:" + m_hp.ToString();
		m_ultText.text = "Ult:" + m_ult.ToString();


		var strixNetwork = StrixNetwork.instance;


		//m_nameText.text = strixNetwork.selfRoomMember.GetName();
		//m_canvas.transform.rotation = Camera.main.transform.rotation;

	}

	private void FixedUpdate()
	{
		if (!isLocal)
			return;

		//�����ꂩ�̕����Ɉړ����Ă���ꍇ
		if (m_vel.magnitude > 0.0f || m_isShot == true)
		{
			Vector3 vel = (m_isAvoid == true ? m_avoidVel : m_vel);


			//�ړ��ʂ����W�ɉ��Z
			//m_rb.MovePosition(m_rb.position + vel);
			if (m_isLanding == true)
				m_rb.AddForce(vel * 1100.0f, ForceMode.Force);
			else
				m_rb.AddForce(vel * 500.0f, ForceMode.Force);


			//�ړ������ɉ�]
			//var targetRotatin = Quaternion.LookRotation(vel, Vector3.up);

			var targetRotatin = m_isShot == false ?
				Quaternion.LookRotation(vel, Vector3.up) :
				Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

			//��]�X�s�[�h
			var rotationSpeed = m_isShot == false ? 600 * Time.deltaTime : 1800 * Time.deltaTime;



			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotatin, rotationSpeed);
		}

		//�W�����v
		if (m_isJumpInput == true)
		{
			m_rb.AddForce(0.0f, JUMP_FORCE, 0.0f, ForceMode.Impulse);
			//�W�����v�����u�Ԃ̈ړ��ʂ��擾
			m_jumpVel = m_vel;
			m_isJumpInput = false;
		}


	}

	private void OnTriggerStay(Collider other)
	{
	}

	private void OnTriggerExit(Collider other)
	{
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


	}

	//�W�����v
	private void Jump()
	{
		//�X�y�[�X�L�[�ŃW�����v
		if (Input.GetButtonDown("Jump") && m_isLanding == true && m_isAvoid == false && m_isJumpInput == false)
		{
			m_isJumpInput = true;
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

	}

	//�`�[���̐ݒ�
	public void SetTeam(eTEAM team)
	{
		m_team = team;
	}

	//�`�[���̎擾
	public eTEAM GetTeam()
	{
		return m_team;
	}

	//�V���b�g�t���O�̎擾
	public bool GetIsShot()
	{
		return m_isShot;
	}

}