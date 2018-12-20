using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using General_Scripts;
using General_Scripts.AI.GOAP;
using General_Scripts.Labourers;
using UnityEngine;
using Runner = General_Scripts.Labourers.Runner;

namespace Assets.TeamDEL.GoalOrientedBehaviour.Scripts.GameData.Actions
{
    public class SecondRunnerAction:GoapAction
    {

        private bool _isFollowingRunner;

        public TeamManager _teamManager;

        private FlagComponent _flag;

        private Runner thisRunner;




        private Runner runnerCarrier;

     

        protected override void Awake()
        {
            base.Awake();

            

            AddPrecondition("WeHaveFlag", true);
            AddPrecondition("_isFollowingRunner", false);
            AddEffect("_isFollowingRunner", true);


            // follow the runner tha has the flag
            _flag = FindObjectOfType<FlagComponent>();
            thisRunner = GetComponent<Runner>();

            



        }

        public override void Reset()
        {
            _isFollowingRunner = false;
            StartTime = 0;
        }

        public override bool IsDone()
        {
            return _isFollowingRunner;
        }


        /// <summary>
        /// Checks if the agent need to be in range of the target to complete this action.
        /// </summary>
        /// <returns></returns>
        public override bool RequiresInRange()
        {
            return true; // yes we need to be near the flag to pick it up  
        }

        /// <summary>
        /// Checks if there is a <see cref="ChoppingBlockComponent"/> close to the agent.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool CheckProceduralPrecondition(GameObject agent)
        {
            // the flag will "be worked" if there is some1 carrying it. Otherwise it is free to be picked up.
            if (_flag.Carrier == null)
                return false;
            if (_teamManager.runnerCarrier == null)
                return false;
            Runner runnerFollower;
            Utils.GetClosest(_teamManager.MyRunners, _teamManager.runnerCarrier.transform, out runnerFollower);
            if (thisRunner.Equals(runnerFollower))//if this runner isnt the closest do nothing
                return false;

            Target = _teamManager.runnerCarrier.gameObject;



            print("SECOND ACTION checkProceduralCondition");
            return true;
        }

        /// <summary>
        /// Once the WorkDuration is compelted, adds 5 FireWood to the agent's backpack.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public override bool Perform(GameObject agent)
        {
            if (StartTime == 0)
            {
                AnimManager.Work();
                StartTime = Time.time;
            }

            // still working
            if (StillWorking())
                return true;

            if (Target == null)
                return false;

            if (_flag.CanBeWorked == false) return false;
            _flag.StartWorking(GetComponent<Labourer>());

            //_isFollowingRunner = true;
            //AnimManager.GoIdle();

            return true;
        }
       


    }
}
