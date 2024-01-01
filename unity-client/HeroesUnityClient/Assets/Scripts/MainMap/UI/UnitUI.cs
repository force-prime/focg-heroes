using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitUI : MonoBehaviour
{
    [SerializeField] private TMP_Text countLabel;
    [SerializeField] private Image icon;

    public void Attach(IUnitDescription unitDescription, int count)
    {
        if (unitDescription == null)
        {
            //UnityEngine.Debug.LogError("Null unity description");
            Hide();
            return;
        }

        gameObject.SetActive(true);
        icon.sprite = unitDescription.GetIcon();
        countLabel.text = count.ToString();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        
    }
}
