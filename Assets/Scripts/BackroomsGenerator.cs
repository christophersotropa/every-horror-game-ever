using System.Collections.Generic;
using UnityEngine;

public class BackroomsGenerator : MonoBehaviour
{
    public Transform player;
    public GameObject cellPrefab;
    public GameObject wallPrefab;
    public GameObject pillarPrefab;
    public float cellSize = 4f;
    public int viewRadius = 6;
    [Range(0f, 1f)] public float wallChance = 0.3f;
    [Range(0f, 1f)] public float pillarChance = 0.04f;
    [Range(0f, 1f)] public float lightChance = 0.6f;
    public int seed = 12345;

    readonly Dictionary<Vector2Int, GameObject> cells = new();

    void Update()
    {
        var c = new Vector2Int(
            Mathf.RoundToInt(player.position.x / cellSize),
            Mathf.RoundToInt(player.position.z / cellSize));

        for (int x = -viewRadius; x <= viewRadius; x++)
            for (int z = -viewRadius; z <= viewRadius; z++)
            {
                var key = new Vector2Int(c.x + x, c.y + z);
                if (!cells.ContainsKey(key)) cells[key] = Spawn(key);
            }

        var toRemove = new List<Vector2Int>();
        foreach (var kv in cells)
            if (Mathf.Abs(kv.Key.x - c.x) > viewRadius + 2 || Mathf.Abs(kv.Key.y - c.y) > viewRadius + 2)
                toRemove.Add(kv.Key);
        foreach (var k in toRemove) { Destroy(cells[k]); cells.Remove(k); }
    }

    GameObject Spawn(Vector2Int k)
    {
        Vector3 pos = new Vector3(k.x * cellSize, 0f, k.y * cellSize);
        var cell = Instantiate(cellPrefab, pos, Quaternion.identity, transform);

        bool safeZone = Mathf.Abs(k.x) <= 1 && Mathf.Abs(k.y) <= 1;

        if (!safeZone && Hash01(k.x, k.y, 1) < wallChance)
            Instantiate(wallPrefab, pos + new Vector3(0, 0, cellSize / 2f),
                        Quaternion.identity, cell.transform);
        if (!safeZone && Hash01(k.x, k.y, 2) < wallChance)
            Instantiate(wallPrefab, pos + new Vector3(cellSize / 2f, 0, 0),
                        Quaternion.Euler(0, 90, 0), cell.transform);

        if (pillarPrefab && !safeZone && Hash01(k.x, k.y, 3) < pillarChance)
            Instantiate(pillarPrefab, pos, Quaternion.identity, cell.transform);

        // Turn off the whole light fixture (panel, light, and hum) on some cells
        var lamp = cell.GetComponentInChildren<Light>();
        if (lamp && Hash01(k.x, k.y, 4) > lightChance)
            lamp.transform.parent.gameObject.SetActive(false);

        return cell;
    }

    float Hash01(int a, int b, int salt)
    {
        unchecked
        {
            uint h = 2166136261;
            h = (h ^ (uint)seed) * 16777619;
            h = (h ^ (uint)a) * 16777619;
            h = (h ^ (uint)b) * 16777619;
            h = (h ^ (uint)salt) * 16777619;
            return (h & 0xFFFFFF) / (float)0x1000000;
        }
    }
}