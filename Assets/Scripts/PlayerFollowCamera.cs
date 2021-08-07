using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
	//プレイヤーの座標
	[SerializeField] private Transform player;
	//プレイヤーとの距離
	[SerializeField] private float distance = 15.0f;

	//垂直回転
	[SerializeField] private Quaternion vRotation;
	//水平回転
	[SerializeField] public Quaternion hRotation;

	//回転速度
	[SerializeField] private float turnSpeed = 10.0f;

	//回転速度
	[SerializeField] private float turnSpeed_key = 2.0f;

	// Start is called before the first frame update
	void Start()
	{
		//回転の初期化
		vRotation = Quaternion.Euler(15.0f, 0.0f, 0.0f);        //垂直回転
		hRotation = Quaternion.identity;                        //水平回転
		transform.rotation = hRotation * vRotation;             //最終的なカメラの角度

		//座標の初期化
		transform.position = player.position - transform.rotation * Vector3.forward * distance;
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void LateUpdate()
	{
		//水平回転の更新
		//if (Input.GetMouseButton(0))
		//hRotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * -turnSpeed, 0);
		//if(Input.GetAxis("Horizontal2") != 0.0f)
			hRotation *= Quaternion.Euler(0, Input.GetAxis("Horizontal2") * -turnSpeed_key, 0);

		//最終的なカメラの角度
		transform.rotation = hRotation * vRotation;             

		//座標の更新
		transform.position = player.position - transform.rotation * Vector3.forward * distance;

	}
}
