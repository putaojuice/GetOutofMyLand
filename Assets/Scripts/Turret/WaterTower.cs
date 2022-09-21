using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTower : Turret
{

    [SerializeField] public StatusData acidRainStatusEffect;
    [SerializeField] LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        range = 15.0f;
        firingRate = 0.2f;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    public override void UpdateTarget()
    {
        var enemies = GameObject.FindObjectsOfType<Enemy>();

        foreach(Enemy enemy in enemies) {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if(distance < range){
                enemy.ApplyStatus(acidRainStatusEffect);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        DoRenderer(128, 20.0f); // to draw range around the tower

    }

    // Renders a circle around the tower to indicate range
    public void DoRenderer (int steps, float radius) {

        line.positionCount = steps;
        for(int currStep = 0; currStep < steps; currStep ++){
            float circumferenceProgress = (float)currStep/steps;
            float currRadian = circumferenceProgress * 2 * Mathf.PI;
            float xScaled = Mathf.Cos(currRadian);
            float yScaled = Mathf.Sin(currRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currPosition = transform.position + new Vector3(x,0,y); 
            line.SetPosition(currStep, currPosition);

        }
    }

}
