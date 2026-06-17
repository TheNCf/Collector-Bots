using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent(typeof(BurstSearcher))]
public class BotBaseSpawner : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _ground;
    [SerializeField] private BotBase _botBasePrefab;

    [SerializeField] private Vector3 _firstBasePosition;

    private BurstSearcher _searcher;

    private List<BotBase> _botBaseList = new List<BotBase>();

    public BurstSearcher BurstSearcher => _searcher;

    public IReadOnlyList<BotBase> BotBaseList => _botBaseList;

    private void Awake()
    {
        _searcher = GetComponent<BurstSearcher>();

        SpawnBotBase(_firstBasePosition);
    }

    public void SpawnBotBase(Vector3 position)
    {
        BotBase newBase = Instantiate(_botBasePrefab, position, Quaternion.identity);
        newBase.Initialize(_botBaseList.Count, this, _searcher);
        _botBaseList.Add(newBase);

        _ground.BuildNavMesh();
    }
}
