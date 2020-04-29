using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics : MonoBehaviour
{
    public GameObject g1;
    public GameObject g2;
    private LineRenderer LineRenderer = new LineRenderer();

    public void Update()
    {
        DrawLine(g1.transform.position, g2.transform.position);
        Debug.Log("Draw Line");
    }

    public void DrawLine(Vector3 v1, Vector3 v2)
    {
        Vector3[] vv = new Vector3[2];
        vv[0] = v1;
        vv[1] = v1;

        LineRenderer.SetPositions(vv);
    }
}
