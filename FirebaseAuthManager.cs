using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;

public class FirebaseAuthManager : MonoBehaviour
{

    private FirebaseAuth auth; // 로그인, 회원가입 등에 사용
    private FirebaseUser user; // 인증이 완료된 유저 정보

    public InputField email;
    public InputField password;


    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Create()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("회원가입 취소");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("회원가입 실패:" + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            //Debug.LogError("회원가입 완료" + task.Exception);
            //Debug.LogFormat("회원가입 완료: {0} ({1})", newUser.DisplayName, newUser.UserId);
        });
    }

    public void Login()
    {
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                //Debug.LogError("로그인 취소");
                return;
            }
            if (task.IsFaulted)
            {
                //Debug.LogError("로그인 실패");
                return;
            }

            FirebaseUser newUser = task.Result;
            //Debug.LogError("로그인 완료");
        });

    }

    public void LogOut()
    {
        auth.SignOut();
        Debug.Log("로그아웃");
    }
}