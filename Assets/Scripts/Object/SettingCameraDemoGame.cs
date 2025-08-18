using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingCameraDemoGame : MonoBehaviour
{
    [Header("Main: ")]
    [SerializeField] private List<Transform> _cameraPoints;
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _waitBetweenPoints;
    [SerializeField] private float _returnSpeedMultiplier;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private Transform _cameraFollowTarget;

    [Header("Component: ")]
    private GameManager _gameManager;
    private PlayerController _player;
    private Transform _playerTarget;

    private static bool _hasPlayedDemo = false;

    private void Awake()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
        _player = FindAnyObjectByType<PlayerController>();
        _playerTarget = _player.transform;
    }
    void Start()
    {
        if (_hasPlayedDemo)
        {
            _virtualCamera.Follow = _playerTarget;
            _player.enabled = true;
            _gameManager.StartGame();
        }
        else
        {
            _player.enabled = false;
            _virtualCamera.Follow = _cameraFollowTarget;
            StartCoroutine(PanThoughPoints());
        }
    }
    //Tạo đường đi camera
    private IEnumerator PanThoughPoints()
    {
        Transform camPos = _virtualCamera.transform;

        for (int i = 0; i < _cameraPoints.Count; i++)
        {
            yield return StartCoroutine(MoveCameraTo(_cameraPoints[i].position, _moveDuration));
            yield return new WaitForSecondsRealtime(_waitBetweenPoints);
        }
        yield return StartCoroutine(MoveCameraTo(_playerTarget.position, _moveDuration / _returnSpeedMultiplier));
        _virtualCamera.Follow = _playerTarget;
        Time.timeScale = 1f;
        yield return new WaitForSeconds(0.5f);

        _gameManager.StartGame();
        _hasPlayedDemo = true;
    }
    private IEnumerator MoveCameraTo(Vector3 targetPos, float duration)
    {
        Vector3 start = _cameraFollowTarget.position;
        targetPos.z = start.z;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            _cameraFollowTarget.position = Vector3.Lerp(start, targetPos, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        _cameraFollowTarget.position = targetPos;
    }
}
