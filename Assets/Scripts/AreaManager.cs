using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaManager : MonoBehaviour
{
	//Aチームのプレイヤーがエリア内にいる数
	private int m_playerAStayNum = 0;
	//Bチームのプレイヤーがエリア内にいる数
	private int m_playerBStayNum = 0;

	//Aチームの人数のテキスト
	[SerializeField] private Text m_aTeamText;
	//Bチームの人数のテキスト
	[SerializeField] private Text m_bTeamText;


	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		//テキストの更新
		m_aTeamText.text = "Aチーム：" + m_playerAStayNum.ToString();
		m_bTeamText.text = "Bチーム：" + m_playerBStayNum.ToString();
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			PlayerController player = other.GetComponent<PlayerController>();

			//Aチームの数を増やす
			if (player.GetTeam() == PlayerController.eTEAM.A)
				m_playerAStayNum += 1;
			//Bチームの数を増やす
			if (player.GetTeam() == PlayerController.eTEAM.B)
				m_playerBStayNum += 1;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			PlayerController player = other.GetComponent<PlayerController>();

			//Aチームの数を減らす
			if (player.GetTeam() == PlayerController.eTEAM.A)
			{
				m_playerAStayNum--;
				if (m_playerAStayNum < 0)
					m_playerAStayNum = 0;
			}
			//Bチームの数を減らす
			if (player.GetTeam() == PlayerController.eTEAM.B)
			{
				m_playerBStayNum--;
				if (m_playerBStayNum < 0)
					m_playerBStayNum = 0;
			}

		}

	}
}
