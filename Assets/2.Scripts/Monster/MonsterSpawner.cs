using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : BaseBehaviour
{
    [SerializeField] private GameObject _monsterPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private int _maxSpawnCount;

    private HashSet<GameObject> _spawnedMonsters = new HashSet<GameObject>();

    private WaitForSeconds _spawnWaitTime;
    protected override void Initialize()
    {
        base.Initialize();
        _spawnWaitTime = new WaitForSeconds(_spawnDelay);
        StartCoroutine(CoSpawnMonster());
    }

    private IEnumerator CoSpawnMonster()
    {
        while (true)
        {
            yield return _spawnWaitTime;
            if (CanSpawnMonster())
            {
                GameObject monster = Instantiate(_monsterPrefab, _spawnPoint);
                _spawnedMonsters.Add(monster);
            }
        }
    }

    private bool CanSpawnMonster()
    {
        return (_spawnedMonsters.Count < _maxSpawnCount);
    }


#if UNITY_EDITOR
    protected override void OnBindField()
    {
        base.OnBindField();
        _monsterPrefab = FindObjectInAsset<GameObject>("ZombieMelee", EDataType.prefab);
        _spawnPoint = GameObject.Find("MonsterSpawnPoint").transform;
    }
#endif

}
