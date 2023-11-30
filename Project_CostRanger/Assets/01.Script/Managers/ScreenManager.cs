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

    public void SetCameraPosition(Vector3 _vector2 )
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


    #region SkillDirecting
    public void PlayRangerSkillDirecting(int _rangerUID, Action _callback = null)
    {
        Managers.UI.SetRangerSkillScreen(_rangerUID, _callback);
    }

    public void PlayEnemySkillDirecting(int _enemyUID, Action _callback = null)
    {
        Managers.UI.SetEnemySkillScreen(_enemyUID, _callback);
    }
    public void StopRangerSkillDirecting()
    {
        Managers.UI.CloseRangerSkillScreen();
    }

    public void StopEnemySkillDirecting()
    {
        Managers.UI.CloseEnemySkillScreen();
    }

    #endregion

    #region Fade

    public void FadeIn(float _fadeTime, Action _callback = null)
    {
        Managers.Routine.StartCoroutine(FadeInRoutine(_fadeTime, () => { _callback?.Invoke(); }));
    }

    public void FadeOut(float _fadeTime, Action _callback = null)
    {
        Managers.Routine.StartCoroutine(FadeOutRoutine(_fadeTime, () => { _callback?.Invoke(); }));
    }

    public void FadeInOut(float _totalTile, Action _callback = null)
    {
        Managers.Routine.StartCoroutine(FadeInOutRoutine(_totalTile, () => { _callback?.Invoke(); }));
    }

    private IEnumerator FadeInRoutine(float _fadeTime, Action _callback = null)
    {
        Managers.UI.BlackPanel.gameObject.SetActive(true);
        Managers.UI.BlackPanel.alpha = 0;
        while (Managers.UI.BlackPanel.alpha < 1)
        {
            Managers.UI.BlackPanel.alpha = Managers.UI.BlackPanel.alpha + Time.deltaTime / _fadeTime;
            yield return null;
        }
        Managers.UI.BlackPanel.alpha = 1;
        _callback?.Invoke();
    }

    private IEnumerator FadeOutRoutine(float _fadeTime, Action _callback = null)
    {
        Managers.UI.BlackPanel.gameObject.SetActive(true);
        Managers.UI.BlackPanel.alpha = 1;
        while (Managers.UI.BlackPanel.alpha > 0)
        {
            Managers.UI.BlackPanel.alpha = Managers.UI.BlackPanel.alpha - Time.deltaTime / _fadeTime;
            yield return null;
        }
        Managers.UI.BlackPanel.alpha = 0;
        Managers.UI.BlackPanel.gameObject.SetActive(false);
        _callback?.Invoke();
    }

    private IEnumerator FadeInOutRoutine(float _totalTile, Action _callback = null)
    {
        yield return Managers.Routine.StartCoroutine(FadeInRoutine(_totalTile * 0.5f));
        yield return Managers.Routine.StartCoroutine(FadeOutRoutine(_totalTile * 0.5f));
        _callback?.Invoke();
    }
    #endregion
}