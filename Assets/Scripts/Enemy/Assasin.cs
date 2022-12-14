using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Assasin : Enemy
{
    [Header("Assasin Data")]
    [SerializeField]public Material hiddenMat;
    [SerializeField]private Material originalMat;
    [SerializeField]private float skillDuration = 3.0f;
    [SerializeField]private bool skillToggled = false;
    [SerializeField]private Renderer rend;
    [SerializeField]private float hpScaling;
    [SerializeField]private float hpBase;


    // Start is called before the first frame update
    void Start()
    {
        // Setting up the stats for Warriors
        // maxHp = 1f ;
        maxHp = 5.0f + 10.0f * Mathf.Pow(hpBase, (hpScaling * GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().waveIndex + 1));
        hp = maxHp;
        skillCoolDown = 1.5f;
        defence = 0f;
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
        if(skillToggled) {
            if(skillDuration > 0f){
                skillDuration -= Time.deltaTime;
            } else {
                UndoSkill();
                skillDuration = 3.0f;
            }
        }
        skillCoolDown -= Time.deltaTime;

    }

    public override void GetDamaged(float damage) {
        if(skillCoolDown <= 0f) {
            UseSkill();
            skillCoolDown = 5f;
        }
        if(damage - defence > 0f){
            hp -= damage;
        }
    }

    public override void GetStatusDamaged(float damage)
    {
            hp -= damage;

    }

    public override void UseSkill() {

        gameObject.tag = "HiddenEnemy";
        rend.material = hiddenMat;
        skillToggled = true;

    }

    // To undo the skills activated
    public void UndoSkill() {
        
        gameObject.tag = "Enemy";
        rend.material = originalMat;
        skillToggled = false;
    }

}
