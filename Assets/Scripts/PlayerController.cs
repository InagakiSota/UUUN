using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����
//�W�����v�FA
//�u�[�X�g�FL
//����F


public class PlayerController : MonoBehaviour
{
	//�ړ�����
	private Vector3 vel;

	//�ړ����x
	[SerializeField] private float MOVE_SPEED = 5.0f;

	//�J����
	[SerializeField] private PlayerFollowCamera refCamera;

	private Quaternion rotation;

	private Rigidbody rb;

	//�ڒn�t���O
	private bool isLanding;

	//�̗�
	public int life;
	//�ő�̗�
	[SerializeField] private int LIFE_MAX = 100;

	//����̃X�s�[�h
	[SerializeField] private float AVOID_SPEED = 25.0f;

	//����̃t���O
	private bool isAvoid = false;

	//����̃^�C�}�[
	private float avoidTimer = 0.0f;

	//�������
	[SerializeField] private float AVOID_TIME = 1.0f;

	// Start is called before the first frame update
	void Start()
	{
		rb = this.GetComponent<Rigidbody>();
		isLanding = false;

		//�̗͂̏�����
		life = LIFE_MAX;

		Physics.gravity = new Vector3(0.0f, -15.0f, 0.0f);
	}

	// Update is called once per frame
	void Update()
	{
		//�ړ�
		Move();
		//�W�����v
		Jump();
	}

	private void OnTriggerEnter(Collider other)
	{
		//�ڒn�t���O�𗧂Ă�
		isLanding = true;
	}

	private void OnTriggerExit(Collider other)
	{
		//�ڒn�t���O������
		isLanding = false;
	}

	//�ړ�
	private void Move()
	{
		if (isAvoid == false)
		{
			vel = Vector3.zero;

			vel.x = -Input.GetAxis("Horizontal");
			vel.z = Input.GetAxis("Vertical");
		}

		//�J�����̑O�����v���C���[�̈ړ������ɂ���
		var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

		vel = horizontalRotation * vel.normalized * MOVE_SPEED * Time.deltaTime;

		//LB���͂ŉ���t���O�𗧂Ă�
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

		//�����ꂩ�̕����Ɉړ����Ă���ꍇ
		if (vel.magnitude > 0.0f)
		{
			//�ړ������ɉ�]
			transform.rotation = Quaternion.LookRotation(vel, Vector3.up);
			//�ړ��ʂ����W�ɉ��Z
			//�ʏ�ړ�
			if(isAvoid == false)
				transform.position += vel;
			else
				transform.position += vel * AVOID_SPEED;

		}
	}

	//�W�����v
	private void Jump()
	{
		//�X�y�[�X�L�[�ŃW�����v
		if (Input.GetButtonDown("Jump") && isLanding == true)
			rb.AddForce(0.0f, 15.0f, 0.0f, ForceMode.Impulse);

	}

	//���
	private void Avoidance()
	{

	}
}
