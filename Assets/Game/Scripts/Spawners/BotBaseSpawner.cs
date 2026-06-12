using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class BotBaseSpawner : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _ground;
    [SerializeField] private BotBase _botBasePrefab;

    [SerializeField] private Vector3 _firstBasePosition;

    void Awake()
    {
        SpawnBotBase(_firstBasePosition);
    }

    public void SpawnBotBase(Vector3 position)
    {
        BotBase newBase = Instantiate(_botBasePrefab, position, Quaternion.identity);
        newBase.Initialize(this);

        _ground.BuildNavMesh();
    }
}
