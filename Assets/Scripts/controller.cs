using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class controller : MonoBehaviour
{
    protected GameObject Ball;
    protected Vector3 MousePos;
    protected GameObject cloneWhiteBall;

    private void Start()
    {
        Ball = GameObject.Find("Ball (cue)");
        cloneWhiteBall = GameObject.Find("GhostWhiteball");
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>() as LineRenderer;
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.startWidth = 0.05f;
        lineRenderer.startColor = Color.white;
        lineRenderer.positionCount = (2);
    }

    private void Update()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        MousePos = Input.mousePosition;
        MousePos.z = Ball.transform.position.y;
        var v3 = Camera.main.ScreenToWorldPoint(MousePos);
        v3.y = Ball.transform.position.y;

        lineRenderer.SetPosition(0, Ball.transform.position);
        lineRenderer.SetPosition(1, v3);
        ForecastHitPosition(Ball, cloneWhiteBall, v3);
    }

    private Vector3 ForecastGhostBallPosition(GameObject ball, Vector3 direction)
    {
        float radius = Ball.GetComponent<SphereCollider>().radius;
        Vector3 originalPosition = ball.transform.position;
        Vector3 targetPosition = Vector3.zero;
        Vector3 rayStartLeftPosition = originalPosition + Quaternion.AngleAxis(-90, Vector3.up) * direction * radius;
        Vector3 rayStartRightPosition = originalPosition + Quaternion.AngleAxis(90, Vector3.up) * direction * radius;

        Debug.DrawRay(rayStartLeftPosition, direction);
        Debug.DrawRay(rayStartRightPosition, direction);

        RaycastHit hit1, hit2;
        GameObject A = null, B = null;
        if (Physics.Raycast(rayStartLeftPosition, direction, out hit1, 1000))
        {
            A = hit1.rigidbody.gameObject;
        }
        if (Physics.Raycast(rayStartRightPosition, direction, out hit2, 1000))
        {
            B = hit2.rigidbody.gameObject;
        }

        if (A != null || B != null)
        {
            if (A != null && B != null)
            {
                if (A.transform.position != B.transform.position)
                {
                    float distanceA = Vector3.Distance(originalPosition, A.transform.position);
                    float distanceB = Vector3.Distance(originalPosition, B.transform.position);
                    if (distanceA < distanceB)
                    {
                        targetPosition = A.transform.position;
                    }
                    else
                    {
                        targetPosition = B.transform.position;
                    }
                }
                else
                {
                    targetPosition = A.transform.position;
                }
            }
            else if (A != null && B == null)
            {
                targetPosition = A.transform.position;
            }
            else if (A == null && B != null)
            {
                targetPosition = B.transform.position;
            }
            if (targetPosition != Vector3.zero)
            {
                Vector3 projectPoint = ProjectPointOnLine(originalPosition, direction, targetPosition);
                float h = Vector3.Distance(targetPosition, projectPoint);
                float x = Mathf.Sqrt(4 * radius * radius - h * h);
                return projectPoint - direction * x;
            }
        }
        return Vector3.zero;
    }

    public void ForecastHitPosition(GameObject WhiteBall, GameObject GhostWhiteBall, Vector3 MousePositionInWorld)
    {
        var direction = (MousePositionInWorld - WhiteBall.transform.position).normalized;

        Vector3 pos = ForecastGhostBallPosition(WhiteBall, direction);
        if (pos != Vector3.zero)
        {
            GhostWhiteBall.transform.position = pos;
        }
        else
        {
            GhostWhiteBall.transform.position = MousePositionInWorld;
        }
    }

    public static Vector3 ProjectPointOnLine(Vector3 linePoint, Vector3 lineVec, Vector3 point)
    {
        //get vector from point on line to point in space
        Vector3 linePointToPoint = point - linePoint;
        float t = Vector3.Dot(linePointToPoint, lineVec);
        return linePoint + lineVec * t;
    }
}