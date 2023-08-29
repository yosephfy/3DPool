using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SliderScript : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler, IPointerDownHandler
{
    public static SliderScript Instance;

    private void Awake()
    {
        if (Instance)
            Destroy(Instance);

        Instance = this;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnSubmit(BaseEventData eventData)
    {
    }

    private bool is_ball_shot = false;

    public bool GetBallShot()
    {
        return is_ball_shot;
    }

    public void SetBallShot(bool shot)
    {
        is_ball_shot = shot;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("UP");
        if (GetComponent<Slider>().value > 0)
        {
            BallScript.Instance.SetForce(GetComponent<Slider>().value);
        }
        GetComponent<Slider>().value = 0;
        SetBallShot(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void ActivateSlider(bool active)
    {
        gameObject.SetActive(active);
    }

    private void Update()
    {
        StickScript.Instance.SlideStick(GetComponent<Slider>().value / 10);
    }
}