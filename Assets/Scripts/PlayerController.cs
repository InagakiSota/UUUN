using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//移動方向
	private Vector3 vel;

	//移動速度
	[SerializeField] private float moveSpeed = 5.0f;

	//回転の適応速度
	[SerializeField] private float applySpeed = 0.2f;

	//カメラ
	[SerializeField] private PlayerFollowCamera refCamera;

	private Quaternion rotation;

	private Rigidbody rb;

	//接地フラグ
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

		//キー入力による移動(地上にいる場合)
		if (Input.GetKey(KeyCode.W))
			vel.z += 1.0f;
		if (Input.GetKey(KeyCode.S))
			vel.z -= 1.0f;
		if (Input.GetKey(KeyCode.A))
			vel.x -= 1.0f;
		if (Input.GetKey(KeyCode.D))
			vel.x += 1.0f;


		//スペースキーでジャンプ
		if (Input.GetKeyDown(KeyCode.Space) && isLanding == true)
			rb.AddForce(0.0f, 10.0f, 0.0f, ForceMode.Impulse);

		//カメラの前方をプレイヤーの移動方向にする
		var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

		//フレームごとに移動量加算
		//if (isLanding == true) moveSpeed = 15.0f;
		//else moveSpeed = 1.0f;
		//空中にいる場合は移動量を減らす

		vel = horizontalRotation * vel.normalized * moveSpeed * Time.deltaTime;

		//transform.rotation = refCamera.hRotation;

		//いずれかの方向に移動している場合
		if (vel.magnitude > 0.0f )
		{
			//移動方向に回転
			transform.rotation = Quaternion.LookRotation(vel, Vector3.up);
			//移動量を座標に加算
			transform.position += refCamera.hRotation * vel;
		}

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
}
