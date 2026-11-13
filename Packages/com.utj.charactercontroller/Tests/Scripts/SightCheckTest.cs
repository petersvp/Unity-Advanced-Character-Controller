using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.TinyCharacterController.Check;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.TinyCharacterController.Test
{
    public class SightCheckTest : SceneTestBase
    {
        protected override string ScenePath => TestConstants.PackagePath + "Scenes/SightCheckTest.unity";


        [UnityTest]
        public IEnumerator FindObjectsWithinSight()
        {
            var sightCheck = FindComponent<SightCheck>("Enemy_1");
            
            yield return null;
            
            Assert.That(sightCheck.IsInsightAnyTarget, Is.True); 
        }
        
        [UnityTest]
        public IEnumerator FindsOnlyObjectsWithMatchingTag()
        {
            var sightCheck = FindComponent<SightCheck>("Enemy_2");
            var player2 = GameObject.Find("Player_2");
            var player3 = GameObject.Find("Player_3");
            
            yield return null;
            
            Assert.That(sightCheck.IsInsightAnyTarget, Is.True); 
            Assert.That(sightCheck.InsightTargets.Count, Is.EqualTo(2));
            Assert.That(sightCheck.InsightTargets.Contains(player2), Is.True);
            Assert.That(sightCheck.InsightTargets.Contains(player3), Is.True);
        }

        [UnityTest]
        public IEnumerator ObstacleBlocksSight()
        {
            var sightCheck = FindComponent<SightCheck>("Enemy_3");
            yield return null;
            
            Assert.That(sightCheck.IsInsightAnyTarget, Is.False);
        }
        
        [UnityTest]
        public IEnumerator ObstacleDoesNotBlockSightWhenRaycastDisabled()
        {
            var sightCheck = FindComponent<SightCheck>("Enemy_4");
            yield return null;
            
            Assert.That(sightCheck.IsInsightAnyTarget, Is.True);
        }
        
        [UnityTest]
        public IEnumerator ObjectExistsButTagsDoNotMatch()
        {
            var sightCheck = FindComponent<SightCheck>("Enemy_5");
            yield return null;
            
            Assert.That(sightCheck.IsInsightAnyTarget, Is.False);
        }
        
        [UnityTest]
        public IEnumerator IncludedInVisibleLayerMask()
        {
            var sightCheck = FindComponent<SightCheck>("Enemy_6");
            yield return null;
            
            Assert.That(sightCheck.IsInsightAnyTarget, Is.True);
        }
        
        [UnityTest]
        public IEnumerator ShieldedByEnvironmentLayer()
        {
            var sightCheck = FindComponent<SightCheck>("Enemy_7");
            yield return null;
            
            Assert.That(sightCheck.IsInsightAnyTarget, Is.False);
        }
     
        [UnityTest]
        public IEnumerator IncludedInEnvironmentLayerButNotIncludedInVisibleLayerMask()
        {
            var sightCheck = FindComponent<SightCheck>("Enemy_8");
            yield return null;
            
            Assert.That(sightCheck.IsInsightAnyTarget, Is.False);
        }
        
        [UnityTest]
        public IEnumerator TargetCountChanges()
        {
            var sightCheck = FindComponent<SightCheck>("Enemy_9");
            var player10 = GameObject.Find("Player_10");
            var isChangeInsightAnyTargetState = false;
            
            sightCheck.OnChangeInsightAnyTargetState.AddListener((c) =>
            {
                isChangeInsightAnyTargetState = true;
            });
            
            player10.SetActive(false);
            
            Assert.That(sightCheck.IsInsightAnyTarget, Is.True);
            yield return null;
            
            // Assert.That(sightCheck.IsInsightAnyTarget, Is.False);
            Assert.That(isChangeInsightAnyTargetState, Is.True);
        }
        
        [UnityTest]
        public IEnumerator VisibleDistanceDecreases()
        {
            var sightCheck = FindComponent<SightCheck>("Enemy_9");
            sightCheck.Range = 1;

            
            Assert.That(sightCheck.IsInsightAnyTarget, Is.True);
            yield return null;
            
            Assert.That(sightCheck.IsInsightAnyTarget, Is.False);
        }
        
        [UnityTest]
        public IEnumerator FieldOfViewNarrows()
        {
            var sightCheck = FindComponent<SightCheck>("Enemy_9");
            sightCheck.Angle = 10;
            
            Assert.That(sightCheck.IsInsightAnyTarget, Is.True);
            yield return null;
            
            Assert.That(sightCheck.IsInsightAnyTarget, Is.False);
        }
    }
}
