using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickScript : MonoBehaviour
{
    public static StickScript Instance;

    private void Awake()
    {
        if (Instance)
            Destroy(Instance);

        Instance = this;
    }

    public void SlideStick(float value)
    {
        GetComponent<Transform>().localPosition = new Vector3(-value - 12.88f, 1.75f, 0);
    }
}