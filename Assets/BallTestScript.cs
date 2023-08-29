using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTestScript : MonoBehaviour
{
    public static BallTestScript Instance;
    #region
    public GameObject singleBall;
    private List<GameObject> theBalls = new List<GameObject>();
    public List<Material> the_materials;
    public float Ball_Scale;
    private List<bool> movin;

    public Vector3 start_pos;

    private void Awake()
    {
        for (int i = 0; i < theBalls.Count; i++)
            movin.Add(theBalls[i].GetComponent<NormalBallScript>().IsMoving);
    }

    public void SpawnBalls()
    {
        int index = 0;
        for (int c = 0; c < 15; c++)
        {
            int indexplusone = index + 1;
            theBalls.Add(Instantiate(singleBall) as GameObject);
            theBalls[theBalls.Count - 1].GetComponent<NormalBallScript>().SetBallIndex(index + 1);
            theBalls[theBalls.Count - 1].name = "Ball (" + indexplusone + ")";
            theBalls[theBalls.Count - 1].transform.parent = transform;
            theBalls[theBalls.Count - 1].transform.localScale = new Vector3(Ball_Scale, Ball_Scale, Ball_Scale);

            index++;
        }
    }

    public bool AreBallsMoving()
    {
        //if (GameObject.Find("Ball (1)") != null && !Holes.Instance.GetIsIn()[0]) if (GameObject.Find("Ball (1)").GetComponent<Ball1>().IsMoving == true) return true;
        //if (GameObject.Find("Ball (2)") != null && !Holes.Instance.GetIsIn()[1]) if (GameObject.Find("Ball (2)").GetComponent<Ball2>().IsMoving == true) return true;
        //if (GameObject.Find("Ball (3)") != null && !Holes.Instance.GetIsIn()[2]) if (GameObject.Find("Ball (3)").GetComponent<Ball3>().IsMoving == true) return true;
        //if (GameObject.Find("Ball (4)") != null && !Holes.Instance.GetIsIn()[3]) if (GameObject.Find("Ball (4)").GetComponent<Ball4>().IsMoving == true) return true;
        //if (GameObject.Find("Ball (5)") != null && !Holes.Instance.GetIsIn()[4]) if (GameObject.Find("Ball (5)").GetComponent<Ball5>().IsMoving == true) return true;
        //if (GameObject.Find("Ball (6)") != null && !Holes.Instance.GetIsIn()[5]) if (GameObject.Find("Ball (6)").GetComponent<Ball6>().IsMoving == true) return true;
        //if (GameObject.Find("Ball (7)") != null && !Holes.Instance.GetIsIn()[6]) if (GameObject.Find("Ball (7)").GetComponent<Ball7>().IsMoving == true) return true;
        //if (GameObject.Find("Ball (8)") != null && !Holes.Instance.GetIsIn()[7]) if (GameObject.Find("Ball (8)").GetComponent<Ball8>().IsMoving == true) return true;
        //if (GameObject.Find("Ball (9)") != null && !Holes.Instance.GetIsIn()[8]) if (GameObject.Find("Ball (9)").GetComponent<Ball9>().IsMoving == true) return true;
        //if (GameObject.Find("Ball (10)") != null && !Holes.Instance.GetIsIn()[9]) if (GameObject.Find("Ball (10)").GetComponent<Ball10>().IsMoving == true) return true;
        //if (GameObject.Find("Ball (11)") != null && !Holes.Instance.GetIsIn()[10]) if (GameObject.Find("Ball (11)").GetComponent<Ball11>().IsMoving == true) return true;
        //if (GameObject.Find("Ball (12)") != null && !Holes.Instance.GetIsIn()[11]) if (GameObject.Find("Ball (12)").GetComponent<Ball12>().IsMoving == true) return true;
        //if (GameObject.Find("Ball (13)") != null && !Holes.Instance.GetIsIn()[12]) if (GameObject.Find("Ball (13)").GetComponent<Ball13>().IsMoving == true) return true;
        //if (GameObject.Find("Ball (14)") != null && !Holes.Instance.GetIsIn()[13]) if (GameObject.Find("Ball (14)").GetComponent<Ball14>().IsMoving == true) return true;
        //if (GameObject.Find("Ball (15)") != null && !Holes.Instance.GetIsIn()[14]) if (GameObject.Find("Ball (15)").GetComponent<Ball15>().IsMoving == true) return true;
        bool status = false;
        for (int i = 0; i < theBalls.Count; i++)
        {
            if (theBalls[i] != null && !Holes.Instance.GetIsIn()[i] && theBalls[i].GetComponent<NormalBallScript>().IsMoving == true) { return true; }
        }
        //if (GameObject.Find("Ball (15)") != null && !Holes.Instance.GetIsIn()[14]) if (GameObject.Find("Ball (15)").GetComponentInParent<NormalBallScript>().IsMoving == true) return true;

        return false;
    }

    private void SetBallPos()
    {
        start_pos = transform.position;
        theBalls[0].transform.position = new Vector3(3.28f, start_pos.y, 0f);
        theBalls[1].transform.position = new Vector3(3.656f, start_pos.y, 0.216f);
        theBalls[2].transform.position = new Vector3(4.039f, start_pos.y, 0.424f);
        theBalls[3].transform.position = new Vector3(4.4224f, start_pos.y, 0.6334f);
        theBalls[4].transform.position = new Vector3(4.8224f, start_pos.y, -0.916f);
        theBalls[5].transform.position = new Vector3(4.8224f, start_pos.y, 0.425f);
        theBalls[6].transform.position = new Vector3(4.4224f, start_pos.y, -0.216f);
        theBalls[7].transform.position = new Vector3(4.039f, start_pos.y, 0.0f);
        theBalls[8].transform.position = new Vector3(3.665f, start_pos.y, -0.216f);
        theBalls[9].transform.position = new Vector3(4.039f, start_pos.y, -0.425f);
        theBalls[10].transform.position = new Vector3(4.4224f, start_pos.y, -0.663f);
        theBalls[11].transform.position = new Vector3(4.8224f, start_pos.y, 0.9167f);
        theBalls[12].transform.position = new Vector3(4.8224f, start_pos.y, -0.425f);
        theBalls[13].transform.position = new Vector3(4.4224f, start_pos.y, 0.216f);
        theBalls[14].transform.position = new Vector3(4.8224f, start_pos.y, 0.0f);
    }

    private void SetBallMaterial()
    {
        for (int i = 0; i < theBalls.Count; i++)
        {
            theBalls[i].GetComponent<MeshRenderer>().material = the_materials[i];
        }
    }

    public void CreateBalls()
    {
        SpawnBalls();
        SetBallPos();
        SetBallMaterial();
    }

    #endregion

    private void Start()
    {
        CreateBalls();
    }
}