using MineSweeperCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour {

    private Cell _cell;
    private Transform myTransform;


    public TextMesh text;

    public delegate void onClickHandler(bool isLeft);
    public event onClickHandler onCellClick;


    // Use this for initialization
    void Start () {
        myTransform = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        text.text = "";

        if (_cell.IsNoneType)
        {
            text.text = "#";
        }
        else if (_cell.IsMark)
        {
            text.text = "Ф";
        }
        else if (_cell.IsOpen)
        {
            if(_cell.TypeCell == Cell.CellType.ctMINE)
            {
                text.text = "*";
            } else
            {
                text.text = _cell.NeighborsMine.ToString();
            }
            
        }
    }

    public void InitController(Cell c)
    {
        _cell = c;
    }

    void OnMouseOver()
    {
        myTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("HEHEY !!");
            onCellClick(true);
        } 
        else if (Input.GetMouseButtonDown(1))
        {
            onCellClick(false);
        }
    }

    private void OnMouseExit()
    {
        myTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    void OnGUI()
    {
        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));

        Camera camera = Camera.main;

        if (_cell.IsOpen)
        {
            Vector3 pos = camera.WorldToScreenPoint(myTransform.position);
            //GUI.Label(new Rect(pos.x, pos.y, 50, 50), "!!!");
        }
        

        GUI.EndGroup();
    }
}
