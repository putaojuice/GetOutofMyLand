using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Tank : Enemy
{

    [Header("Tank Data")]
    [SerializeField]private float skillDuration = 3.0f;
    [SerializeField]public bool skillToggled = false;
    [SerializeField]private Renderer rend;
    [SerializeField]public Material aggroMat;
    [SerializeField]private Material originalMat;
    [SerializeField]private float hpScaling;
    [SerializeField]private float hpBase;

    // Start is called before the first frame update
    void Start()
    {

        // Setting up the stats for Warriors
        // maxHp = 3.0f * (GameManager.instance.waveIndex + 1) * 0.5f;
        maxHp = 5.0f + 10.0f * Mathf.Pow(hpBase, (hpScaling * GameManager.instance.waveIndex + 1));
        hp = maxHp;
        skillCoolDown = 2f;
        defence = 2f;


        rend = GetComponentInChildren<Renderer>();
        originalMat = rend.material;

    }

    // Update is called once per frame
    void Update()
    {

        if(hp <= 0.0f){
            Destroy(gameObject);
            UpdateNumOfEnemies();
            return;
        }

        // Skill check
        if(!skillToggled){
            if(skillCoolDown <= 0f) {
                UseSkill();
                skillCoolDown = 4.0f;
            }
        } else {
            if(skillDuration > 0f){
                skillDuration -= Time.deltaTime;
            } else {
                UndoSkill();
                skillDuration = 2.0f;
            }
        }

        // to be changed to scheduler
        skillCoolDown -= Time.deltaTime;
    }

    // Charge Skill will be called
    public override void UseSkill() {
        rend.material = aggroMat;
        skillToggled = true;

    }

    // To undo the skills activated
    public void UndoSkill() {
        rend.material = originalMat;
        skillToggled = false;
    }

}
