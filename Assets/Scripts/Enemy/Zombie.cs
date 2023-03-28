using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private float _hp;
    [SerializeField]
    ZombieData zombieData;
    private void Start()
    {
        _hp = zombieData.Hp;
    }
    [ContextMenu("attack zombie")]
    public void TEST()
    {
        _hp -= 10;
        Debug.Log(_hp);
    }
}
