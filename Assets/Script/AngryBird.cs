using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class AngryBird : MonoBehaviour
{
    private Rigidbody2D _rb2d;

    private CircleCollider2D _circleCollider;

    private bool _hasBeenLaunched;
    private bool _shouldFaceVelDir;

    private void Awake() {
        _rb2d = GetComponent<Rigidbody2D>();
         _circleCollider = GetComponent<CircleCollider2D>();

        _rb2d.isKinematic = true;
        _circleCollider.enabled = false;
    }

    private void FixedUpdate() {
        if (_hasBeenLaunched && _shouldFaceVelDir) {
            transform.right = _rb2d.velocity;
        }
    }

    public void launchBird(Vector2 direction, float force) {
        _rb2d.isKinematic = false;
        _circleCollider.enabled = true;

        //Apply force
        _rb2d.AddForce(direction * force, ForceMode2D.Impulse);

        _hasBeenLaunched = true;
        _shouldFaceVelDir = true;

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        _shouldFaceVelDir = false;
    }
}
