using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class PlayArcadeGame : MonoBehaviourPunCallbacks
{
    
    public PhotonView PV;
    public GameObject playPanel;
    public Text gameText;
    public bool isPlaying;
    public void Awake()
    {
        PV = this.photonView;
        playPanel.SetActive(false);
    }
    public void Start()
    {
        gameText.text += " Test Scene.";
    }
 
    public void OnTriggerEnter(Collider other)
    {
        if(PV.IsMine)
        {
            if (other.tag == "Player")
            {
                playPanel.SetActive(true);
                isPlaying = true;
                
            }
        }
       
    }
    public void OnTriggerExit(Collider other)
    {
        if (PV.IsMine)
        {
            if (other.tag == "Player")
            {
                playPanel.SetActive(false);
                isPlaying = false;
            }
        }
       
    }
    public void Update()
    {
        if (isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PhotonNetwork.LoadLevel("Test");
            }
        }
       
    }
}
