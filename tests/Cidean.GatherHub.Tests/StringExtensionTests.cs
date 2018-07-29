using Cidean.GatherHub.Core.Helpers;
using System;
using Xunit;

namespace Cidean.GatherHub.Tests
{
    public class StringExtensionTests
    {
        [Fact]
        public void StripHtml_ContainsHtml_ContainsNoTags()
        {
            var noHtml = "<strong>testing</strong>".StripHtml();

            Assert.Equal("testing", noHtml);
            
        }
       
    }
}
