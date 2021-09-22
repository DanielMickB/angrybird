using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    public enum BirdState { Idle, Thrown, HitSomething }
    public GameObject Parent;
    public Rigidbody2D RigidBody;
    public CircleCollider2D Collider;

    public UnityAction OnBirdDestroyed = delegate { };//harus kosong
    public UnityAction<Bird> OnBirdShot = delegate { };
    
    public BirdState State { get { return _state; } }//fungsi cek state dipanggil di scipt lain

    private BirdState _state;
    private float _minVelocity = 0.05f;
    private bool _flagDestroy = false;

    void OnDestroy()
    {
        if(_state == BirdState.Thrown || _state == BirdState.HitSomething){
            OnBirdDestroyed();
        }
        
    }

    
    private IEnumerator DestroyAfter(float second){
        yield return new WaitForSeconds(second);
        Destroy(gameObject);
    }

    public void MoveTo(Vector2 target, GameObject parent){
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    public void Shoot(Vector2 velocity, float distance, float speed){
        Collider.enabled = true;
        RigidBody.bodyType = RigidbodyType2D.Dynamic;
        RigidBody.velocity = velocity * speed * distance;
        OnBirdShot(this);//manggil fungsi di script trailcontroller
    }

    void OnCollisionEnter2D(Collision2D col){
        _state = BirdState.HitSomething;
    }
    public virtual void OnTap(){// virtual supaya bisa overide 
        //Do nothing
    }
    // Start is called before the first frame update
    void Start()
    {
        RigidBody.bodyType = RigidbodyType2D.Kinematic;
        Collider.enabled = false;
        _state = BirdState.Idle;
        
    }
    void FixedUpdate()
    {
        //kalau keadaan burung lagi idle  dan ada kecepatan maka terlempar
        if(_state == BirdState.Idle && RigidBody.velocity.sqrMagnitude >= _minVelocity){
            _state = BirdState.Thrown;
        }

        if ((_state == BirdState.Thrown || _state == BirdState.HitSomething)&&RigidBody.velocity.sqrMagnitude < _minVelocity &&!_flagDestroy){
            //Hancurkan gameobject setelah 2 detik
            //jika kecepatannya sudah kurang dari batas minimum
            //supaya tidak ada banyak burung yang "tergeletak"
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }

    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
