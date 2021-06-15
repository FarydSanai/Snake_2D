using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUpdate : MonoBehaviour
{
    public ViewportHandler handler;
    // Update is called once per frame
    private void Start()
    {
        handler = this.gameObject.GetComponent<ViewportHandler>();
        handler.UnitsSize = 45f;
    }
    void Update()
    {
        if (Screen.orientation == ScreenOrientation.Portrait ||
            Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            handler.constraint = ViewportHandler.Constraint.Portrait;
        }
        else
        {
            handler.constraint = ViewportHandler.Constraint.Landscape;
        }
    }
}
