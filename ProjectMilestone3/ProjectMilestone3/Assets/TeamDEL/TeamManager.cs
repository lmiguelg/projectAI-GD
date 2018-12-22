using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.TeamDEL.GoalOrientedBehaviour.Scripts.GameData.Actions;
using General_Scripts.AI.GOAP;
using General_Scripts.Labourers;
using SteeringBehaviours.Scripts.Basics;
using UnityEngine;

namespace Assets.TeamDEL
{
    public class TeamManager : MonoBehaviour
    {
        public bool WeHaveFlag;

        public List<GoapAgent> MyAgents;
        public List<Runner> MyRunners;
        public Runner runnerCarrier;

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
            StartCoroutine(CheckFlag());
            StartCoroutine(CheckRunnerCarrier());
            //StartCoroutine(teste());

        }

        private IEnumerator teste()
        {
            yield return null;
            while (true)

            foreach (var runner in MyRunners)
            {
                if (runner.Equals(runnerCarrier))
                {
                    SetTeamNewGoal("attacar");
                    yield return null;
                }

                print("goals" + runner.Goals);
            }

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
            yield return null;
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
		private IEnumerator CheckFlag()
        {
            yield return null;
            while (true)
            {
                WeHaveFlag = MyRunners.Any(runner => runner.Backpack.HasFlag);

                yield return null;
            }
        }
        public IEnumerator CheckRunnerCarrier()
        {
            yield return null;
            while (true)
            {
                runnerCarrier = MyRunners.Find(runner => runner.Backpack.HasFlag);
                
                    
                print("corroutine runner carrirer: " + runnerCarrier);

                yield return null;
            }
        }


        public void setTeamNewGoal(string goal, string name)
        {
            var run = MyRunners.First(runner => runner.name == name);
            run.Goals.Clear();
            run.Goals.Add(goal);
        }
    }
}