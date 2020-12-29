using System;
using NUnit.Framework;

namespace Tests
{
   public abstract class BaseTestClass<TSut> where TSut : IDisposable
    {
        public TSut Sut { get; set; }

        [SetUp]
        public abstract void Setup();

        [Test]
        public void CanCreate() => Assert.IsNotNull(Sut);

        [TearDown]
        protected void TearDown() => Sut.Dispose();
    }
}
