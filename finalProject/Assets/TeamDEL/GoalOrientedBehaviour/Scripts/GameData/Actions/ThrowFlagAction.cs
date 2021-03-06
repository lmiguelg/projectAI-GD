﻿using General_Scripts;
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
            
            
            var backpack = agent.GetComponent<BackpackComponent>();
            backpack.Flag.ThrowFlag(agent.transform.forward + agent.transform.up);
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