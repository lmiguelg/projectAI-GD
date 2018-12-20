using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using General_Scripts;
using General_Scripts.AI.GOAP;
using General_Scripts.Labourers;
using SteeringBehaviours.Scripts.Basics;
using UnityEngine;

namespace Assets.TeamDEL.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    public class ThrowFlagAction : GoapAction
    {
        /// <summary>
        /// For the runner, this will have the same effect as scoring a point
        /// </summary>
        private bool _dropFlag;


        /// <summary>
        /// The runner that will perform this action
        /// </summary>
        private Runner _runner;

        public TeamManager _teamManager;
        /// <summary>
        /// Cache for the steering basics of the runner
        /// </summary>
        private SteeringBasics _steering;

        protected override void Awake()
        {
            base.Awake();
            AddPrecondition("hasFlag", true);
            AddEffect("hasFlag", false);
            AddEffect("dropFlag", true);

            _runner = GetComponent<Runner>();
            _steering = GetComponent<SteeringBasics>();
           
        }

        public override void Reset()
        {
            _dropFlag = false;
            StartTime = 0;
        }

        public override bool IsDone()
        {
            return _dropFlag;
        }

        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            return _steering.MaxVelocity == 0; // we decided we want to throw the flag when we are stopped. This can be changed
        }

        public override bool Perform(GameObject agent)
        {
            //print("flag thrown");

            if (StartTime == 0)
            {
                AnimManager.Work();
                StartTime = Time.time;
            }


            // still working
            if (StillWorking())
                return true;
            
            //throw flag to closest teammate
            //get the list of teammates 
            List<Runner> myTeamates = _teamManager.MyRunners.ToList();

            //remove the teammate that has the flag
            myTeamates.Remove(_runner);
            IEnumerable<Runner> my2Runners = myTeamates;

            //get the closest
            Runner _closestRunner;
            Utils.GetClosest(my2Runners, _runner.transform, out _closestRunner);
            

            //get teammate direction
            Vector3 vClosestRunner = (_closestRunner.transform.position - agent.transform.position).normalized;

            var backpack = agent.GetComponent<BackpackComponent>();

            backpack.Flag.ThrowFlag(agent.transform.forward + agent.transform.up);//vClosestRunner

            backpack.Flag = null;
            backpack.HasFlag = false;
            _dropFlag = true; // you have dropped the flag
            AnimManager.GoIdle();

            return true;
        }

        public override bool RequiresInRange()
        {
            return false; // as long as we have the flag, we can throw it
        }
    }
}