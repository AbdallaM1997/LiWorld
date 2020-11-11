using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoScene : MonoBehaviour {

	//public Animator animator;
	public Text text;
	//public Animator animator;

	private int _index = 0;

	// Use this for initialization
	void Start () {

		ChangeAnimation();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			_index -= 1;
			if (_index < 0)
			{
				_index = 5;
			}
			ChangeAnimation();
		}
		else if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			_index += 1;
			if (_index > 5)
			{
				_index = 0;
			}
			ChangeAnimation();
		}

		
	}

	void ChangeAnimation ()
	{

        	Animator animator1 = GameObject.Find("ghost_tPose").GetComponent<Animator>();
		animator1.SetInteger("index", _index);

		//animator1 = GameObject.Find("girl/clothingSet_01_body").GetComponent<Animator>();
		//animator1.SetInteger("index", _index);

		string name = "";
		switch (_index)
		{
			case 0:
				name = "idle";
				break;
			case 1:
				name = "walk";
				break;
			case 2:
				name = "run";
				break;
			case 3:
				name = "attack";
				break;
			case 4:
				name = "taking damage";
				break;


		}

		text.text = string.Concat(_index.ToString(), ". ", name);
	}
}
