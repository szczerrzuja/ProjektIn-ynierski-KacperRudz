using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    private GameObject respawnPoint;
    [SerializeField]private int repsawnPointID;
    private HeroController Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player1").GetComponent<HeroController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator respwanOnDelay(){
        yield return new WaitForSeconds(3.0f);
        respwanPlayer();
    }
    void respwanPlayer()
    {
        Player.respawn(ref respawnPoint);
    }
    public void setSpawnPoint(ref GameObject respawnPt){
        respawnPoint = respawnPt;
    }
}
