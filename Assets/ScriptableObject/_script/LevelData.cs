using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/Level/LevelData")]
public class LevelData : ScriptableObject
{
    public WaveData[] Waves = new WaveData[3];

    private void OnValidate()
    {
        if (Waves == null || Waves.Length != 3)
            System.Array.Resize(ref Waves, 3);
    }
}
