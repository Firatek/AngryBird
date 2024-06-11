using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; 

    public int MaxNumberOfShots = 3;

    private int _usedNumberOfShots;
    [SerializeField] private float _waitOfLastShot = 1f;

    private IconHandler _iconHandler;

    private List<Porky> _porkies = new List<Porky>();

    [SerializeField] private GameObject _restartScreenObject;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }

        _iconHandler = FindObjectOfType<IconHandler>();
        Porky[] porkies = FindObjectsOfType<Porky>();
        foreach(Porky p in porkies) {
            _porkies.Add(p);
        }
        Debug.Log(_porkies + "lenght = " + _porkies.Count);

    }

    public Boolean isShotAvailable() {
        return _usedNumberOfShots < MaxNumberOfShots;
    }

    public void UseShot() {
        _usedNumberOfShots++;
        _iconHandler.UseShot(_usedNumberOfShots);
        CheckForLastShot();
    }

    public void CheckForLastShot() {
        if(_usedNumberOfShots == MaxNumberOfShots) {
            StartCoroutine(checkAfterWaitTime());
        }
    }

    private IEnumerator checkAfterWaitTime() {
        yield return new WaitForSeconds(_waitOfLastShot);

        if (_porkies.Count <= 0) {
            WinGame();
        } else {
            LoseGame();
        }
    }

    public void RemovePorky(Porky porky) {
        _porkies.Remove(porky);
        Debug.Log(_porkies.ToArray());
        checkForWin();
    }



    #region Win/Lose
    private void checkForWin() {
        Debug.Log(_porkies.Count);
        if (_porkies.Count <= 0) {
            WinGame();
        }

    }

    private void WinGame() {
        Debug.Log("win");
        _restartScreenObject.SetActive(true);
    }
    private void LoseGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
