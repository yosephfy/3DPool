using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball2 : MonoBehaviour
{
    public static Ball2 Instance;

    private void Awake()
    {
        if (Instance)
            Destroy(Instance);

        Instance = this;

        //For good measure, set the previous locations
        for (int i = 0; i < previousLocations.Length; i++)
        {
            previousLocations[i] = Vector3.zero;
        }

        // isMoving = false;
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    #region

    //Set this to the transform you want to check
    // private Transform objectTransfom;

    private float noMovementThreshold = 0.0001f;
    private const int noMovementFrames = 3;
    private Vector3[] previousLocations = new Vector3[noMovementFrames];
    private bool isMoving;

    //Let other scripts see if the object is moving
    public bool IsMoving
    {
        get { return isMoving; }
    }

    private void Update()
    {
        //Store the newest vector at the end of the list of vectors
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            previousLocations[i] = previousLocations[i + 1];
        }
        previousLocations[previousLocations.Length - 1] = transform.position;

        //Check the distances between the points in your previous locations
        //If for the past several updates, there are no movements smaller than the threshold,
        //you can most likely assume that the object is not moving
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            if (Vector3.Distance(previousLocations[i], previousLocations[i + 1]) >= noMovementThreshold)
            {
                //The minimum movement has been detected between frames
                isMoving = true;
                break;
            }
            else
            {
                isMoving = false;
            }
        }

        if (isMoving == true)
        {
            Debug.Log("Ball " + gameObject.name + " is moving");
        }
        else
        {
        }
    }

    public bool GetIsNormalBallsMoving()
    {
        return IsMoving;
    }

    #endregion
}