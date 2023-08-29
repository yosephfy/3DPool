using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllBallsScript : MonoBehaviour
{
    public List<GameObject> all_balls = new List<GameObject>();

    // Start is called before the first frame update

    private void Awake()
    {
    }

    private bool is_moving_balls = true;

    // Update is called once per frame
    private void Update()
    {
        for (int i = 0; i < 15; i++)
        {
            //if (all_balls[i].GetComponent<NormalBallScript>().IsMoving == false)
            //if (all_balls[i].GetComponent<NormalBallScript>().IsMoving == false)
            //Debug.Log("IT WORKED");
            is_moving_balls = false;
        }
    }
}