using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class InstructionScript : MonoBehaviour {

	public int panelNum;

	public GameObject Instructions;
	public GameObject btnNext;
	public GameObject btnBack;
	public GameObject btnMenu;

	public GameObject[] panel;

	// Use this for initialization
	void Start () {

		Instructions = GameObject.Find ("Instructions"); 
		btnNext = Instructions.transform.FindChild("NextButton").gameObject;
		btnBack = Instructions.transform.FindChild("BackButton").gameObject;
		btnMenu = Instructions.transform.FindChild ("MenuButton").gameObject;

		panel = new GameObject [5];
		panel [0] = Instructions.transform.FindChild("Panel0").gameObject;
		panel [1] = Instructions.transform.FindChild("Panel1").gameObject;
		panel [2] = Instructions.transform.FindChild("Panel2").gameObject;
		panel [3] = Instructions.transform.FindChild("Panel3").gameObject;
		panel [4] = Instructions.transform.FindChild("Panel4").gameObject;

		panelNum = 0;
		updateButton ();
		SwitchPanel (panelNum);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void buttonNext(){
		if (panelNum < 5)
			panelNum++;
		updateButton ();
		SwitchPanel (panelNum);
	}

	public void buttonBack(){
		if (panelNum > 0)
			panelNum--;
		updateButton ();
		SwitchPanel (panelNum);
	}

	public void buttonMenu(){
		SceneManager.LoadScene ("Menu");
	}

	public void updateButton(){
		if (panelNum == 0)
			btnBack.SetActive (false);
		else
			btnBack.SetActive (true);

		if (panelNum == 4)
			btnNext.SetActive (false);
		else
			btnNext.SetActive (true);
	}

	void SwitchPanel(int num){
		for (int i = 0; i < panel.Length; i++) {
			if (i == num)
				panel [i].SetActive (true);
			else
				panel [i].SetActive (false);
		}
	}
}
