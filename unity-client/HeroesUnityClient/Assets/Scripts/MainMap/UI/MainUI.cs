using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainUI : MonoBehaviour, IGameUI
{
    [SerializeField] private TMP_Text infoLabel;

    private float _showTime = -1;

    private void Awake()
    {
        GameManager.Current.UI = this;
    }

    public void ShowError(Error error)
    {
        Log.Error(error.ToString());
        if (infoLabel != null)
        {
            _showTime = Time.time;  
            infoLabel.text = error.ToString();
            infoLabel.gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _showTime > 3f)
        {
            infoLabel.gameObject.SetActive(false);
        }
    }
}
