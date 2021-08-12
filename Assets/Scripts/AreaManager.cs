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

	//エリアのMaterial
	private Renderer m_material;

	//制圧状態
	enum eSUPPRESION_STATE
	{
		NON,		//制圧無し
		TEAM_A,		//Aチーム制圧
		TEAM_B,     //Bチーム制圧
	}

	//制圧状態の変数
	private eSUPPRESION_STATE m_suppresionState;

	//制圧状態のテキスト
	[SerializeField] private Text m_suppresionText;

	// Start is called before the first frame update
	void Start()
	{
		m_suppresionState = eSUPPRESION_STATE.NON;

		m_material = GetComponent<Renderer>();

	}

	// Update is called once per frame
	void Update()
	{

		//テキストの更新
		m_aTeamText.text = "Aチーム：" + m_playerAStayNum.ToString();
		m_bTeamText.text = "Bチーム：" + m_playerBStayNum.ToString();


		//制圧状態の更新
		//Aチーム制圧
		if (m_playerAStayNum > m_playerBStayNum)
		{
			m_suppresionState = eSUPPRESION_STATE.TEAM_A;
			m_suppresionText.text = "エリア：Aチーム制圧";
			m_material.material.color = Color.red;
		}

		//Bチーム制圧
		else if (m_playerBStayNum > m_playerAStayNum)
		{
			m_suppresionState = eSUPPRESION_STATE.TEAM_B;
			m_suppresionText.text = "エリア：Bチーム制圧";
			m_material.material.color = Color.blue;

		}
		//制圧無し
		else
		{
			m_suppresionState = eSUPPRESION_STATE.NON;
			m_suppresionText.text = "エリア：制圧無し";
			m_material.material.color = Color.black;

		}


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
