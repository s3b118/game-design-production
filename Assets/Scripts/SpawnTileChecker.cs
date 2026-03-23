using System.Collections.Generic;
using UnityEngine;

public class SpawnTileTracker : MonoBehaviour
{
    Vector3 _position;
    HashSet<Vector3> _occupied;

    public void Initialize(Vector3 pos, HashSet<Vector3> occupiedSet)
    {
        _position = pos;
        _occupied = occupiedSet;
    }

    void OnDestroy()
    {
        if (_occupied != null)
            _occupied.Remove(_position);
    }
}
