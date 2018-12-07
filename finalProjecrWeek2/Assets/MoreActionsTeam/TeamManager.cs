using System.Collections;
using System.Collections.Generic;
using General_Scripts.AI.GOAP;
using General_Scripts.Labourers;
using MoreActionsTeam.GoalOrientedBehaviour.Scripts.GameData.Actions;
using SteeringBehaviours.Scripts.Basics;
using UnityEngine;

namespace MoreActionsTeam
{
    public class TeamManager : MonoBehaviour
    {
        public bool WeHaveFlag;

        public List<GoapAgent> MyAgents;
        public List<Runner> MyRunners;

        public void SetTeamNewGoal(string goal)
        {
            foreach (var runner in MyRunners)
            {
                runner.Goals.Clear();
                runner.Goals.Add(goal);
            }
        }

        private void Awake()
        {
            StartCoroutine(RequestNewPlan());
            StartCoroutine(AdaptActionsCosts());
        }

        private IEnumerator RequestNewPlan()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                foreach (var agent in MyAgents)
                {
                    if(agent.NeedNewPlan)
                        agent.GetComponent<SteeringBasics>().Stop();

                    agent.AbortPlan();
                }
            }
        }

        private IEnumerator AdaptActionsCosts()
        {
            while (true)
            {
                foreach (var runner in MyRunners)
                {
                    var steering = runner.GetComponent<SteeringBasics>();
                    runner.GetComponent<DropOffFlag>().Cost = steering.MaxVelocity == 0 ? float.PositiveInfinity : 1 / runner.GetComponent<SteeringBasics>().MaxVelocity;

                    yield return null;
                }
            }
        }
    }
}