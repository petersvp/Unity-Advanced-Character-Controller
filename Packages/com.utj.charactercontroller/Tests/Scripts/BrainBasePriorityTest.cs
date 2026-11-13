using System.Collections;
using NUnit.Framework;
using Unity.TinyCharacterController.Control;
using Unity.TinyCharacterController.Interfaces.Components;
using Unity.TinyCharacterController.Interfaces.Core;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.TinyCharacterController.Test
{
    public class BrainBasePriorityTests : SceneTestBase
    {
        protected override string ScenePath => TestConstants.PackagePath + "Scenes/HighestPriorityTest.unity";

        [UnityTest]
        public IEnumerator BrainControlIsNullImmediatelyAfterComponentUpdate()
        {
            var instance = GameObject.Find("Target_1");
            var control = instance.GetComponent<HighestPriorityTestControl>();
            var brain = instance.GetComponent<IBrain>();

            IMove previousMove = null;
            ITurn previousTurn = null;

            control.OnAcquireHighestMovePriority.AddListener(previous => previousMove = previous);
            control.OnAcquireHighestTurnPriority.AddListener(previous => previousTurn = previous);

            yield return null;

            control.MovePriority = 50;
            control.MovePriority = 50;

            yield return null;

            Assert.AreEqual(null, previousMove);
            Assert.AreEqual(null, previousTurn);
        }

        [UnityTest]
        public IEnumerator CallbackIsCalledWhenComponentIsUpdated()
        {
            var instance = GameObject.Find("Target_1");

            var control = instance.GetComponent<HighestPriorityTestControl>();
            var manualControl = instance.GetComponent<ManualControl>();

            var isCallTurnCallback = false;
            var isCallMoveCallback = false;

            control.OnAcquireHighestMovePriority.AddListener(previous => isCallMoveCallback = true);
            control.OnAcquireHighestTurnPriority.AddListener(previous => isCallTurnCallback = true);

            manualControl.MovePriority = 10;
            manualControl.TurnPriority = 10;
            yield return null;

            control.MovePriority = 50;
            control.TurnPriority = 50;

            yield return null;

            Assert.IsTrue(isCallTurnCallback);
            Assert.IsTrue(isCallMoveCallback);
        }

        [UnityTest]
        public IEnumerator ControlIsNotUpdatedInCallbackWhenUpdatingComponent()
        {
            var instance = GameObject.Find("Target_1");

            var control = instance.GetComponent<HighestPriorityTestControl>();
            var brain = instance.GetComponent<IBrain>();
            var manualControl = instance.GetComponent<ManualControl>();

            IMove previousMove = null;
            ITurn previousTurn = null;

            control.OnAcquireHighestMovePriority.AddListener(previous => previousMove = previous);
            control.OnAcquireHighestTurnPriority.AddListener(previous => previousTurn = previous);

            manualControl.MovePriority = 10;
            manualControl.TurnPriority = 10;
            yield return null;

            control.MovePriority = 50;
            control.TurnPriority = 50;

            yield return null;

            Assert.AreEqual(manualControl, previousMove);
            Assert.AreEqual(manualControl, previousTurn);
        }

        [UnityTest]
        public IEnumerator UpdateTiming()
        {
            var instance = GameObject.Find("Target_1");

            var control = instance.GetComponent<HighestPriorityTestControl>();
            control.MovePriority = 10;

            for (var i = 0; i < 5; i++)
                yield return null;

            Assert.AreEqual(control.MoveUpdateCount, 5);
            Assert.AreEqual(control.TurnUpdateCount, 0);
            control.TurnPriority = 10;

            for (var i = 0; i < 5; i++)
                yield return null;

            Assert.AreEqual(control.MoveUpdateCount, 10);
            Assert.AreEqual(control.TurnUpdateCount, 5);
            control.MovePriority = 0;

            for (var i = 0; i < 5; i++)
                yield return null;

            Assert.AreEqual(control.MoveUpdateCount, 10);
            Assert.AreEqual(control.TurnUpdateCount, 10);
        }

        [UnityTest]
        public IEnumerator BehaviorWhenOtherComponentsHavePriority0()
        {
            var instance = GameObject.Find("Target_1");

            var control = instance.GetComponent<HighestPriorityTestControl>();
            var manualControl = instance.GetComponent<ManualControl>();

            control.MovePriority = 5;
            yield return null;

            Assert.True(control.HasMoveHighestPriority, "After increasing priority");
            control.MovePriority = 0;
            yield return null;

            Assert.False(control.HasMoveHighestPriority, "After setting priority to zero");
        }


        [UnityTest]
        public IEnumerator LowerTurnPriorityWithAnotherComponent()
        {
            var instance = GameObject.Find("Target_1");

            var control = instance.GetComponent<HighestPriorityTestControl>();
            var manualControl = instance.GetComponent<ManualControl>();
            manualControl.TurnPriority = 5;
            control.TurnPriority = 10;
            yield return null;

            Assert.True(control.HasTurnHighestPriority, "IncreasePriority");
            control.TurnPriority = 4;
            yield return null;

            Assert.IsFalse(control.HasTurnHighestPriority, "Immediately after startup");
        }

        [UnityTest]
        public IEnumerator LowerPriorityWithAnotherComponent()
        {
            var instance = GameObject.Find("Target_1");

            var control = instance.GetComponent<HighestPriorityTestControl>();
            var manualControl = instance.GetComponent<ManualControl>();
            control.TurnPriority = 15;
            control.MovePriority = 15;
            manualControl.TurnPriority = 10;
            manualControl.MovePriority = 10;
            yield return null;

            Assert.True(control.HasTurnHighestPriority);
            Assert.True(control.HasMoveHighestPriority);
            control.TurnPriority = 5;
            control.MovePriority = 5;
            yield return null;

            Assert.False(control.HasTurnHighestPriority);
            Assert.False(control.HasMoveHighestPriority);
        }

        [UnityTest]
        public IEnumerator RaisePriorityWithAnotherComponent()
        {
            var instance = GameObject.Find("Target_1");

            var control = instance.GetComponent<HighestPriorityTestControl>();
            var manualControl = instance.GetComponent<ManualControl>();

            manualControl.TurnPriority = 5;
            manualControl.MovePriority = 5;
            yield return null;

            control.TurnPriority = 10;
            control.MovePriority = 10;
            yield return null;

            Assert.True(control.HasTurnHighestPriority);
            Assert.True(control.HasMoveHighestPriority);
        }


        [UnityTest]
        public IEnumerator SetPriorityTo0AfterRaisingIt()
        {
            var instance = GameObject.Find("Target_1");

            var control = instance.GetComponent<HighestPriorityTestControl>();
            control.TurnPriority = 10;
            control.MovePriority = 10;
            yield return null;

            control.TurnPriority = 0;
            control.MovePriority = 0;
            yield return null;

            Assert.IsFalse(control.HasMoveHighestPriority);
            Assert.IsFalse(control.HasTurnHighestPriority);
        }

        [UnityTest]
        public IEnumerator IncreasePriority()
        {
            var instance = GameObject.Find("Target_1");

            var control = instance.GetComponent<HighestPriorityTestControl>();
            control.TurnPriority = 10;
            control.MovePriority = 10;

            yield return null;

            Assert.IsTrue(control.HasTurnHighestPriority);
            Assert.IsTrue(control.HasMoveHighestPriority);
        }
    }
}
