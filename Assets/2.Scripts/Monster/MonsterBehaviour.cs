using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviour : BaseBehaviour
{
    [SerializeField] private Rigidbody2D _rigid;



    [Header("Movement")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _maxMovementSpeed;






    private void FixedUpdate()
    {
        MoveTowardPlayer();
    }


    private void MoveTowardPlayer()
    {
        _rigid.AddForce(Vector2.left * _movementSpeed, ForceMode2D.Impulse);
        var clampedVelocity = Mathf.Clamp(_rigid.velocity.x, -_maxMovementSpeed, _maxMovementSpeed);
        _rigid.velocity = new Vector2(clampedVelocity, _rigid.velocity.y);
    }



#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _rigid = GetComponent<Rigidbody2D>();
    }
#endif

}
