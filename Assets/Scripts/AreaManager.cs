using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaManager : MonoBehaviour
{
	//A�`�[���̃v���C���[���G���A���ɂ��鐔
	private int m_playerAStayNum = 0;
	//B�`�[���̃v���C���[���G���A���ɂ��鐔
	private int m_playerBStayNum = 0;

	//A�`�[���̐l���̃e�L�X�g
	[SerializeField] private Text m_aTeamText;
	//B�`�[���̐l���̃e�L�X�g
	[SerializeField] private Text m_bTeamText;


	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		//�e�L�X�g�̍X�V
		m_aTeamText.text = "A�`�[���F" + m_playerAStayNum.ToString();
		m_bTeamText.text = "B�`�[���F" + m_playerBStayNum.ToString();
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			PlayerController player = other.GetComponent<PlayerController>();

			//A�`�[���̐��𑝂₷
			if (player.GetTeam() == PlayerController.eTEAM.A)
				m_playerAStayNum += 1;
			//B�`�[���̐��𑝂₷
			if (player.GetTeam() == PlayerController.eTEAM.B)
				m_playerBStayNum += 1;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			PlayerController player = other.GetComponent<PlayerController>();

			//A�`�[���̐������炷
			if (player.GetTeam() == PlayerController.eTEAM.A)
			{
				m_playerAStayNum--;
				if (m_playerAStayNum < 0)
					m_playerAStayNum = 0;
			}
			//B�`�[���̐������炷
			if (player.GetTeam() == PlayerController.eTEAM.B)
			{
				m_playerBStayNum--;
				if (m_playerBStayNum < 0)
					m_playerBStayNum = 0;
			}

		}

	}
}
