using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ScreenManager
{
    private CameraController cameraController;  // 카메라 컨트롤러 선언

    public CameraController CameraController    // 카메라 컨트롤러 프로퍼티 선언
    {
        get
        {
            if (cameraController == null)
                CreateCamera();

            return cameraController;
        }
    }

    public bool isSkillCasting = false;
    public void SetCamera(CameraController _cameraController)
    {
        cameraController = _cameraController;
    }

    public void SetCameraPosition(Vector2 _vector2 )
    {
        CameraController.SetPosition(_vector2);
    }

    private void CreateCamera()
    {
        GameObject go = GameObject.Find("Main Camera");
        if (go == null)
            go = Managers.Resource.Instantiate("Main Camera");
        UnityEngine.Object.DontDestroyOnLoad(go);
        cameraController = go.GetOrAddComponent<CameraController>();
    }

    // 카메라 타겟 설정
    public void SetCameraTarget(Transform _target)
    {
        CameraController.SetTarget(_target);
    }

    // 카메라 오프셋 설정
    public void SetCameraOffset(Vector3 _offset)
    {
        CameraController.SetOffset(_offset);
    }

    public void Shake(float _shakeForce, float _time = 0)
    {
        CameraController.shakeForce = _shakeForce;
        CameraController.isShake = !CameraController.isShake;
        if (_time != 0)
            Managers.Routine.StartCoroutine(ShakeRoutine(_time));
    }

    private IEnumerator ShakeRoutine(float _time)
    {
        yield return new WaitForSeconds(_time);
        CameraController.isShake = false;
    }

    public void FadeIn(float _fadeTime, Action _callback = null)
    {
        Managers.Routine.StartCoroutine(CameraController.FadeIn(_fadeTime, () => { _callback?.Invoke(); }));
    }

    public void FadeOut(float _fadeTime, Action _callback = null)
    {
        Managers.Routine.StartCoroutine(CameraController.FadeOut(_fadeTime, () => { _callback?.Invoke(); }));
    }

    public void FadeInOut(float _totalTile, Action _callback = null)
    {
        Managers.Routine.StartCoroutine(CameraController.FadeInOut(_totalTile, () => { _callback?.Invoke(); }));
    }

    public void SkillScreen(BattleEntityData _data)
    {
        isSkillCasting = true;
        Managers.UI.SkillScreen.gameObject.SetActive(true);
        Managers.UI.ShowToast($"{_data.name} UseSkill");
        Managers.Routine.StartCoroutine(CameraController.SkillScreenRoutine(() => 
        { 
            isSkillCasting = false;  
        }));
    }
}