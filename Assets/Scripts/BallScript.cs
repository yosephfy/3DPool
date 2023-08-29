using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public GameObject Stick;
    public static BallScript Instance;

    #region
    public List<GameObject> all_balls;
    #endregion

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (Instance)
            Destroy(Instance);

        Instance = this;

        //For good measure, set the previous locations
        for (int i = 0; i < previousLocations.Length; i++)
        {
            previousLocations[i] = Vector3.zero;
        }
    }

    private void Start()
    {
        lastXVal = transform.position.x;
    }

    private void OnMouseDown()
    {
        transform.localRotation = new Quaternion(0, 0, 0, 360);
    }

    public void ChangeDirection()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.mousePosition.x > 300)
            if (Physics.Raycast(ray, out hit, 1000))
            {
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
                transform.Rotate(0, -90, 0);
            }
    }

    private void FixedUpdate()
    {
        //if (GetComponent<Rigidbody>().velocity == new Vector3(0, 0, 0))
        //{
        //    Stick.SetActive(true);
        //    transform.localEulerAngles -= new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        //    SliderScript.Instance.ActivateSlider(true);
        //    ChangeDirection();
        //}
        //else if (GetComponent<Rigidbody>().velocity.x > 0.2 || GetComponent<Rigidbody>().velocity.z > 0.2)
        //{
        //    SliderScript.Instance.ActivateSlider(false);
        //    Stick.SetActive(false);
        //}
    }

    private float lastXVal;

    private void Uppdate()
    {
        if (SliderScript.Instance.GetBallShot())
        {
            SliderScript.Instance.ActivateSlider(false);
            SliderScript.Instance.SetBallShot(true);
            Stick.SetActive(false);
            if (transform.position.x < lastXVal)
            {
                Debug.Log("Decreased!");
                //Update lastXVal
                if ((GetComponent<Rigidbody>().velocity.x) < 0.02 || (GetComponent<Rigidbody>().velocity.z) < 0.02)
                {
                    transform.position = transform.position;
                    GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                    SliderScript.Instance.SetBallShot(false);
                }
                else
                {
                    SliderScript.Instance.ActivateSlider(false);
                    SliderScript.Instance.SetBallShot(true);
                    Stick.SetActive(false);
                }
                lastXVal = transform.position.x;
            }
            else if (transform.position.x > lastXVal)
            {
                Debug.Log("Increased");

                //Update lastXVal
                lastXVal = transform.position.x;
            }

            transform.hasChanged = false;
        }
        else
        {
            Stick.SetActive(true);
            transform.localEulerAngles -= new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            SliderScript.Instance.ActivateSlider(true);
            ChangeDirection();
        }
    }

    private void OnMouseDrag()
    {
        transform.localEulerAngles -= new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);

        float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
        transform.position = new Vector3(pos_move.x, transform.position.y, pos_move.z);
    }

    public Vector3 GetForces(Quaternion rotation, float force)
    {
        float frac = (Mathf.Tan(rotation.y));

        return new Vector3(force, 0, frac * force);
    }

    public void SetForce(float force)
    {
        Stick.SetActive(false);
        GetComponent<Rigidbody>().AddForce(transform.right * force, ForceMode.Impulse);
    }

    public void SetStickActive(bool active)
    {
        Stick.SetActive(active);
    }

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

    //
    public GameObject RayCastObj;

    private float distance;
    public int reflections;
    public float maxLength;

    private LineRenderer lineRenderer;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 direction;

    public void LineRender()
    {
        lineRenderer.enabled = true;

        ray = new Ray(transform.position, transform.right);

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        float remainingLength = maxLength;

        for (int i = 0; i < reflections; i++)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength))
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                remainingLength = Vector3.Distance(ray.origin, hit.point);
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));

                if (hit.collider != null)
                {
                    remainingLength = Vector3.Distance(ray.origin, hit.point) + 1;
                    //RayCastObj.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                }
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
            }
        }
    }

    private void Update()
    {
        //
        //

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

        bool is_moving_balls = true;
        BallTestScript ballTestScript = new BallTestScript();

        if (ballTestScript.AreBallsMoving() == false)
        {
            is_moving_balls = false;
        }

        if (isMoving == true && is_moving_balls == true)
        {
            SliderScript.Instance.ActivateSlider(false);
            SetStickActive(false);
            lineRenderer.enabled = false;
            //Stick.SetActive(false);
        }
        else if (isMoving == false && is_moving_balls == false)
        {
            SetStickActive(true);
            //Stick.SetActive(true);
            transform.localEulerAngles -= new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
            SliderScript.Instance.ActivateSlider(true);
            ChangeDirection();
            LineRender();
        }
        else
        {
            lineRenderer.enabled = false;
            SliderScript.Instance.ActivateSlider(false);
            SetStickActive(false);
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
        for (int i = 1; i < 16; i++)
            if (GameObject.Find("Ball (" + i + ")") != null && !Holes.Instance.GetIsIn()[i - 1]) if (GameObject.Find("Ball (" + i + ")").GetComponent<NormalBallScript>().IsMoving == true) return true;

        //if (GameObject.Find("Ball (15)") != null && !Holes.Instance.GetIsIn()[14]) if (GameObject.Find("Ball (15)").GetComponentInParent<NormalBallScript>().IsMoving == true) return true;

        return false;
    }

    #endregion
}