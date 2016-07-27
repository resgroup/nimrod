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
        public void GetMethod()
        {
            // test the GET attribute
        }

        [HttpPost]
        public void PostMethod()
        {
            // test the POST attribute
        }

        [HttpPut]
        public void PutMethod()
        {
            // test the PUT attribute
        }

        [HttpDelete]
        public void DeleteMethod()
        {
            // test the DELETE attribute
        }
        [HttpOptions]
        public void OptionsMethod()
        {
            // test the OPTIONS attribute
        }
        [HttpHead]
        public void HeadMethod()
        {
            // test the HEAD attribute
        }
        [HttpPatch]
        public void PatchMethod()
        {
            // test the PATCH attribute
        }

        public void NoHttpMethod()
        {
            // test missing http method attribute
        }
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
        public void FirstOrDefaultHttpMethodAttribute_Head_ReturnsNull()
        {
            Assert.IsNull(typeof(ExtensionsTest).GetMethod(nameof(HeadMethod)).FirstOrDefaultHttpMethodAttribute());
        }
        [Test]
        public void FirstOrDefaultHttpMethodAttribute_Options_ReturnsNull()
        {
            Assert.IsNull(typeof(ExtensionsTest).GetMethod(nameof(OptionsMethod)).FirstOrDefaultHttpMethodAttribute());
        }
        [Test]
        public void FirstOrDefaultHttpMethodAttribute_Patch_ReturnsNull()
        {
            Assert.IsNull(typeof(ExtensionsTest).GetMethod(nameof(PatchMethod)).FirstOrDefaultHttpMethodAttribute());
        }

    }
}
