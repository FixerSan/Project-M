using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHPBar : MonoBehaviour
{
    private BattleEntityController controller;
    private Image hpSlider;
    private Transform bundle;
    public Vector3 offset;

    public void Init(BattleEntityController _controller)
    {
        controller = _controller;
        hpSlider = Util.FindChild<Image>(gameObject, "Image_HP", true);
        bundle = Util.FindChild<Transform>(gameObject, "Bundle_HPSlider", true);

        if (controller.entityType == Define.BattleEntityType.Army)
            hpSlider.color = Color.green;
        else
            hpSlider.color = Color.red;
    }

    public void FixedUpdate()
    {
        if (controller == null || controller.state == Define.BattleEntityState.Die)
        {
            Managers.Resource.Destroy(gameObject);
            return;
        }
        hpSlider.fillAmount = (float)controller.status.CurrentHP / (float)controller.status.maxHP;
        bundle.position = Managers.Screen.CameraController.Camera.WorldToScreenPoint(controller.transform.position + offset);
    }
}
