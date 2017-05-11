using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour {

    private bool _selected = false;

    public bool Selected
    {
        set { _selected = value; }
        get { return _selected; }
    }

    private void CheckSelected()
    {
        if (_selected)
        {
            gameObject.GetComponent<Image>().color = Color.grey;
        }

        if (!_selected)
        {
            gameObject.GetComponent<Image>().color = Color.white;
        }
    }

    private void Update()
    {
        CheckSelected();
    }
}
