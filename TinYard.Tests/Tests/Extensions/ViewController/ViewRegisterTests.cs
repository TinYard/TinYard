using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TinYard.API.Interfaces;
using TinYard.Extensions.ViewController.API.Interfaces;
using TinYard.Extensions.ViewController.Impl.Base;
using TinYard.Extensions.ViewController.Tests.MockClasses;

namespace TinYard.Extensions.ViewController.Tests
{
    [TestClass]
    public class ViewRegisterTests
    {
        private IContext _context;
        private IViewRegister _register;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
            _register = new ViewRegister(_context);
        }

        [TestCleanup]
        public void Teardown()
        {
            _register = null;
        }

        [TestMethod]
        public void ViewRegister_is_IViewRegister()
        {
            Assert.IsInstanceOfType(_register, typeof(IViewRegister));
        }

        [TestMethod]
        public void ViewRegister_Instance_Is_Created()
        {
            Assert.IsNotNull(ViewRegister.Instance);
        }

        [TestMethod]
        public void IView_Is_Registered()
        {
            TestView expected = new TestView();
            ViewRegister.Register(expected);

            IReadOnlyList<IView> registeredViews = ViewRegister.Instance.RegisteredViews;
            
            Assert.IsTrue(registeredViews.Contains(expected));
        }

        [TestMethod]
        public void IView_Cant_Register_Twice()
        {
            TestView testView = new TestView();

            bool actual2 = ViewRegister.Register(testView);

            Assert.IsTrue(ViewRegister.Instance.RegisteredViews.Contains(testView));
            Assert.IsFalse(actual2);
        }
    }
}