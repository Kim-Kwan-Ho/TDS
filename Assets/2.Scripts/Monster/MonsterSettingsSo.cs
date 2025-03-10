using UnityEngine;


[CreateAssetMenu(fileName = "MonsterSettings", menuName = "Scriptable Objects/Monster/Settings")]
public class MonsterSettingsSo : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float _movementSpeed;
    public float MovementSpeed { get { return _movementSpeed; } }
    [SerializeField] private float _maxMovementSpeed;
    public float MaxMovementSpeed { get { return _maxMovementSpeed; } }


    [Header("Jump")]
    [SerializeField] private float _jumpPower;
    public float JumpPower { get { return _jumpPower; } }
    [SerializeField] private float _jumpDelay;
    public float JumpDelay { get { return _jumpDelay; } }

    [Header("Physics")]
    [SerializeField] private float _rayOffSet;
    public float RayOffSet { get { return _rayOffSet; } }
    [SerializeField] private float _frontRayDist;
    public float FrontRayDist { get { return _frontRayDist; } }
    [SerializeField] private float _backRayDist;
    public float BackRayDist { get { return _backRayDist; } }
    [SerializeField] private float _upperRayDist;
    public float UpperRayDist { get { return _upperRayDist; } }
    [SerializeField] private float _upperSideRayDist;
    public float UpperSideRayDist { get { return _upperSideRayDist; } }
    [SerializeField] private float _downRayDist;
    public float DownRayDist { get { return _downRayDist; } }
    [SerializeField] private LayerMask _monsterLayer;
    public LayerMask MonsterLayer { get { return _monsterLayer; } }

    [Header("Cycle")]
    [SerializeField] private float _maxKnockBackSpeed;
    public float MaxKnockBackSpeed { get { return _maxKnockBackSpeed; } }
    [SerializeField] private float _knockBackPower;
    public float KnockBackPower { get { return _knockBackPower; } }
    [SerializeField] private float _knockBackMargin;
    public float KnockBackMargin { get { return _knockBackMargin; } }
}
