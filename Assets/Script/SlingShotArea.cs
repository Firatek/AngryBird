using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingShotArea : MonoBehaviour
{

    [SerializeField] private LayerMask _slingshotAreaMask;

    public bool isWithinSlingshotArea() {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        return Physics2D.OverlapPoint(worldPos, _slingshotAreaMask);
    }

}
