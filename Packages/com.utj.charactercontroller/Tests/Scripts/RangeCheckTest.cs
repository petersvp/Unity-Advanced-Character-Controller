using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Interfaces.Components;
using Unity.TinyCharacterController.Interfaces.Core;
using UnityEngine;
using UnityEngine.TestTools;

namespace Unity.TinyCharacterController.Test
{
    public class RangeCheckTest : SceneTestBase
    {
        // A Test behaves as an ordinary method
        [Test]
        public void RangeCheckTestSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        [UnityTest]
        public IEnumerator StartsWithMultipleObjects()
        {
            var root = GetObjectAndComponentInRoot<Transform>("Test_1");
            root.gameObject.SetActive(true);
            var rangeTargetCheck = FindComponent<RangeTargetCheck>("Character_1");

            yield return null;
            
            var targets = rangeTargetCheck.GetTargets(0);
            Assert.That(targets.Count, Is.EqualTo(4));
            Assert.That(targets.Exists(item => item.name == "Sphere_1"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_2"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_3"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_4"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_5"), Is.False);
        }
        
        [UnityTest]
        public IEnumerator FindObjectsWithinCameraRange()
        {
            var root = GetObjectAndComponentInRoot<Transform>("Test_2");
            root.gameObject.SetActive(true);
            var rangeTargetCheck = FindComponent<RangeTargetCheck>("Character_1");
            var targets = rangeTargetCheck.GetTargets(0);

            yield return null;
            
            Assert.That(targets.Count, Is.EqualTo(3));
            Assert.That(targets.Exists(item => item.name == "Sphere_1"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_2"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_3"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_4"), Is.False);
            Assert.That(targets.Exists(item => item.name == "Sphere_5"), Is.False);
        }
        
        [UnityTest]
        public IEnumerator FindObjectsNotHiddenByObstaclesFromTheCamera()
        {
            var root = GetObjectAndComponentInRoot<Transform>("Test_3");
            root.gameObject.SetActive(true);
            var rangeTargetCheck = FindComponent<RangeTargetCheck>("Character_1");
            var targets = rangeTargetCheck.GetTargets(0);

            yield return null;
            
            Assert.That(targets.Count, Is.EqualTo(3));
            Assert.That(targets.Exists(item => item.name == "Sphere_1"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_2"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_3"), Is.False);
            Assert.That(targets.Exists(item => item.name == "Sphere_4"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_5"), Is.False);
        }

        [UnityTest]
        public IEnumerator FindsObjectsVisibleFromPlayer()
        {
            var root = GetObjectAndComponentInRoot<Transform>("Test_4");
            root.gameObject.SetActive(true);
            var rangeTargetCheck = FindComponent<RangeTargetCheck>("Character_1");
            var targets = rangeTargetCheck.GetTargets(0);

            yield return null;
            
            Assert.That(targets.Count, Is.EqualTo(3));
            Assert.That(targets.Exists(item => item.name == "Sphere_1"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_2"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_3"), Is.False);
            Assert.That(targets.Exists(item => item.name == "Sphere_4"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_5"), Is.False);
        }

        [UnityTest]
        public IEnumerator FindsObjectsVisibleFromPlayerAndCamera()
        {
            var root = GetObjectAndComponentInRoot<Transform>("Test_5");
            root.gameObject.SetActive(true);
            var rangeTargetCheck = FindComponent<RangeTargetCheck>("Character_1");
            var targets = rangeTargetCheck.GetTargets(0);

            yield return null;
            
            Assert.That(targets.Count, Is.EqualTo(2));
            Assert.That(targets.Exists(item => item.name == "Sphere_1"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_2"), Is.False);
            Assert.That(targets.Exists(item => item.name == "Sphere_3"), Is.False);
            Assert.That(targets.Exists(item => item.name == "Sphere_4"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_5"), Is.False);
        }
        
        [UnityTest]
        public IEnumerator MoveTheCharacter()
        {
            var root = GetObjectAndComponentInRoot<Transform>("Test_6");
            root.gameObject.SetActive(true);
            var rangeTargetCheck = FindComponent<RangeTargetCheck>("Character_1");
            var targets = rangeTargetCheck.GetTargets(0);
            
            rangeTargetCheck.GetComponent<ITransform>().Position = (new Vector3(-3, 0, -3));

            yield return null;
            
            Assert.That(targets.Count, Is.EqualTo(1));
            Assert.That(targets.Exists(item => item.name == "Sphere_1"), Is.False);
            Assert.That(targets.Exists(item => item.name == "Sphere_2"), Is.False);
            Assert.That(targets.Exists(item => item.name == "Sphere_3"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_4"), Is.False);
            Assert.That(targets.Exists(item => item.name == "Sphere_5"), Is.False);
        }
        [UnityTest]
        public IEnumerator FindsChangedTargetsAfterMovingCharacter()
        {
            var root = GetObjectAndComponentInRoot<Transform>("Test_7");
            root.gameObject.SetActive(true);
            var rangeTargetCheck = FindComponent<RangeTargetCheck>("Character_1");

            yield return null;
            
            rangeTargetCheck.GetComponent<ITransform>().Position = (new Vector3(1, 0, -5));
            var targets = rangeTargetCheck.GetTargets(0);

            yield return null;

            var result = rangeTargetCheck.ChangedValues(0, out var add, out var remove);
            
            Assert.That(result, Is.True, "The value did not change.");
            Assert.That(targets.Count, Is.EqualTo(3));
            Assert.That(add.Count, Is.EqualTo(1));
            Assert.That(remove.Count, Is.EqualTo(1));

            Assert.That(add.Exists(item => item.name == "Sphere_3"), Is.True);
            Assert.That(remove.Exists(item => item.name == "Sphere_1"), Is.True);
        }
        
        
        
        [UnityTest]
        public IEnumerator DisablesObject()
        {
            var root = GetObjectAndComponentInRoot<Transform>("Test_8");
            root.gameObject.SetActive(true);
            var rangeTargetCheck = FindComponent<RangeTargetCheck>("Character_1");
            var targets = rangeTargetCheck.GetTargets(0);

            yield return null;
            
            var sphere = GameObject.Find("Sphere_1");
            sphere.SetActive(false);

            yield return null;
            
            var result = rangeTargetCheck.ChangedValues(0, out var add, out var remove);
            Assert.That(result, Is.True, "The value did not change.");

            Assert.That(remove.Count, Is.EqualTo(1));
            Assert.That(remove.Exists(item => item.name == "Sphere_1"));
            
            Assert.That(targets.Count, Is.EqualTo(3));
            Assert.That(targets.Exists(item => item.name == "Sphere_1"), Is.False);
            Assert.That(targets.Exists(item => item.name == "Sphere_2"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_3"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_4"), Is.True);
        }
        
        [UnityTest]
        public IEnumerator DestroysObject()
        {
            var root = GetObjectAndComponentInRoot<Transform>("Test_8");
            root.gameObject.SetActive(true);
            var rangeTargetCheck = FindComponent<RangeTargetCheck>("Character_1");
            
            var targets = rangeTargetCheck.GetTargets(0);

            yield return null;
            
            var sphere = GameObject.Find("Sphere_1");
            Object.Destroy(sphere);

            yield return null;
            
            
            
            var result = rangeTargetCheck.ChangedValues(0, out var add, out var remove);
            Assert.That(result, Is.True, "The value did not change.");

            Assert.That(remove.Count, Is.EqualTo(1));
            
            Assert.That(targets.Count, Is.EqualTo(3));
            Assert.That(targets.Exists(item => item.name == "Sphere_1"), Is.False);
            Assert.That(targets.Exists(item => item.name == "Sphere_2"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_3"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_4"), Is.True);
        }
        
        [UnityTest]
        public IEnumerator RaisesEventWhenObjectDestroyed()
        {
            var root = GetObjectAndComponentInRoot<Transform>("Test_8");
            root.gameObject.SetActive(true);
            var rangeTargetCheck = FindComponent<RangeTargetCheck>("Character_1");
            var isCallEvent = false;
            
            rangeTargetCheck.GetSearchRangeSettings(0).OnChangeValue.AddListener(result =>
            {
                isCallEvent = true;
            });

            yield return null;
            
            var sphere = GameObject.Find("Sphere_1");
            Object.Destroy(sphere);

            yield return null;
            
            Assert.That(isCallEvent, Is.True);
        }
        
        [Test]
        public void GetTag()
        {
            var root = GetObjectAndComponentInRoot<Transform>("Test_9");
            root.gameObject.SetActive(true);
            var rangeTargetCheck = FindComponent<RangeTargetCheck>("Character_1");
            
            Assert.That(rangeTargetCheck.GetTagIndex("Player"), Is.EqualTo(0) );
            Assert.That(rangeTargetCheck.GetTagIndex("MainCamera"), Is.EqualTo(1) );
            Assert.That(rangeTargetCheck.GetTagIndex("Respawn"), Is.EqualTo(2) );
        }

        [UnityTest]
        public IEnumerator IgnoreTransparentWalls()
        {
            var root = GetObjectAndComponentInRoot<Transform>("Test_10");
            root.gameObject.SetActive(true);
            var rangeTargetCheck = FindComponent<RangeTargetCheck>("Character_1");

            yield return null;
            
            var targets = rangeTargetCheck.GetTargets(0);
            Assert.That(targets.Count, Is.EqualTo(3));
            Assert.That(targets.Exists(item => item.name == "Sphere_1"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_2"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_3"), Is.True);
            Assert.That(targets.Exists(item => item.name == "Sphere_4"), Is.False);
            Assert.That(targets.Exists(item => item.name == "Sphere_5"), Is.False);
        }

        protected override string ScenePath => TestConstants.PackagePath +  "Scenes/RangeCheckTest.unity";
    }
}
