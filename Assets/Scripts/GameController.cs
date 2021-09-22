using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public SlingShooter SlingShooter;
    public TrailController TrailController;
    public List<Bird> Birds;//daftarin array bird
    

    public List<Enemy> Enemies;//daftarin array enemy, buat cek msh ada musuh?
    private Bird _shotBird;
    public BoxCollider2D TapCollider;
    
    private bool _isGameEnded = false;

    public void ChangeBird(){
        TapCollider.enabled = false;//supaya g bisa diklik setelah reload
        if (_isGameEnded){//buat berentiin ganti burung kalo udah kelar
            return;
        }
        Birds.RemoveAt(0);

        if(Birds.Count > 0){
            SlingShooter.InitiateBird(Birds[0]);
        }
        _shotBird = Birds[0];
    }
    public void CheckGameEnd(GameObject destroyedEnemy){
        for(int i = 0; i < Enemies.Count; i++){
            if(Enemies[i].gameObject == destroyedEnemy){
                Enemies.RemoveAt(i);
                break;
            }
        }

        if(Enemies.Count == 0){
            _isGameEnded = true;
        }
    }
    public void AssignTrail(Bird bird){
        TrailController.SetBird(bird);
        StartCoroutine(TrailController.SpawnTrail());
        TapCollider.enabled = true;//supaya bisa di klik setelah ditembak
    }
    void OnMouseUp(){
        if(_shotBird != null){
            _shotBird.OnTap();//buat aktifin skill
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < Birds.Count; i++){
            Birds[i].OnBirdDestroyed += ChangeBird;//buat ngambil bird selanjutnya
            Birds[i].OnBirdShot += AssignTrail;//ninggalin jejak
        }
        for(int i = 0; i < Enemies.Count; i++){
            Enemies[i].OnEnemyDestroyed += CheckGameEnd;//cek apa masih ada musuh?
        }
        TapCollider.enabled = false;//supaya ketapel bisa ketarik
        SlingShooter.InitiateBird(Birds[0]);//nge reload ketapel dengan bird
        _shotBird = Birds[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
