using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confy.File;
using Confy.File.Exceptions;
using Confy.File.FluentBuilder;
using Confy.File.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Confy.Test
{
    [TestClass]
    public class When_InitializeNewWrapper_WithThrowingOnReload
    {
        private string _path = string.Empty;

        [TestInitialize]
        public void Init()
        {
            _path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            TestHelpers.SetConfigAsDef();
        }
        
        [TestMethod]
        [TestCategory("Initializers"), TestCategory("Complex"), TestCategory("File"), TestCategory("LastUpdateRefreshMode"), TestCategory("ThrowingOnReload")]
        public void If_CreateComplexObjectWrapperWithJustOneSectionsRefreshUsingLastUpdate_AndChangeOccurs_Then_ReturnCorrectComplexObjectWrapper_CanReload()
        {
            TestHelpers.SetConfigAsDef();
            // ARRANGE & ACT
            var container =
                FileContainerBuilder.BuildContainer<ComplexSampleObject>()
                    .LocatedAt(_path + @"\Config_ComplexSectionConfig.json")
                    .UsingSectionInFile("NO-SAMPLE")
                    .RefreshingWhenFileChange()
                    .ThrowsIfUnableToRefresh(true)
                    .Build();

            var firstSnapshot = Helpers.DeepClone(container);
            TestHelpers.ModifyConfig();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            var secondSnapshot = Helpers.DeepClone(container);

            //ASSERT
            Assert.IsNotNull(firstSnapshot.Configuration);
            Assert.IsNotNull(firstSnapshot.Configuration.SampleSimpleObject);
            Assert.AreEqual("Complex Name", firstSnapshot.Configuration.ComplexFirstLevelName);
            Assert.AreEqual(new DateTime(2016, 10, 4, 5, 20, 0), firstSnapshot.Configuration.TimeStamp);
            Assert.AreEqual("Second Level Name", firstSnapshot.Configuration.SampleSimpleObject.Name);
            Assert.AreEqual(50, firstSnapshot.Configuration.SampleSimpleObject.Age);

            Assert.IsNotNull(secondSnapshot.Configuration);
            Assert.IsNotNull(secondSnapshot.Configuration.SampleSimpleObject);
            Assert.AreEqual("New Complex Name", secondSnapshot.Configuration.ComplexFirstLevelName);
            Assert.AreEqual(new DateTime(2017, 10, 4, 5, 20, 0), secondSnapshot.Configuration.TimeStamp);
            Assert.AreEqual("New Second Level Name", secondSnapshot.Configuration.SampleSimpleObject.Name);
            Assert.AreEqual(30, secondSnapshot.Configuration.SampleSimpleObject.Age);
        }

        [TestMethod]
        [TestCategory("Initializers"), TestCategory("Complex"), TestCategory("File"), TestCategory("LastUpdateRefreshMode")]
        [ExpectedException(typeof(InconsistantContainerException))]
        public void If_CreateComplexObjectWrapperWithJustOneSectionsRefreshUsingLastUpdateAndCamaleonicField_AndChangeOccurs_CannotReload_Then_ReturnCorrectComplexObjectWrapper()
        {
            TestHelpers.SetConfigAsDef();
            // ARRANGE & ACT
            var container =
                FileContainerBuilder.BuildContainer<ComplexSampleObject>()
                .LocatedAt(_path + @"\Config_ComplexSectionConfig.json")
                .UsingEntireFile()
                .RefreshingWhenFileChange()
                .ThrowsIfUnableToRefresh(true)
                .Build();

            TestHelpers.ModifyConfigBlockingMode();
            var test = container.Configuration.SampleSimpleObject;


        }
    }
}



