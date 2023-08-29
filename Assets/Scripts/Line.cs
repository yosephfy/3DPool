using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject RayCastObj;

    private float distance;

    public void LineRender()
    {
        RaycastHit hit;
        if (Physics.Raycast(RayCastObj.transform.position, RayCastObj.transform.forward, out hit))
        {
            if (hit.collider != null)
                transform.localScale = new Vector3(1, 1, distance);

            distance = hit.distance;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        LineRender();
    }
}