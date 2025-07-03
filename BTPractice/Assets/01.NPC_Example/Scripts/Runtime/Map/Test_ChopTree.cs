using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ChopTree : MonoBehaviour
{
    [SerializeField] Terrain _terrain;
    [SerializeField] GameObject _stumpPrefab;
    [SerializeField] LayerMask _terrainMask;
    TerrainData _data;

    private void Awake()
    {
        _data = _terrain.terrainData;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            TryChop();
        }
    }

    void TryChop()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _terrainMask))
        {
            Debug.Log($"HIT {hit.collider.gameObject.name}");

            int index = FindTreeIndex(hit.point, 0.01f);

            if (index < 0)
                return;

            TreeInstance removed = _data.GetTreeInstance(index);
            RemoveTree(index);

            Instantiate(_stumpPrefab,
                        TerrainLocalToWorld(removed.position),
                        Quaternion.Euler(0, Random.Range(0, 360f), 0));
        }
    }

    void RemoveTree(int index)
    {
        List<TreeInstance> trees = new List<TreeInstance>(_data.treeInstances);
        trees.RemoveAt(index); //O(N) 알고리즘
        _data.SetTreeInstances(trees.ToArray(), true);
    }

    int FindTreeIndex(Vector3 worldPos, float maxDistance)
    {
        Vector3 terrainPos = worldPos - _terrain.transform.position;

        Vector3 terrainPosNormalizedXZ = new Vector3(terrainPos.x / _data.size.x, 0f, terrainPos.z / _data.size.z);

        TreeInstance[] instances = _data.treeInstances;
        int nearestIndex = -1;
        float nearestDistance = maxDistance * maxDistance;

        for(int i = 0; i < instances.Length; i++)
        {
            Vector2 deltaPos = new Vector2(instances[i].position.x - terrainPosNormalizedXZ.x,
                                           instances[i].position.z - terrainPosNormalizedXZ.z);

            float sq = deltaPos.sqrMagnitude;

            if(sq < nearestDistance)
            {
                nearestDistance = sq;
                nearestIndex = i;
            }
        }

        return nearestIndex;
        
    }

    Vector3 TerrainLocalToWorld(Vector3 normalizedPos)
    {
        Vector3 pos = Vector3.Scale(normalizedPos, _data.size);
        pos.y = _terrain.SampleHeight(pos) + _terrain.transform.position.y;
        return pos;
    }
}
