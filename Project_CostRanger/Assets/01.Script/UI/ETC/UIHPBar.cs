using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHPBar : MonoBehaviour
{
    private BaseController controller;
    private Image hpSlider;
    private Transform bundle;
    public Vector3 offset;

    public void Init(BaseController _controller)
    {
        controller = _controller;
        hpSlider = Util.FindChild<Image>(gameObject, "Image_HP", true);
        bundle = Util.FindChild<Transform>(gameObject, "Bundle_HPSlider", true);

        if (controller as RangerController)
            hpSlider.color = Color.green;
        else
            hpSlider.color = Color.red;
    }

    public void Update()
    {
        hpSlider.fillAmount = (float)controller.status.CurrentHP / (float)controller.status.CurrentMaxHP;
        bundle.transform.position = Camera.main.WorldToScreenPoint(controller.hpBarTrans.position);
        bundle.localScale = controller.transform.localScale;

        if(controller.status.CurrentHP == 0)
        {
            Managers.Resource.Destroy(gameObject);
        }
    }
}
