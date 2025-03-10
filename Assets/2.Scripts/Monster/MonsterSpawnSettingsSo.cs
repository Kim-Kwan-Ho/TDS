using UnityEngine;


[CreateAssetMenu(fileName = "MonsterSpawnSettings", menuName = "Scriptable Objects/Monster/SpawnSettings")]
public class MonsterSpawnSettingsSo : ScriptableObject
{
    [SerializeField] private float _spawnDelay;
    public float SpawnDelay { get { return _spawnDelay; } }
    [SerializeField] private int _maxSpawnCount;
    public float MaxSpawnCount { get { return _maxSpawnCount; } }
}
