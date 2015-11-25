using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Nimrod.Test
{
    [TestFixture]
    public class ExtensionsTest
    {
        [HttpGet]
        public void GetMethod() { }

        [HttpPost]
        public void PostMethod() { }

        [HttpPut]
        public void PutMethod() { }

        [HttpDelete]
        public void DeleteMethod() { }
        [HttpOptions]
        public void OptionsMethod() { }
        [HttpHead]
        public void HeadMethod() { }
        [HttpPatch]
        public void PatchMethod() { }

        public void NoHttpMethod() { }
        [Test]
        public void FirstOrDefaultHttpMethodAttribute_NoHttp()
        {
            Assert.AreEqual(null, typeof(ExtensionsTest).GetMethod(nameof(NoHttpMethod)).FirstOrDefaultHttpMethodAttribute());
        }
        [Test]
        public void FirstOrDefaultHttpMethodAttribute_Get()
        {
            Assert.AreEqual(HttpMethodAttribute.Get, typeof(ExtensionsTest).GetMethod(nameof(GetMethod)).FirstOrDefaultHttpMethodAttribute());

        }
        [Test]
        public void FirstOrDefaultHttpMethodAttribute_Post()
        {
            Assert.AreEqual(HttpMethodAttribute.Post, typeof(ExtensionsTest).GetMethod(nameof(PostMethod)).FirstOrDefaultHttpMethodAttribute());

        }
        [Test]
        public void FirstOrDefaultHttpMethodAttribute_Put()
        {
            Assert.AreEqual(HttpMethodAttribute.Put, typeof(ExtensionsTest).GetMethod(nameof(PutMethod)).FirstOrDefaultHttpMethodAttribute());

        }
        [Test]
        public void FirstOrDefaultHttpMethodAttribute_Delete()
        {
            Assert.AreEqual(HttpMethodAttribute.Delete, typeof(ExtensionsTest).GetMethod(nameof(DeleteMethod)).FirstOrDefaultHttpMethodAttribute());
        }
        [Test]
        public void FirstOrDefaultHttpMethodAttribute_Head()
        {
            Assert.AreEqual(HttpMethodAttribute.Head, typeof(ExtensionsTest).GetMethod(nameof(HeadMethod)).FirstOrDefaultHttpMethodAttribute());
        }
        [Test]
        public void FirstOrDefaultHttpMethodAttribute_Options()
        {
            Assert.AreEqual(HttpMethodAttribute.Options, typeof(ExtensionsTest).GetMethod(nameof(OptionsMethod)).FirstOrDefaultHttpMethodAttribute());
        }
        [Test]
        public void FirstOrDefaultHttpMethodAttribute_Patch()
        {
            Assert.AreEqual(HttpMethodAttribute.Patch, typeof(ExtensionsTest).GetMethod(nameof(PatchMethod)).FirstOrDefaultHttpMethodAttribute());
        }

    }
}
