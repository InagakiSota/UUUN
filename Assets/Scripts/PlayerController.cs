using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//�ړ�����
	private Vector3 vel;

	//�ړ����x
	[SerializeField] private float moveSpeed = 5.0f;

	//��]�̓K�����x
	[SerializeField] private float applySpeed = 0.2f;

	//�J����
	[SerializeField] private PlayerFollowCamera refCamera;

	private Quaternion rotation;

	private Rigidbody rb;

	//�ڒn�t���O
	private bool isLanding;


	// Start is called before the first frame update
	void Start()
	{
		rb = this.GetComponent<Rigidbody>();
		isLanding = false;
	}

	// Update is called once per frame
	void Update()
	{
		vel = Vector3.zero;

		//�L�[���͂ɂ��ړ�(�n��ɂ���ꍇ)
		if (Input.GetKey(KeyCode.W))
			vel.z += 1.0f;
		if (Input.GetKey(KeyCode.S))
			vel.z -= 1.0f;
		if (Input.GetKey(KeyCode.A))
			vel.x -= 1.0f;
		if (Input.GetKey(KeyCode.D))
			vel.x += 1.0f;


		//�X�y�[�X�L�[�ŃW�����v
		if (Input.GetKeyDown(KeyCode.Space) && isLanding == true)
			rb.AddForce(0.0f, 10.0f, 0.0f, ForceMode.Impulse);

		//�J�����̑O�����v���C���[�̈ړ������ɂ���
		var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

		//�t���[�����ƂɈړ��ʉ��Z
		//if (isLanding == true) moveSpeed = 15.0f;
		//else moveSpeed = 1.0f;
		//�󒆂ɂ���ꍇ�͈ړ��ʂ����炷

		vel = horizontalRotation * vel.normalized * moveSpeed * Time.deltaTime;

		//transform.rotation = refCamera.hRotation;

		//�����ꂩ�̕����Ɉړ����Ă���ꍇ
		if (vel.magnitude > 0.0f )
		{
			//�ړ������ɉ�]
			transform.rotation = Quaternion.LookRotation(vel, Vector3.up);
			//�ړ��ʂ����W�ɉ��Z
			transform.position += refCamera.hRotation * vel;
		}

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
}
