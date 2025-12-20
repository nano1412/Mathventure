using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/Level/WaveData")]
public class WaveData : ScriptableObject
{
    public GameObject[] Enemies = new GameObject[4];

    private void OnValidate()
    {
        // Enforce fixed size
        if (Enemies == null || Enemies.Length != 4)
            System.Array.Resize(ref Enemies, 4);

        // Validate entries
        for (int i = 0; i < Enemies.Length; i++)
        {
            var enemy = Enemies[i];

            if (enemy == null)
                continue; // allow empty slots while editing

            if (!enemy.TryGetComponent<Enemy>(out _))
            {
                Debug.LogWarning(
                    $"WaveData '{name}' slot {i} does not contain an Enemy component",
                    this
                );
            }
        }
    }
}
