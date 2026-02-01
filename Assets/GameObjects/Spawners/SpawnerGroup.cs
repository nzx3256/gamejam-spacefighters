using System;
using UnityEngine;
using UnityEngine.Splines;

[System.Serializable]
public class SpawnerGroup
{
    [SerializeField]
    private bool _spawnForever;
    [SerializeField]
    private int _numToSpawn = 1;
    [SerializeField] [Range(0f,10f)]
    private float _delay = 0.5f;

    public bool spawnForever
    { get => _spawnForever; }
    public int numToSpawn
    { get => _numToSpawn; }
    public float delay
    { get => _delay; }
}