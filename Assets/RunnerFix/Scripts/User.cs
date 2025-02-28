using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

public class User : MonoBehaviour
{
    private string _email;
    private string _userName;
    private int score;
    public User(string _email, string _userName, int score)
    {
        this._email = _email;
        this._userName = _userName;
        this.score = score;
    }
}
