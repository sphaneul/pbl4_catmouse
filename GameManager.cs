using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public int stageIndex;
    public CatMove player;
    public GameObject playerPrefab;
    public GameObject[] Stages;

    //생쥐 채력관리
    public int totalPoint;
    public int stagePoint;
    public int health;
    public Image[] UIHealth;

    //Room 이동 가능하게 하는 코드
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //생쥐 체력관리
    public void HealthDown()
    {
        if (health > 0)
        {
            health--;
            Destroy(UIHealth[health]);
        }
        else
        {
            Debug.Log("Mouse Out");
        }
    }

}