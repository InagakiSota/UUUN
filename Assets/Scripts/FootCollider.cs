using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootCollider : MonoBehaviour
{
	//ジャンプフラグ
	private bool m_isLanding;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	//接地フラグの取得
	public bool GetIsLanding()
	{
		return m_isLanding;
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag != "Area")
		{
			//接地フラグを立てる
			m_isLanding = true;

		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag != "Area")
		{

			//接地フラグを消す
			m_isLanding = false;

		}
	}

}
