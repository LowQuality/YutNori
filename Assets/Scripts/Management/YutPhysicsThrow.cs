using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using Random = System.Random;

namespace Management
{
public class YutPhysicsThrow : MonoBehaviour
{
    public GameObject yut;
    public Rigidbody yutRigidbody;
    public GameObject marked;

    public int a;
    public int b;
    public int c;
    public int d;
    public int aA;
    public int bB;
    public int cC;
    public int dD;
    
    private bool _isStartedCoroutine;
    private bool _isMarked;
    
    // !! Notice !! //
    // Front = Curved Side, Back = Straight Side, Drop = Dropped
    // !! Notice !! //
    
    public void Start()
    {
        if (marked != null) _isMarked = true;
        
        var random = new Random();
        yutRigidbody.AddForce(new Vector3(random.Next(-a, a), b, random.Next(-c, c)) * d, ForceMode.VelocityChange);
        yutRigidbody.AddTorque(new Vector3(random.Next(-aA, aA), random.Next(-bB, bB), random.Next(-cC, cC)) * dD, ForceMode.VelocityChange);
    }
    
    public void Update()
    {
        if (!(yut.transform.position.y < -100)) return;
        YutPhysicsMode.Yut.Add(YutPhysicsMode.Yut.Count, new Dictionary<int, bool> { { 3, _isMarked } });
        foreach (var t in YutPhysicsMode.YutGameObject)
        {
            Destroy(t);
        }
            
        BoardGame.MoveCount = 0;
        BoardGame.DoubleChance = false;
        BoardGame.ShowedValue = true;
        BoardGame.DroppedYut = true;
    }

    private IEnumerator UpdateYut()
    {
        yield return new WaitForSeconds(1.5f);
        var anglesZ = yut.transform.eulerAngles.z;
        anglesZ %= 360;
        YutPhysicsMode.Yut.Add(YutPhysicsMode.Yut.Count,
            anglesZ is > -10 and < 10 or > 340 and < 360
                ? new Dictionary<int, bool> { { 2, _isMarked } }
                : new Dictionary<int, bool> { { 1, _isMarked } });
    }

    public void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Ground") || _isStartedCoroutine) return;
        StartCoroutine(UpdateYut());
        _isStartedCoroutine = true;
    }
}
}