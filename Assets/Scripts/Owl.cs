using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owl : Bird
{
    [SerializeField]
    public float _Skill = -200.0f;
    public float _boostForce = 100;
    public bool _hasSkill = false;
    
    

    public void Boost(){
        if (State == BirdState.Thrown && !_hasSkill){
            var body = GetComponent<Rigidbody2D>();
            var impulse = (_Skill * Mathf.Deg2Rad) * body.inertia;
            
            RigidBody.AddTorque( impulse, ForceMode2D.Impulse);
            RigidBody.AddForce(RigidBody.velocity * _boostForce*-1);
            _hasSkill = true;
        }
    }
    public override void OnTap(){//mengoverride fungsi ontap di red
       
        Boost();
    }
}
