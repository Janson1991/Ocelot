using Ocelot.DownstreamRouteFinder.UrlMatcher;
using Ocelot.Responses;
using Shouldly;
using TestStack.BDDfy;
using Xunit;

namespace Ocelot.UnitTests.DownstreamRouteFinder.UrlMatcher
{
    public class RegExUrlMatcherTests
    {
        private readonly IUrlPathToUrlTemplateMatcher _urlMatcher;
        private string _downstreamUrlPath;
        private string _downstreamPathTemplate;
        private Response<UrlMatch> _result;

        public RegExUrlMatcherTests()
        {
            _urlMatcher = new RegExUrlMatcher();
        }

        [Fact]
        public void should_find_match_when_template_smaller_than_valid_path()
        {
            this.Given(x => x.GivenIHaveAUpstreamPath("/api/products/2354325435624623464235"))
                .And(x => x.GivenIHaveAnUpstreamUrlTemplatePattern("/api/products/.*$"))
                .When(x => x.WhenIMatchThePaths())
                .And(x => x.ThenTheResultIsTrue())
                .BDDfy();
        }

        [Fact]
        public void should_not_find_match()
        {
            this.Given(x => x.GivenIHaveAUpstreamPath("/api/values"))
                .And(x => x.GivenIHaveAnUpstreamUrlTemplatePattern("/$"))
                .When(x => x.WhenIMatchThePaths())
                .And(x => x.ThenTheResultIsFalse())
                .BDDfy();
        }

        [Fact]
        public void can_match_down_stream_url()
        {
            this.Given(x => x.GivenIHaveAUpstreamPath(""))
                .And(x => x.GivenIHaveAnUpstreamUrlTemplatePattern("$"))
                .When(x => x.WhenIMatchThePaths())
                .And(x => x.ThenTheResultIsTrue())
                .BDDfy();
        }

        [Fact]
        public void can_match_down_stream_url_with_no_slash()
        {
            this.Given(x => x.GivenIHaveAUpstreamPath("api"))
                 .Given(x => x.GivenIHaveAnUpstreamUrlTemplatePattern("api$"))
                 .When(x => x.WhenIMatchThePaths())
                 .Then(x => x.ThenTheResultIsTrue())
                 .BDDfy();
        }

        [Fact]
        public void can_match_down_stream_url_with_one_slash()
        {
            this.Given(x => x.GivenIHaveAUpstreamPath("api/"))
                 .Given(x => x.GivenIHaveAnUpstreamUrlTemplatePattern("api/$"))
                 .When(x => x.WhenIMatchThePaths())
                 .Then(x => x.ThenTheResultIsTrue())
                 .BDDfy();
        }

        [Fact]
        public void can_match_down_stream_url_with_downstream_template()
        {
            this.Given(x => x.GivenIHaveAUpstreamPath("api/product/products/"))
              .Given(x => x.GivenIHaveAnUpstreamUrlTemplatePattern("api/product/products/$"))
              .When(x => x.WhenIMatchThePaths())
              .Then(x => x.ThenTheResultIsTrue())
              .BDDfy();
        }

        [Fact]
        public void can_match_down_stream_url_with_downstream_template_with_one_place_holder()
        {
            this.Given(x => x.GivenIHaveAUpstreamPath("api/product/products/1"))
               .Given(x => x.GivenIHaveAnUpstreamUrlTemplatePattern("api/product/products/.*$"))
               .When(x => x.WhenIMatchThePaths())
               .Then(x => x.ThenTheResultIsTrue())
               .BDDfy();
        }

        [Fact]
        public void can_match_down_stream_url_with_downstream_template_with_two_place_holders()
        {
            this.Given(x => x.GivenIHaveAUpstreamPath("api/product/products/1/2"))
                 .Given(x => x.GivenIHaveAnUpstreamUrlTemplatePattern("api/product/products/.*/.*$"))
                 .When(x => x.WhenIMatchThePaths())
                 .Then(x => x.ThenTheResultIsTrue())
                 .BDDfy();
        }

        [Fact]
        public void can_match_down_stream_url_with_downstream_template_with_two_place_holders_seperated_by_something()
        {
            this.Given(x => x.GivenIHaveAUpstreamPath("api/product/products/1/categories/2"))
                .And(x => x.GivenIHaveAnUpstreamUrlTemplatePattern("api/product/products/.*/categories/.*$"))
                .When(x => x.WhenIMatchThePaths())
                .Then(x => x.ThenTheResultIsTrue())
                .BDDfy();
        }

        [Fact]
        public void can_match_down_stream_url_with_downstream_template_with_three_place_holders_seperated_by_something()
        {
            this.Given(x => x.GivenIHaveAUpstreamPath("api/product/products/1/categories/2/variant/123"))
                .And(x => x.GivenIHaveAnUpstreamUrlTemplatePattern("api/product/products/.*/categories/.*/variant/.*$"))
                .When(x => x.WhenIMatchThePaths())
                .Then(x => x.ThenTheResultIsTrue())
                .BDDfy();
        }

        [Fact]
        public void can_match_down_stream_url_with_downstream_template_with_three_place_holders()
        {
            this.Given(x => x.GivenIHaveAUpstreamPath("api/product/products/1/categories/2/variant/"))
                 .And(x => x.GivenIHaveAnUpstreamUrlTemplatePattern("api/product/products/.*/categories/.*/variant/$"))
                 .When(x => x.WhenIMatchThePaths())
                 .Then(x => x.ThenTheResultIsTrue())
                 .BDDfy();
        }

        [Fact]
        public void should_ignore_case_sensitivity()
        {
            this.Given(x => x.GivenIHaveAUpstreamPath("API/product/products/1/categories/2/variant/"))
               .And(x => x.GivenIHaveAnUpstreamUrlTemplatePattern("(?i)api/product/products/.*/categories/.*/variant/$"))
               .When(x => x.WhenIMatchThePaths())
               .Then(x => x.ThenTheResultIsTrue())
               .BDDfy();
        }

        [Fact]
        public void should_respect_case_sensitivity()
        {
            this.Given(x => x.GivenIHaveAUpstreamPath("API/product/products/1/categories/2/variant/"))
              .And(x => x.GivenIHaveAnUpstreamUrlTemplatePattern("api/product/products/.*/categories/.*/variant/$"))
              .When(x => x.WhenIMatchThePaths())
              .Then(x => x.ThenTheResultIsFalse())
              .BDDfy();
        }

        private void GivenIHaveAUpstreamPath(string downstreamPath)
        {
            _downstreamUrlPath = downstreamPath;
        }

        private void GivenIHaveAnUpstreamUrlTemplatePattern(string downstreamUrlTemplate)
        {
            _downstreamPathTemplate = downstreamUrlTemplate;
        }

        private void WhenIMatchThePaths()
        {
            _result = _urlMatcher.Match(_downstreamUrlPath, _downstreamPathTemplate);
        }

        private void ThenTheResultIsTrue()
        {
            _result.Data.Match.ShouldBeTrue();
        }

        private void ThenTheResultIsFalse()
        {
            _result.Data.Match.ShouldBeFalse();
        }
    }
} 