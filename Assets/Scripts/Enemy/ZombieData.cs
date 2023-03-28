using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Zombie Data", menuName = "Scriptable Object/Zombie Data", order = int.MaxValue - 1)]
public class ZombieData : ScriptableObject
{
    [SerializeField] private string _monsterType;
    [SerializeField] private Transform _monsterTransform;
    [SerializeField] private float _hp;
    [SerializeField] private float _dmg;
    [SerializeField] private float _movespeed;

    public string MonsterType { get { return _monsterType; } }
    public Transform MonsterTransform { get { return _monsterTransform; } }
    public float Hp { get { return _hp; } set { _hp = value; } }
    public float Dmg { get { return _dmg; } }
    public float MoveSpeed { get { return _movespeed; } }
}