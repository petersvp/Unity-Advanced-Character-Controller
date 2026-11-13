using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.TinyCharacterController.Check;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace Unity.TinyCharacterController.Test
{
    public class AimCheckTest : SceneTestBase
    {
        protected override string ScenePath => TestConstants.PackagePath + "Scenes/AimCheckTest.unity";
        

        [UnityTest]
        public IEnumerator DoesNotHitOwnCollider()
        {
            var aimCheck = FindComponent<AimCheck>("Player_1");
            var hitWall = GameObject.Find("Wall_1");
            
            Assert.That(hitWall, Is.Not.Null);
            yield return null;
            
            Assert.That(aimCheck.PreAimGameObject, Is.Null);
            Assert.That(aimCheck.AimGameObject, Is.EqualTo(hitWall));
        }

        [UnityTest]
        public IEnumerator MovesViewpoint()
        {
            var aimCheck = FindComponent<AimCheck>("Player_1");
            var hitWall = GameObject.Find("Wall_2");

            aimCheck.AimTargetViewportPoint = new Vector2(0, 0.5f);

            Assert.That(hitWall, Is.Not.Null);
            yield return null;
            
            Assert.That(aimCheck.AimGameObject, Is.EqualTo(hitWall));
        }
        
        [UnityTest]
        public IEnumerator LooksAtEmptySpace()
        {
            var aimCheck = FindComponent<AimCheck>("Player_1");

            aimCheck.AimTargetViewportPoint = new Vector2(1, 0.5f);

            yield return null;
            
            Assert.That(aimCheck.AimGameObject, Is.Null);
            Assert.That(aimCheck.IsHit, Is.False);
        }
        
        [UnityTest]
        public IEnumerator ChangingViewpointChangesTargetObject()
        {
            var aimCheck = FindComponent<AimCheck>("Player_1");
            var preHitWall = GameObject.Find("Wall_1");
            var hitWall = GameObject.Find("Wall_2");
            var isCallEvent = false;
            aimCheck.OnChangeAimedGameObject.AddListener(() => isCallEvent = true);
            
            yield return null;

            aimCheck.AimTargetViewportPoint = new Vector2(0.3f, 0.5f);

            yield return null;
            
            Assert.That(aimCheck.IsHit, Is.True);
            Assert.That(aimCheck.IsChangeAImedObject, Is.True);
            Assert.That(isCallEvent, Is.True);

            Assert.That(aimCheck.PreAimGameObject, Is.EqualTo(preHitWall));
            Assert.That(aimCheck.AimGameObject, Is.EqualTo(hitWall));
        }
        
        [UnityTest]
        public IEnumerator ChangingViewpointLooksAtEmptySpace()
        {
            var aimCheck = FindComponent<AimCheck>("Player_1");
            var preHitWall = GameObject.Find("Wall_1");
            Assert.NotNull(preHitWall);

            var isCallEvent = false;
            aimCheck.OnChangeAimedGameObject.AddListener(() => isCallEvent = true);
            aimCheck.AimTargetViewportPoint = new Vector2(1, 0.5f);

            yield return null;
            
            Assert.That(aimCheck.PreAimGameObject, Is.EqualTo(preHitWall));
            Assert.That(aimCheck.AimGameObject, Is.Null);
            Assert.That(aimCheck.IsHit, Is.False);
            Assert.That(aimCheck.IsChangeAImedObject, Is.True);
            Assert.That(isCallEvent, Is.True);
        }

        [UnityTest]
        public IEnumerator TargetDoesNotChangeAfterMovingViewpoint()
        {
            var aimCheck = FindComponent<AimCheck>("Player_1");
            var preHitWall = GameObject.Find("Wall_1");
            
            yield return null;

            aimCheck.AimTargetViewportPoint = new Vector2(1, 0.5f);

            yield return null;
            yield return null;
            
            Assert.That(aimCheck.PreAimGameObject, Is.EqualTo(preHitWall));
            Assert.That(aimCheck.AimGameObject, Is.Null);
            Assert.That(aimCheck.IsChangeAImedObject, Is.False);
        }

        [UnityTest]
        public IEnumerator NoObjectInViewDirection()
        {
            var aimCheck = FindComponent<AimCheck>("Player_1");
            var hitWall = GameObject.Find("Wall_1");
            
            hitWall.SetActive(false);

            yield return null;
            
            Assert.That(aimCheck.PreAimGameObject, Is.EqualTo(hitWall));
            Assert.That(aimCheck.AimGameObject, Is.Null);
            Assert.That(aimCheck.IsHit, Is.False);
            Assert.That(aimCheck.Distance, Is.EqualTo(30).Using(FloatEqualityComparer.Instance));
            
        }

        [UnityTest]
        public IEnumerator TargetOccludedFromPlayer()
        {
            var aimCheck = FindComponent<AimCheck>("Player_1");
            var hitWall = GameObject.Find("Wall_2");

            aimCheck.transform.position = new Vector3(-3f, 0, -5f);

            yield return null;
            
            Assert.That(aimCheck.IsHit);
            Assert.That(aimCheck.AimGameObject, Is.EqualTo(hitWall));
        }
        
        [UnityTest]
        public IEnumerator TargetOccludedFromPlayerWithAimParallaxDisabled()
        {
            var aimCheck = FindComponent<AimCheck>("Player_1");
            var hitWall = GameObject.Find("Wall_1");

            aimCheck.IsUseAimParallax = false;
            aimCheck.transform.position = new Vector3(-3f, 0, -5f);

            yield return null;
            
            Assert.That(aimCheck.IsHit);
            Assert.That(aimCheck.AimGameObject, Is.EqualTo(hitWall));
        }

        [UnityTest]
        public IEnumerator ReducesViewRange()
        {
            var aimCheck = FindComponent<AimCheck>("Player_1");
            aimCheck.MaxRange = 1;
            var hitWall = GameObject.Find("Wall_1");

            
            yield return null;
            
            Assert.That(aimCheck.IsHit, Is.False);
            Assert.That(aimCheck.PreAimGameObject, Is.EqualTo(hitWall));
            Assert.That(aimCheck.AimGameObject, Is.Null);
        }
    }
}
