using System.Collections.Generic;
using General_Scripts.AI.GOAP;
using General_Scripts.Labourers;
using UnityEngine;

namespace Assets.TeamDEL.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    public class DefenseAction : GoapAction
    {

        /// <summary>
        /// Target of this action - center of the map
        /// </summary>

        private bool _isDefending;

        public Runner _runner;

        public TeamManager _teamManager;


        //public Transform Target;

        protected override void Awake()
        {
            base.Awake();

            AddPrecondition("hasFlag", false);

            _runner = GetComponent<Runner>();

        }

        public override void Reset()
        {
            _isDefending = false;
            StartTime = 0;
        }

        public override bool IsDone()
        {
            return _isDefending;
        }

        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            print("Defense Action: check iniciado");
           // if (!_teamManager.MyRunners[0].name.Contains(_runner.name)  )//only runner 0 will performe this action (for now)
               // return false;
           // print(_runner.name);
            Vector3 defensePosition;

            if (_teamManager.myTeamName.Contains("B")) //-20 -20
                defensePosition = new Vector3(-20f,0,-20f); 

            else if (_teamManager.myTeamName.Contains("A"))
                defensePosition = new Vector3(20f, 0, 20f);
          
            else
                return false;
            
            Target = new GameObject();
            Target.transform.position = defensePosition;

            print("Defense Action: check feito");

            return true;
        }

        public override bool Perform(GameObject agent)
        {
            if (StartTime == 0)
            {
                AnimManager.Work();
                StartTime = Time.time;
            }
            if (StillWorking())
                return true;

            if (Target == null)
                return false;
            //AnimManager.Move();


            return true;
        }

        public override bool RequiresInRange()
        {
            return true;
        }
    }

}
