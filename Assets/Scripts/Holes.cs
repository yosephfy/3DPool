using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holes : MonoBehaviour
{
    public static Holes Instance;
    private GameObject ball;

    public string name;
    private Vector3 rackEnterance = new Vector3();
    public List<bool> isMoving = new List<bool>(15);

    private void Awake()
    {
        //if (Instance)
        //    Destroy(Instance);
        Instance = this;

        rackEnterance = new Vector3(9.71f, 4.111f, 3.415f);
        for (int i = 0; i < 15; i++)
            isMoving.Add(false);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            ball = collision.gameObject;
            PutBallsInRack();
            //do
            //{
            //    Invoke(nameof(PutBallsInRack), 2);
            //} while (CheckIfBallCanRoll(rackEnterance) == false);

            name = collision.collider.name;

            Debug.Log("Ball " + ReturnBallsInHole(name) + " in");
            isMoving[ReturnBallsInHole(name) - 1] = true;
        }
    }

    public List<bool> GetIsIn()
    {
        return isMoving;
    }

    public void PutBallsInRack()
    {
        ball.transform.position = rackEnterance;
    }

    public int ReturnBallsInHole(string name)
    {
        return name switch
        {
            "Ball (1)" => 1,
            "Ball (2)" => 2,
            "Ball (3)" => 3,
            "Ball (4)" => 4,
            "Ball (5)" => 5,
            "Ball (6)" => 6,
            "Ball (7)" => 7,
            "Ball (8)" => 8,
            "Ball (9)" => 9,
            "Ball (10)" => 10,
            "Ball (11)" => 11,
            "Ball (12)" => 12,
            "Ball (13)" => 13,
            "Ball (14)" => 14,
            "Ball (15)" => 15,
            "CueBall" => 0,
            _ => -1,
        };
    }

    public bool CheckIfBallCanRoll(Vector3 entrancepos)
    {
        bool isIn = false;
        for (int j = 1; j < 16; j++)
        {
            Vector3 ballPosition = GameObject.Find("Ball (" + j + ")").transform.position;
            Vector3 minPosition = entrancepos - new Vector3(3, 3, 3);
            Vector3 maxPosition = entrancepos + new Vector3(3, 3, 3);

            for (int i = 0; i < 3 && isIn; ++i)
            {
                if (ballPosition[i] > minPosition[i] || ballPosition[i] < maxPosition[i])
                    isIn = true;
            }
        }
        if (isIn)
        {
            Debug.Log("The ball is inside the area");
            return false;
        }
        else
        {
            Debug.Log("The ball is outside of the area");
            return true;
        }
    }
}