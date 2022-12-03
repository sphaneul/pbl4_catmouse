using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public InputField NickNameInput, RoomNameInput, RoomCode;
    public Text List_NickName, Room_NickName, Room_RoomName, textPlayerList, textPlayerLog;
    public GameObject GameMainPanel, GameStartPanel, ConnectPanel, RespawnPanel, ListPanel, RoomPanel, AddRoomPanel, JoinRoomPanel, LoginPanel, SignUpPanel, School;
    public Button[] CellBtn;
    public Button PrevBtn, NextBtn, AddBtn, DelBtn, AddRoomBtn, BackBtn, JoinBtn, RandomJoinBtn, JoinRoomBtn;
    public List<string> myList = new List<string>() { };
    List<int> GachaList = new List<int>() { 0, 1, 2, 3, 4 };

    int currentPage = 1, maxPage, multiple;

    public Text msgList;
    public InputField ifSendMsg;
    public Text playerCount;


    void Awake()
    {
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();                                                          
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    //public override void OnConnectedToMaster()
    //{
    //    PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
    //    PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { Max Players = 5 }, null);
    //}
    //public override void OnJoinedRoom()
    //{
    //    //ConnectPanel.SetActive(false);
    //    //ListPanel.SetActive(true);
    //    //Spawn();
    //}

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        //PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null);

        PhotonNetwork.JoinLobby();
        Debug.Log("로비연결완료");

        List_NickName.text = NickNameInput.text + " 님 환영합니다."; // 가능하다면 나중에 닉네임만 bold 처리

        ConnectPanel.SetActive(false);
        ListPanel.SetActive(true);

        //Spawn();

        //Room_NickName.text = NickNameInput.text;
        //Room_RoomName.text = Button.text;

        //ListPanel.SetActive(false);
        //RoomPanel.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            print(roomList[i]);
        }
        //Debug.Log(roomList.Count);

        // 최대페이지
        if (CellBtn.Length == 0)
        {
            maxPage = 1;
        }
        else
            maxPage = (roomList.Count % CellBtn.Length == 0) ? roomList.Count / CellBtn.Length : roomList.Count / CellBtn.Length + 1;

        // 이전, 다음버튼
        PrevBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < roomList.Count) ? true : false;
            CellBtn[i].GetComponentInChildren<Text>().text = (multiple + i < roomList.Count) ? roomList[multiple + i].Name : "";
        }

        //int roomCount = roomList.Count;
        //for (int i = 0; i < roomCount; i++)
        //{
        //    if (!roomList[i].RemovedFromList)
        //    {
        //        if (!roomList.Contains(roomList[i])) roomList.Add(roomList[i]);
        //        else roomList[roomList.IndexOf(roomList[i])] = roomList[i];
        //    }
        //    else if (roomList.IndexOf(roomList[i]) != -1) roomList.RemoveAt(roomList.IndexOf(roomList[i]));
        //}
    }

    public override void OnJoinedRoom()
    {
        print("방 참가 성공");
        UpdateRoomPlayer();

        AddRoomPanel.SetActive(false);
        JoinRoomPanel.SetActive(false);
        ListPanel.SetActive(false);
        RoomPanel.SetActive(true);

        textPlayerLog.text += NickNameInput.text;
        textPlayerLog.text += "님이 방에 참가하였습니다.\n";
        //Spawn();
    }

    public override void OnJoinRoomFailed(short returnCode, string message) => print("방 참가 실패");

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateRoomPlayer();
        textPlayerLog.text += newPlayer.NickName;
        textPlayerLog.text += "님이 입장하였습니다.\n";
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateRoomPlayer();
        textPlayerLog.text += otherPlayer.NickName;
        textPlayerLog.text += "님이 퇴장하였습니다.\n";
    }

    void UpdateRoomPlayer()
    {
        textPlayerList.text = "접속자";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            textPlayerList.text += "\n";
            textPlayerList.text += PhotonNetwork.PlayerList[i].NickName;
        }
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(RoomNameInput.text, new RoomOptions { MaxPlayers = 5 });
        //OnRoomListUpdate(roomList_);

        myList.Add(RoomNameInput.text);
        Room_RoomName.text = "방제목 : " + RoomNameInput.text;
    }

    //public void JoinRoom(int num)
    public void JoinRoom()
    {
        //if (num == -2) --currentPage;
        //else if (num == -1) ++currentPage;
        //else PhotonNetwork.JoinRoom(myList[multiple + num]);
        //else print(myList[multiple + num]);

        //PhotonNetwork.JoinRandomRoom();
        //PhotonNetwork.JoinRoom()

        //string BtnString = EventSystem.current.currentSelectedGameObject;
        //print(clickObject.GetComponentInChildren<Text>().text);

        string BtnName = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        print(BtnName);
        PhotonNetwork.JoinRoom(BtnName);

        //Spawn();
    }

    public void JoinRoomCode()
    {
        PhotonNetwork.JoinRoom(RoomCode.text);
        textPlayerLog.text = "접속로그\n";

        Room_RoomName.text = "방제목 : " + RoomNameInput.text;
    }

    public void JoinRoomRandom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();

        RoomPanel.SetActive(false);
        ListPanel.SetActive(true);
    }


    public void Spawn()
    {
        int rand = Random.Range(0, GachaList.Count);

        //print(GachaList[rand]);

        try
        {
            if (GachaList[rand] == 0)
            {
                PhotonNetwork.Instantiate("Cat", new Vector3(-8f, 1.5f, 0f), Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate("Mouse", new Vector3(-8f, 1.5f, 0f), Quaternion.identity);
            }
        }
        catch (System.ArgumentOutOfRangeException e)
        {
            print("역할배정 완료");
        }

        //GachaList.RemoveAt(rand);

        RespawnPanel.SetActive(false);
    }

    void RoomList()
    {
        //// 최대페이지
        //maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        //// 이전, 다음버튼
        //PrevBtn.interactable = (currentPage <= 1) ? false : true;
        //NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        //// 페이지에 맞는 리스트 대입
        //multiple = (currentPage - 1) * CellBtn.Length;
        //for (int i = 0; i < CellBtn.Length; i++)
        //{
        //    CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
        //    CellBtn[i].GetComponentInChildren<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i] : "";
        //}
    }

    //메인 화면에서 로그인 버튼 클릭시
    //로그인 화면으로 이동
    //메인 화면 끄기
    public void Login()
    {
        GameMainPanel.SetActive(false);
        LoginPanel.SetActive(true);
    }

    //로그인 화면 -> 이메일/비번 입력 후 로그인 클릭하면 
    //게임 시작 버튼이 있는 화면으로 이동
    public void LogintoGameStart()
    {
        LoginPanel.SetActive(false);
        SignUpPanel.SetActive(false);
        GameStartPanel.SetActive(true);
    }

    public void LogintoGameMain()
    {
        LoginPanel.SetActive(false);
        SignUpPanel.SetActive(false);
        GameMainPanel.SetActive(true);
    }

    public void SignUp()
    {
        SignUpPanel.SetActive(true);
        GameMainPanel.SetActive(false);
    }

    public void GameStart()
    {
        RoomPanel.SetActive(false);
        School.SetActive(true);
        Spawn();
        //GameStartPanel.SetActive(true);
        //LoginPanel.SetActive(false);
        //SignUpPanel.SetActive(false);
    }

    public void GameStart_to_List()
    {
        GameStartPanel.SetActive(false);
        ListPanel.SetActive(true);
    }

    public void RoomAddPanel()
    {
        AddRoomPanel.SetActive(true);
    }

    public void RoomJoinPanel()
    {
        JoinRoomPanel.SetActive(true);
    }

    public void RoomRemove()
    {
        myList.Remove(RoomNameInput.text);
        RoomList();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ConnectPanel.SetActive(true);
        RespawnPanel.SetActive(false);
    }


}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public InputField NickNameInput;
    public GameObject ConnectPanel;
    public GameObject RespawnPanel;
    List<int> GachaList = new List<int>() { 0, 1, 2, 3, 4 };

    void Awake()
    {
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null);
    }
    public override void OnJoinedRoom()
    {
        ConnectPanel.SetActive(false);
        Spawn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }

    public void Spawn()
    {
        //PhotonNetwork.Instantiate("Cat", new Vector3(-8f, 1.5f, 0f), Quaternion.identity);

        int rand = Random.Range(0, GachaList.Count);

        //print(GachaList[rand]);

        try
        {
            if (GachaList[rand] == 0)
            {
                PhotonNetwork.Instantiate("Cat", new Vector3(-8f, 1.5f, 0f), Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate("Mouse", new Vector3(-8f, 1.5f, 0f), Quaternion.identity);
            }
        }
        catch (System.ArgumentOutOfRangeException e)
        {
            print("역할배정 완료");
        }


        RespawnPanel.SetActive(false);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ConnectPanel.SetActive(true);
        RespawnPanel.SetActive(false);
    }


}*/