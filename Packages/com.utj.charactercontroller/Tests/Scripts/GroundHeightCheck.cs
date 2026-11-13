using System.Collections;
using NUnit.Framework;
using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Interfaces.Components;
using Unity.TinyCharacterController.Interfaces.Core;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace Unity.TinyCharacterController.Test
{

    public class GroundHeightCheckTests : SceneTestBase
    {
        protected override string ScenePath => TestConstants.PackagePath + "Scenes/GroundHeightCheck.unity";

        [UnityTest]
        public IEnumerator IsGrounded()
        {
            // position.y = 0.0f, _toleranceHeight = 0.5f
            var groundCheck = FindComponent<GroundHeightCheck>("Player_1");

            yield return null;

            Assert.That(groundCheck.IsOnGround, Is.True);
            Assert.That(groundCheck.IsFirmlyOnGround, Is.True);
        }


        [UnityTest]
        public IEnumerator SomeDistanceFromTheGround()
        {
            // position.y = 0.2f, _toleranceHeight = 0.5f
            var groundCheck = FindComponent<GroundHeightCheck>("Player_2");
            
            yield return null;

            Assert.That(groundCheck.IsOnGround, Is.True);
            Assert.That(groundCheck.IsFirmlyOnGround, Is.False);
        }

        [UnityTest]
        public IEnumerator OffTheGround()
        {
            // position.y = 1f, _toleranceHeight = 0.5f
            var groundCheck = FindComponent<GroundHeightCheck>("Player_3");

            yield return null;

            Assert.That(groundCheck.IsOnGround, Is.False);
            Assert.That(groundCheck.IsFirmlyOnGround, Is.False);
        }

        
        [UnityTest]
        public IEnumerator GroundHeightSlightlyAboveGround()
        {
            var groundCheck = FindComponent<GroundHeightCheck>("Player_1");
            groundCheck.GroundHeight = -0.1f;

            yield return null;

            Assert.That(groundCheck.IsOnGround, Is.True);
            Assert.That(groundCheck.IsFirmlyOnGround, Is.False);
        }
        
        [UnityTest]
        public IEnumerator GroundHeightFarAboveGround()
        {
            var groundCheck = FindComponent<GroundHeightCheck>("Player_1");
            groundCheck.GroundHeight = -1f;

            yield return null;

            Assert.That(groundCheck.IsOnGround, Is.False);
            Assert.That(groundCheck.IsFirmlyOnGround, Is.False);
        }
        
        [UnityTest]
        public IEnumerator GroundHeightBelowGround()
        {
            var groundCheck = FindComponent<GroundHeightCheck>("Player_1");
            groundCheck.GroundHeight = 0.1f;

            yield return null;


            Assert.That(groundCheck.IsOnGround, Is.True);
            Assert.That(groundCheck.IsFirmlyOnGround, Is.True);
        }


        [UnityTest]
        public IEnumerator MovesPositionSlightlyAway()
        {
            var groundCheck = FindComponent<GroundHeightCheck>("Player_1");
            groundCheck.GetComponent<ITransform>().Position = (new Vector3(0, 0.1f, 0));

            yield return null;

            
            Assert.That(groundCheck.IsOnGround, Is.True);
            Assert.That(groundCheck.IsFirmlyOnGround, Is.False);
        }
        
        [UnityTest]
        public IEnumerator MovesPositionFarAway()
        {
            var groundCheck = FindComponent<GroundHeightCheck>("Player_1");

            yield return null;

            groundCheck.GetComponent<ITransform>().Position = (new Vector3(0, 1f, 0));

            yield return null;

            Assert.That(groundCheck.IsOnGround, Is.False);
            Assert.That(groundCheck.IsFirmlyOnGround, Is.False);
        }

        [UnityTest]
        public IEnumerator MeasuresDistanceToGround([Values(0, 0.1f, -0.1f, -1f, 1f)] float height)
        {
            var groundCheck = FindComponent<GroundHeightCheck>("Player_1");
            groundCheck.GroundHeight = height;

            yield return null;
            
            Assert.That(
                groundCheck.DistanceFromGround, 
                Is.EqualTo(-height).Using(FloatEqualityComparer.Instance));
        }

        [UnityTest]
        public IEnumerator GroundPosition()
        {
            // position : (2, 1, 0)
            var groundCheck = FindComponent<GroundHeightCheck>("Player_3");
            groundCheck.GroundHeight = -1;

            yield return null;

            Assert.That( 
                groundCheck.GroundContactPoint, 
                Is.EqualTo(new Vector3(2, -1, 0)).Using(Vector3EqualityComparer.Instance));
        }
    }
}