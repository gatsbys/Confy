using System;
using System.IO;
using Confy.File.FluentBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Confy.Test
{
    [TestClass]
    public class WhenContainerBuilds
    {
        private string FilePath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
        [TestMethod]
        public void IfContianerBuildsWithAutomaticAndSectionThenOk()
        {
            var container =
                FileContainerBuilder.BuildContainer<SampleSimpleObject>()
                    .LocatedAt(FilePath)
                    .UsingSection("SAMPLE")
                    .Using()
                    .
                    .LookingAtFileEach(TimeSpan.FromSeconds(5))
                    .Build();
        }
    }
}
