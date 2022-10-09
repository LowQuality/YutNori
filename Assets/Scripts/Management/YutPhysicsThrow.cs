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
    private bool _isDropped;
    private readonly RaycastHit[] _results = new RaycastHit[10];
    
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
        if (yut.transform.position.y < 0)
        {
            _isDropped = true;
            YutPhysicsMode.Yut.Add(YutPhysicsMode.Yut.Count, new Dictionary<int, bool> { { 3, _isMarked } });
            Destroy(gameObject);
        }
        
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * 90.0f, Color.red);
        var hits = _isStartedCoroutine ? 0 : Physics.RaycastNonAlloc(transform.position + Vector3.up * 0.1f, Vector3.down, _results, 90.0f);
        
        if (_isStartedCoroutine) return;
        for (var i = 0; i < hits; i++)
        {
            if (!_results[i].collider.CompareTag("Ground")) continue;
            _isStartedCoroutine = true;
            StartCoroutine(UpdateYut());
        }
    }

    private IEnumerator UpdateYut()
    {
        yield return new WaitForSeconds(1.0f);
        yutRigidbody.isKinematic = true;

        if (_isDropped) yield break;
        // is not Drop
        var anglesZ = yut.transform.eulerAngles.z % 360;
        YutPhysicsMode.Yut.Add(YutPhysicsMode.Yut.Count,
            anglesZ is > -10 and < 10 or > 340 and < 360
                ? new Dictionary<int, bool> { { 2, _isMarked } }
                : new Dictionary<int, bool> { { 1, _isMarked } });
    }
}
}