using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confy.File;
using Confy.File.FluentBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Confy.Test
{
    [TestClass]
    public class When_InitializeNewWrapper
    {
        private string _path = string.Empty;

        [TestInitialize]
        public void Init()
        {
            _path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            TestHelpers.SetConfigAsDef();
        }

        [TestMethod]
        [TestCategory("Initializers"), TestCategory("SimpleObjects"), TestCategory("File")]
        public void If_CreateSimpleObjectWrapperWithAllSectionsNoRefresh_Then_ReturnCorrectSimpleObjectWrapper()
        {
            // ARRANGE & ACT
            var container =
                FileContainerBuilder.BuildContainer<SampleSimpleObject>().LocatedAt(_path + @"\Config_SimpleNoSectionConfig.json").GetAll().NoRefresh().Build();

            //ASSERT
            Assert.IsNotNull(container.Configuration);
            Assert.AreEqual("Sample Name", container.Configuration.Name);
            Assert.AreEqual(10, container.Configuration.Age);

        }

        [TestMethod]
        [TestCategory("Initializers"), TestCategory("SimpleObjects"), TestCategory("File")]
        public void If_CreateSimpleObjectWrapperWithJustOneSectionsNoRefresh_Then_ReturnCorrectSimpleObjectWrapper()
        {
            // ARRANGE & ACT
            var container =
                FileContainerBuilder.BuildContainer<SampleSimpleObject>().LocatedAt(_path + @"\Config_SimpleSectionConfig.json").UsingSection("NO-SAMPLE").NoRefresh().Build();

            //ASSERT
            Assert.IsNotNull(container.Configuration);
            Assert.AreEqual("No Sample Name", container.Configuration.Name);
            Assert.AreEqual(15, container.Configuration.Age);

        }

        [TestMethod]
        [TestCategory("Initializers"), TestCategory("Complex"), TestCategory("File")]
        public void If_CreateComplexObjectWrapperWithJustOneSectionsNoRefresh_Then_ReturnCorrectComplexObjectWrapper()
        {
            // ARRANGE & ACT
            var container =
                FileContainerBuilder.BuildContainer<ComplexSampleObject>().LocatedAt(_path + @"\Config_ComplexSectionConfig.json").UsingSection("NO-SAMPLE").NoRefresh().Build();

            //ASSERT
            Assert.IsNotNull(container.Configuration);
            Assert.IsNotNull(container.Configuration.SampleSimpleObject);
            Assert.AreEqual("Complex Name", container.Configuration.ComplexFirstLevelName);
            Assert.AreEqual(new DateTime(2016, 10, 4, 5, 20, 0), container.Configuration.TimeStamp);
            Assert.AreEqual("Second Level Name", container.Configuration.SampleSimpleObject.Name);
            Assert.AreEqual(50, container.Configuration.SampleSimpleObject.Age);
        }


        [TestMethod]
        [TestCategory("Initializers"), TestCategory("Complex"), TestCategory("File"), TestCategory("ForcedAutoRefreshEachMode")]
        public void If_CreateComplexObjectWrapperWithJustOneSectionsForcedRefresh_Then_ReturnCorrectComplexObjectWrapper()
        {
            // ARRANGE & ACT
            var container =
                FileContainerBuilder.BuildContainer<ComplexSampleObject>()
                    .LocatedAt(_path + @"\Config_ComplexSectionConfig.json")
                    .UsingSection("NO-SAMPLE")
                    .UsingRefreshMode()
                    .Automatic()
                    .Each(TimeSpan.FromSeconds(2))
                    .Build();

            var firstSnapshot = TestHelpers.DeepClone(container);
            TestHelpers.ModifyConfig();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            var secondSnapshot = TestHelpers.DeepClone(container);

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
        public void If_CreateComplexObjectWrapperWithJustOneSectionsRefreshUsingLastUpdate_AndChangeOccurs_Then_ReturnCorrectComplexObjectWrapper()
        {
            // ARRANGE & ACT
            var container =
                FileContainerBuilder.BuildContainer<ComplexSampleObject>()
                    .LocatedAt(_path + @"\Config_ComplexSectionConfig.json")
                    .UsingSection("NO-SAMPLE")
                    .UsingRefreshMode()
                    .LookingAtFileEachMode(TimeSpan.FromSeconds(2))
                    .Build();

            var firstSnapshot = TestHelpers.DeepClone(container);
            TestHelpers.ModifyConfig();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            var secondSnapshot = TestHelpers.DeepClone(container);

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
        public void If_CreateComplexObjectWrapperWithJustOneSectionsRefreshUsingLastUpdate_AndNoChangeOccurs_Then_ReturnCorrectComplexObjectWrapper()
        {
            // ARRANGE & ACT
            var container =
                FileContainerBuilder.BuildContainer<ComplexSampleObject>()
                    .LocatedAt(_path + @"\Config_ComplexSectionConfig.json")
                    .UsingSection("NO-SAMPLE")
                    .UsingRefreshMode()
                    .LookingAtFileEachMode(TimeSpan.FromSeconds(10))
                    .Build();

            var firstSnapshot = TestHelpers.DeepClone(container);
            TestHelpers.ModifyConfig();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            var secondSnapshot = TestHelpers.DeepClone(container);

            //ASSERT
            Assert.IsNotNull(firstSnapshot.Configuration);
            Assert.IsNotNull(firstSnapshot.Configuration.SampleSimpleObject);
            Assert.AreEqual("Complex Name", firstSnapshot.Configuration.ComplexFirstLevelName);
            Assert.AreEqual(new DateTime(2016, 10, 4, 5, 20, 0), firstSnapshot.Configuration.TimeStamp);
            Assert.AreEqual("Second Level Name", firstSnapshot.Configuration.SampleSimpleObject.Name);
            Assert.AreEqual(50, firstSnapshot.Configuration.SampleSimpleObject.Age);

            Assert.IsNotNull(secondSnapshot.Configuration);
            Assert.IsNotNull(secondSnapshot.Configuration.SampleSimpleObject);
            Assert.AreEqual("Complex Name", secondSnapshot.Configuration.ComplexFirstLevelName);
            Assert.AreEqual(new DateTime(2016, 10, 4, 5, 20, 0), secondSnapshot.Configuration.TimeStamp);
            Assert.AreEqual("Second Level Name", secondSnapshot.Configuration.SampleSimpleObject.Name);
            Assert.AreEqual(50, secondSnapshot.Configuration.SampleSimpleObject.Age);
        }
    }
}



