using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    [SerializeField] public GameObject respawnPoint;
    [SerializeField] private int repsawnPointID;
    private HeroController Player;
    [SerializeField]public GameObject PlayerOBJ;

    // Start is called before the first frame update
    void Start()
    {
        RespawnController tmp = this;

        Player = PlayerOBJ.GetComponent<HeroController>();
        Player.setRespawnController(ref tmp);
        PlayerOBJ.transform.position = respawnPoint.transform.position;

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
