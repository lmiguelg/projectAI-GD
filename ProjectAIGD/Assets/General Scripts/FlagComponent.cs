using System.Collections;
using General_Scripts.Enums;
using General_Scripts.Labourers;
using UnityEngine;
using UnityEngine.UI;

namespace General_Scripts
{
    public class FlagComponent : WorkComponent
    {
        public Text TeamAScore;
        public Text TeamBScore;

        private Runner _runner;

        public void PickUp(Runner runner)
        {
            _runner = runner;
            transform.SetParent(_runner.transform);
        }

        public void Drop()
        {
            StartCoroutine(Reset());

            transform.parent = null;
        }

        private IEnumerator Reset()
        {
            yield return new WaitForSeconds(2f);
            transform.position = Vector3.zero;
            StoptWorking(_runner);

            if (_runner.MyTeam == Teams.A)
                TeamAScore.text = (int.Parse(TeamAScore.text)+1).ToString();
            else
                TeamBScore.text = (int.Parse(TeamBScore.text) + 1).ToString();

            _runner = null;
        }
    }
}