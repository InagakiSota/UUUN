using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
	//弾丸のプレハブ
	[SerializeField] private GameObject m_bulletPrefab;

	//マズル
	[SerializeField] private Transform m_muzzle;

	//プレイヤー
	[SerializeField] private GameObject m_player;

	//プレイヤーのスクリプト
	private PlayerController m_playerController;

	//発射間隔のタイマー
	private float m_shotIntervalTimer = 0.0f;

	//発射間隔
	[SerializeField] private float SHOT_INTERVAL = 1.0f;

	// Start is called before the first frame update
	void Start()
	{
		m_playerController = m_player.GetComponent<PlayerController>();
	}

	// Update is called once per frame
	void Update()
	{
		//プレイヤーの発射フラグが立ったら
		if (m_playerController.GetIsShot() == true)
		{
			if (m_shotIntervalTimer <= 0.0f)
			{
				//弾丸のインスタンス生成
				GameObject bullet = Instantiate(m_bulletPrefab, m_muzzle);
				bullet.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 100.0f, ForceMode.Impulse);

				m_shotIntervalTimer = SHOT_INTERVAL;
			}
			m_shotIntervalTimer -= Time.deltaTime;
		}
		else
			m_shotIntervalTimer = 0.0f;
	}
}
