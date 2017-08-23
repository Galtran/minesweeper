using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClickSaveButton() {
        EventController.Instance.SaveGame();
    }

    public void onClickLoadButton() {
        EventController.Instance.LoadGame("last");
    }

    public void onClickLoadBeginnersButton()
    {
        EventController.Instance.LoadGame("beginners");
    }
}
