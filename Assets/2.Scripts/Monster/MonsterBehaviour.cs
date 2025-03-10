using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBehaviour : BaseBehaviour
{


    [Header("ScriptableObject")]
    [SerializeField] private MonsterSettingsSo _monsterSettingsSo;


    [Header("Jump")]
    private WaitForSeconds _jumpWaitTime;
    private bool _isGround;



    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] private CapsuleCollider2D _capsuleCol;


    protected override void Initialize()
    {
        base.Initialize();
        _jumpWaitTime = new WaitForSeconds(_monsterSettingsSo.JumpDelay);
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
        _rigid.AddForce(Vector2.left * _monsterSettingsSo.MovementSpeed, ForceMode2D.Impulse);
        var clampedVelocity = Mathf.Clamp(_rigid.velocity.x, -_monsterSettingsSo.MaxMovementSpeed, _monsterSettingsSo.MaxKnockBackSpeed);
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
        _rigid.AddForce(Vector2.up * _monsterSettingsSo.JumpPower, ForceMode2D.Impulse);
    }

    private bool CanJump()
    {
        return ((_isGround || CheckBottom()) && CheckFrontMonster() && !CheckUpperMonster() && !CheckUpperSideMonster() && !CheckBackMonster());
    }

    private bool CheckMonster(Vector2 startPos, Vector2 direction, float dist)
    {
        var hit = Physics2D.Raycast(startPos, direction, dist, _monsterSettingsSo.MonsterLayer);
        return hit.collider != null;
    }


    private bool CheckFrontMonster()
    {
        return CheckMonster(new Vector2(_capsuleCol.bounds.min.x - _monsterSettingsSo.RayOffSet, _capsuleCol.bounds.center.y), Vector2.left, _monsterSettingsSo.FrontRayDist);
    }
    private bool CheckUpperMonster()
    {
        return CheckMonster(new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.max.y + _monsterSettingsSo.RayOffSet), Vector2.left, _monsterSettingsSo.UpperRayDist);
    }
    private bool CheckUpperSideMonster()
    {

        return CheckMonster(new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.max.y + _monsterSettingsSo.RayOffSet * 2), Vector2.left, _monsterSettingsSo.UpperSideRayDist) &&
               CheckMonster(new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.max.y + _monsterSettingsSo.RayOffSet * 2), Vector2.right, _monsterSettingsSo.UpperSideRayDist)
               ;
    }


    private bool CheckBackMonster()
    {
        return CheckMonster(new Vector2(_capsuleCol.bounds.max.x + _monsterSettingsSo.RayOffSet, _capsuleCol.bounds.center.y), Vector2.right, _monsterSettingsSo.BackRayDist);
    }

    private bool CheckBottom()
    {
        return CheckMonster(new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.min.y - _monsterSettingsSo.RayOffSet), Vector2.down, _monsterSettingsSo.DownRayDist);
    }



    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Monster"))
        {
            if (CheckKnockBack(col.collider.bounds.center))
            {
                KnockBack();
            }
        }
    }
    private bool CheckKnockBack(Vector2 target)
    {
        return (_capsuleCol.bounds.center.x >= target.x - _monsterSettingsSo.KnockBackMargin) && (_capsuleCol.bounds.center.y < target.y);
    }

    private void KnockBack()
    {
        if (!_isGround)
            return;
        _rigid.AddForce(Vector2.right * _monsterSettingsSo.KnockBackPower, ForceMode2D.Impulse);
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
        Gizmos.DrawLine(new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.max.y + _monsterSettingsSo.RayOffSet * 2), new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.max.y + _monsterSettingsSo.RayOffSet * 2) + Vector2.right * _monsterSettingsSo.UpperSideRayDist);
        Gizmos.DrawLine(new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.max.y + _monsterSettingsSo.RayOffSet * 2), new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.max.y + _monsterSettingsSo.RayOffSet * 2) + Vector2.left * _monsterSettingsSo.UpperSideRayDist);

        Gizmos.DrawLine(new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.max.y + _monsterSettingsSo.RayOffSet), new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.max.y + _monsterSettingsSo.RayOffSet) + Vector2.up * _monsterSettingsSo.UpperRayDist);
        Gizmos.DrawLine(new Vector2(_capsuleCol.bounds.max.x + _monsterSettingsSo.RayOffSet, _capsuleCol.bounds.center.y), new Vector2(_capsuleCol.bounds.max.x + _monsterSettingsSo.RayOffSet, _capsuleCol.bounds.center.y) + Vector2.right * _monsterSettingsSo.BackRayDist);
        Gizmos.DrawLine(new Vector2(_capsuleCol.bounds.min.x - _monsterSettingsSo.RayOffSet, _capsuleCol.bounds.center.y), new Vector2(_capsuleCol.bounds.min.x - _monsterSettingsSo.RayOffSet, _capsuleCol.bounds.center.y) + Vector2.left * _monsterSettingsSo.FrontRayDist);
        Gizmos.DrawLine(new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.min.y - _monsterSettingsSo.RayOffSet), new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.min.y - _monsterSettingsSo.RayOffSet) + Vector2.down * _monsterSettingsSo.DownRayDist);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.center.y), new Vector2(_capsuleCol.bounds.center.x, _capsuleCol.bounds.center.y) + Vector2.right * _monsterSettingsSo.KnockBackMargin);

    }
    protected override void OnBindField()
    {
        base.OnBindField();
        _rigid = GetComponent<Rigidbody2D>();
        _capsuleCol = GetComponent<CapsuleCollider2D>();
        _monsterSettingsSo = FindObjectInAsset<MonsterSettingsSo>();
    }
#endif

}
