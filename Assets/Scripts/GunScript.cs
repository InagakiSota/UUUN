using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
	//�e�ۂ̃v���n�u
	[SerializeField] private GameObject m_bulletPrefab;

	//�}�Y��
	[SerializeField] private Transform m_muzzle;

	//�v���C���[
	[SerializeField] private GameObject m_player;

	//�v���C���[�̃X�N���v�g
	private PlayerController m_playerController;

	//���ˊԊu�̃^�C�}�[
	private float m_shotIntervalTimer = 0.0f;

	//���ˊԊu
	[SerializeField] private float SHOT_INTERVAL = 1.0f;

	// Start is called before the first frame update
	void Start()
	{
		m_playerController = m_player.GetComponent<PlayerController>();
	}

	// Update is called once per frame
	void Update()
	{
		//�v���C���[�̔��˃t���O����������
		if (m_playerController.GetIsShot() == true)
		{
			if (m_shotIntervalTimer <= 0.0f)
			{
				//�e�ۂ̃C���X�^���X����
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
