using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootCollider : MonoBehaviour
{
	//�W�����v�t���O
	private bool m_isLanding;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	//�ڒn�t���O�̎擾
	public bool GetIsLanding()
	{
		return m_isLanding;
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag != "Area")
		{
			//�ڒn�t���O�𗧂Ă�
			m_isLanding = true;

		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag != "Area")
		{

			//�ڒn�t���O������
			m_isLanding = false;

		}
	}

}
