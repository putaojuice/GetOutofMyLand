using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade : MonoBehaviour
{   
    [SerializeField]
    private Button saveButton;
    [SerializeField]
    private Button damageButton;
    [SerializeField]
    private Button rangeButton;
    [SerializeField]
    private Button rateButton;
    [SerializeField]
    private TextMeshProUGUI gemCount;
    [SerializeField]
    private TextMeshProUGUI damageTextDisplay;
    [SerializeField]
    private TextMeshProUGUI rangeTextDisplay;
    [SerializeField]
    private TextMeshProUGUI healthTextDisplay;
    [SerializeField]
    private UpgradeManager upgradeManager;


    
    // Start is called before the first frame update
    void Start()
    {
        InitializeText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeText()
    {
        upgradeManager.Load();
        gemCount.text = upgradeManager.data.upgradePoint.ToString();
        damageTextDisplay.text = upgradeManager.data.damage.ToString();
        rangeTextDisplay.text = upgradeManager.data.range.ToString();
        healthTextDisplay.text = upgradeManager.data.health.ToString();
    }

    public void UpgradeDamage()
    {
        if (upgradeManager.data.upgradePoint >= 1) 
        {
            upgradeManager.data.damage += .2f;
            upgradeManager.data.upgradePoint -= 1;
            damageTextDisplay.text = upgradeManager.data.damage.ToString();
            gemCount.text = upgradeManager.data.upgradePoint.ToString();
        }
    }

    public void UpgradeRange()
    {
        if (upgradeManager.data.upgradePoint >= 1) 
        {
            upgradeManager.data.range += .2f;
            upgradeManager.data.upgradePoint -= 1;
            rangeTextDisplay.text = upgradeManager.data.range.ToString();
            gemCount.text = upgradeManager.data.upgradePoint.ToString();
        }
    }

    public void UpgradeHealth()
    {
        if (upgradeManager.data.upgradePoint >= 1) 
        {
            upgradeManager.data.health += 1;
            upgradeManager.data.upgradePoint -= 1;
            healthTextDisplay.text = upgradeManager.data.health.ToString();
            gemCount.text = upgradeManager.data.upgradePoint.ToString();
        }
    }

    public void ClickSave()
    {
        upgradeManager.Save();
    }

    public void ClickBack()
    {
        InitializeText();
    }
}
