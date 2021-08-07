using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
	//�v���C���[�̍��W
	[SerializeField] private Transform player;
	//�v���C���[�Ƃ̋���
	[SerializeField] private float distance = 15.0f;

	//������]
	[SerializeField] private Quaternion vRotation;
	//������]
	[SerializeField] public Quaternion hRotation;

	//��]���x
	[SerializeField] private float turnSpeed = 10.0f;

	//��]���x
	[SerializeField] private float turnSpeed_key = 2.0f;

	// Start is called before the first frame update
	void Start()
	{
		//��]�̏�����
		vRotation = Quaternion.Euler(15.0f, 0.0f, 0.0f);        //������]
		hRotation = Quaternion.identity;                        //������]
		transform.rotation = hRotation * vRotation;             //�ŏI�I�ȃJ�����̊p�x

		//���W�̏�����
		transform.position = player.position - transform.rotation * Vector3.forward * distance;
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void LateUpdate()
	{
		//������]�̍X�V
		//if (Input.GetMouseButton(0))
		//hRotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * -turnSpeed, 0);
		//if(Input.GetAxis("Horizontal2") != 0.0f)
			hRotation *= Quaternion.Euler(0, Input.GetAxis("Horizontal2") * -turnSpeed_key, 0);

		//�ŏI�I�ȃJ�����̊p�x
		transform.rotation = hRotation * vRotation;             

		//���W�̍X�V
		transform.position = player.position - transform.rotation * Vector3.forward * distance;

	}
}
