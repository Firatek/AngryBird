using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingshotHandler : MonoBehaviour
{
    [Header("Line renderers")]
    [SerializeField] private LineRenderer _leftLineRenderer;
    [SerializeField] private LineRenderer _rightLineRenderer;

    [Header("Transforms")]
    [SerializeField] private Transform _leftStartPos;
    [SerializeField] private Transform _rightStartPos;
    [SerializeField] private Transform _centerPos;
    [SerializeField] private Transform _idlePos;

    [Header("Stats")]
    [SerializeField] private float _maxDistance = 3.5f;
    [SerializeField] private float _force = 1.0f;
    [SerializeField] private float _timeRespawn = 2.0f;

    [Header("Scripts")]
    [SerializeField] private SlingShotArea _slingShotArea;

    [Header("Bird")]
    [SerializeField] private AngryBird angryBirdPrefab;
    [SerializeField] private float _angryBirdPositionOffset = 2f;

    private AngryBird spawnedBird;

    private Vector2 _slingShotLinesPosition;

    private Vector2 _direction;
    private Vector2 _directionNormalized;

    private bool _clickedWithinArea = false;
    private bool _birdOnSlingshot;

    private void Awake() {
        _leftLineRenderer.enabled = false;
        _rightLineRenderer.enabled = false;
        SpawnAngryBird();
    }


    // Update is called once per frame
    private void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame && _slingShotArea.isWithinSlingshotArea()) {
            _clickedWithinArea = true;
        }
        //_clickedWithinArea = Mouse.current.leftButton.wasPressedThisFrame && _slingShotArea.isWithinSlingshotArea();
       
        if (Mouse.current.leftButton.isPressed && _clickedWithinArea && _birdOnSlingshot) 
        {
            DrawSlingShot();
            PositionAndRotateBird();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame && _birdOnSlingshot) {
            if (GameManager.instance.isShotAvailable()) {
                _clickedWithinArea = false;

                spawnedBird.launchBird(_direction, _force);

                GameManager.instance.UseShot();

                _birdOnSlingshot = false;

                setLines(_centerPos.position);
                if (GameManager.instance.isShotAvailable()) {
                    StartCoroutine(SpawnAngryBirdAfterTime());
                }
            }

        }
    }

    #region SlingShotMethods

    private void DrawSlingShot() 
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        _slingShotLinesPosition = _centerPos.position + Vector3.ClampMagnitude(touchPosition - _centerPos.position, _maxDistance);

        setLines(_slingShotLinesPosition);

        _direction = (Vector2)_centerPos.position - _slingShotLinesPosition;
        _directionNormalized = _direction.normalized;
    }

    private void setLines(Vector2 position) {
        if (!_leftLineRenderer.enabled && !_rightLineRenderer.enabled) {
            _leftLineRenderer.enabled = true;
            _rightLineRenderer.enabled = true;
        }

        _leftLineRenderer.SetPosition(0, position);
        _leftLineRenderer.SetPosition(1, _leftStartPos.position);

        _rightLineRenderer.SetPosition(0, position);
        _rightLineRenderer.SetPosition(1, _rightStartPos.position);
    }

    #endregion

    #region Angry birds methods
    private void SpawnAngryBird() {
        setLines(_idlePos.position);
        Vector2 dir = (_centerPos.position - _idlePos.position).normalized;
        Vector2 spawnPostion = (Vector2)_idlePos.position + dir * _angryBirdPositionOffset;

        spawnedBird = Instantiate(angryBirdPrefab, spawnPostion, Quaternion.identity);
        spawnedBird.transform.right = dir;
        _birdOnSlingshot = true;
        
    }

    private void PositionAndRotateBird() {
        spawnedBird.transform.position = _slingShotLinesPosition + _directionNormalized * _angryBirdPositionOffset;
        spawnedBird.transform.right = _directionNormalized;
    }

    private IEnumerator SpawnAngryBirdAfterTime() {
        yield return new WaitForSeconds(_timeRespawn);

        SpawnAngryBird();

    }
    #endregion
}
