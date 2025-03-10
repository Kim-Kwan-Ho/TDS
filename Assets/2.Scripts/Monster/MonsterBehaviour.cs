using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviour : BaseBehaviour
{



    [Header("Movement")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _maxMovementSpeed;


    [Header("Jump")]
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _jumpDelay;
    private WaitForSeconds _jumpWaitTime;
    private bool _isGround;



    [Header("Physics")]
    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] private CapsuleCollider2D _capsuleCol;
    [SerializeField] private float _rayOffSet;
    [SerializeField] private float _frontRayDist;
    [SerializeField] private float _backRayDist;
    [SerializeField] private float _upperRayDist;
    [SerializeField] private float _upperLeftRayDist;
    [SerializeField] private float _downRayDist;
    [SerializeField] private LayerMask _monsterLayer;
    [SerializeField] private LayerMask _groundLayer;

    protected override void Initialize()
    {
        base.Initialize();
        _jumpWaitTime = new WaitForSeconds(_jumpDelay);
        _isGround = true;
    }

    private void Start()
    {
        StartCoroutine(CoJump());
    }


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



    private IEnumerator CoJump()
    {
        while (true)
        {
            yield return _jumpWaitTime;
            if (CanJump())
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        _rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
    }

    private bool CanJump()
    {
        return ((_isGround || CheckBottom()) && CheckFrontMonster() && !CheckUpperMonster() && !CheckUpperLeftMonster() && !CheckBackMonster());
    }

    private bool CheckMonster(Vector2 startPos, Vector2 direction, float dist)
    {
        var hit = Physics2D.Raycast(startPos, direction, dist, _monsterLayer);
        return hit.collider != null;
    }


    private bool CheckFrontMonster()
    {
        return CheckMonster(new Vector2(_capsuleCol.bounds.min.x - _rayOffSet, _capsuleCol.bounds.center.y), Vector2.left, _frontRayDist);
    }
    private bool CheckUpperMonster()
    {
        return CheckMonster(new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.max.y + _rayOffSet), Vector2.left, _upperRayDist);
    }
    private bool CheckUpperLeftMonster()
    {
        return CheckMonster(new Vector2(_capsuleCol.bounds.max.x, _capsuleCol.bounds.max.y + _rayOffSet), Vector2.left, _upperLeftRayDist);
    }


    private bool CheckBackMonster()
    {
        return CheckMonster(new Vector2(_capsuleCol.bounds.max.x + _rayOffSet, _capsuleCol.bounds.center.y), Vector2.right, _backRayDist);
    }

    private bool CheckBottom()
    {
        return CheckMonster(new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.min.y - _rayOffSet), Vector2.down, _downRayDist);
    }



    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            _isGround = false;
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(_capsuleCol.bounds.max.x, _capsuleCol.bounds.max.y + _rayOffSet), new Vector2(_capsuleCol.bounds.max.x, _capsuleCol.bounds.max.y + _rayOffSet) + Vector2.left * _upperLeftRayDist);
        Gizmos.DrawLine(new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.max.y + _rayOffSet), new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.max.y + _rayOffSet) + Vector2.up * _upperRayDist);
        Gizmos.DrawLine(new Vector2(_capsuleCol.bounds.max.x + _rayOffSet, _capsuleCol.bounds.center.y), new Vector2(_capsuleCol.bounds.max.x + _rayOffSet, _capsuleCol.bounds.center.y) + Vector2.right * _backRayDist);
        Gizmos.DrawLine(new Vector2(_capsuleCol.bounds.min.x - _rayOffSet, _capsuleCol.bounds.center.y), new Vector2(_capsuleCol.bounds.min.x - _rayOffSet, _capsuleCol.bounds.center.y) + Vector2.left * _frontRayDist);
        Gizmos.DrawLine(new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.min.y - _rayOffSet), new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.min.y - _rayOffSet) + Vector2.down * _downRayDist);
    }
    protected override void OnBindField()
    {
        base.OnBindField();
        _rigid = GetComponent<Rigidbody2D>();
        _capsuleCol = GetComponent<CapsuleCollider2D>();
    }
#endif

}
