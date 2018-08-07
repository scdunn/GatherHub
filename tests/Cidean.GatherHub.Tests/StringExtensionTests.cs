using Cidean.GatherHub.Core.Helpers;
using System;
using System.Collections.Generic;
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


        [Fact]
        public void FormatWithTags_TagInText_ReturnsNameInText()
        {
            var text = "Hello, {firstname}";
            var tags = new { firstname = "Chris", address = new { street="Main Street", city="Bloomington"}};

            Assert.Equal("Hello, Chris", text.FormatWithTags(tags));

        }

    }
}
