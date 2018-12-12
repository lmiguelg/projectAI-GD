using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using General_Scripts.AI.GOAP;
using Assets.TeamDEL.AI.HSM.Actions;
using Assets.TeamDEL.Pathfinding.Scripts.Pathfinding;
using General_Scripts;
using General_Scripts.Labourers;
using UnityEditorInternal;
using UnityEngine;

namespace Assets.TeamDEL.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    /// <summary>
    /// Go to middle where flag respawn
    /// </summary>
    public class MiddleAction : GoapAction
    {
      

        /// <summary>
        /// Target of this action - center of the map
        /// </summary>

        private bool _goingToMiddle;

        public Runner _runner;

        public TeamManager _teamManager;

        //public Transform Target;

        protected override void Awake()
        {
            base.Awake();

            //AddPrecondition("weHaveFlag", false); // if we dont have the flg we accept the point as lost
            //AddEffect("goingToMiddle", true);//we give up the point and go to middle
            //AddPrecondition("hasFlag", false);
            AddEffect("dropFlag", true);//we give up the point and go to middle


            //// get runner
            var teamManager = GetComponent<TeamManager>();
            //teamManager.SetTeamNewGoal(_runner.Goals[1]);
            //_teamManager.RequestNewPlan();

        }

        public override void Reset()
        {
            _goingToMiddle = false;
            StartTime = 0;
        }

        public override bool IsDone()
        {
            return _goingToMiddle;
        }

        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            //print("middleaction CheckProceduralPrecondition false");
            if (_teamManager.WeHaveFlag)
                return false;
            //print("middleaction CheckProceduralPrecondition true");

            //Target.position = Vector3.up;
            Target = new GameObject();
            Target.transform.position = Vector3.up;
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
            AnimManager.Move();


            return true;
        }

        public override bool RequiresInRange()
        {
            return true;
        }
    }
}

