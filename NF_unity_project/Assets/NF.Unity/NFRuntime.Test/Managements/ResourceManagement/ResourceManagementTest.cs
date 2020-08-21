using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace NFRuntime.Managements.ResourceManagement.Test
{
    public class ResourceManagementTest
    {
        [Test]
        public async void NewTestScriptSimplePasses()
        {
            ResourcesResourceManager rrm = new ResourcesResourceManager();
            var ta = await rrm.LoadAsync<TextAsset>("test");
            Assert.AreEqual("asdf", ta.text);
        }
    }
}
