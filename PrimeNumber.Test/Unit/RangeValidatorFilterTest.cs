using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using PrimeNumber.Api.Filter;
using PrimeNumber.Extensibility;
using System;
using System.Collections.Generic;
using Range = PrimeNumber.Extensibility.Range;

namespace PrimeNumber.Test.Unit
{
    public class RangeValidatorFilterTest
    {
        private Mock<HttpContext> mockHttpContext;
        private Mock<IServiceProvider> mockServiceProvider;
        private Mock<IOptions<Configuration>> mockOptions;

        [SetUp]
        public void Setup()
        {
            mockHttpContext = new Mock<HttpContext>();
            mockServiceProvider = new Mock<IServiceProvider>();
            mockOptions = new Mock<IOptions<Configuration>>();
            mockHttpContext.SetupGet(mock => mock.RequestServices).Returns(mockServiceProvider.Object);
            mockServiceProvider.Setup(mock => mock.GetService(typeof(IOptions<Configuration>))).Returns(mockOptions.Object);
        }

        [TestCase(2, 2, int.MaxValue, TestName = "OnActionExecuting - number is 2 and in the range, context result should be null")]
        [TestCase(115, 2, int.MaxValue, TestName = "OnActionExecuting - number is 115 and in the range, context result should be null")]
        [TestCase(456789, -2, 456790, TestName = "OnActionExecuting - number is 115 and in the range, context result should be null")]
        [TestCase(int.MaxValue, 2, int.MaxValue, TestName = "OnActionExecuting - number is MaxValue and in the range, context result should be null")]
        public void OnActionExecuting_number_in_range_context_result_should_be_null(int value, int min, int max)
        {
            var context = SetContext(value, min, max);
            var filter = new RangeValidationFilter();
            filter.OnActionExecuting(context);
            Assert.IsNull(context.Result);
        }

        [TestCase(1, 2, 115, TestName = "OnActionExecuting - number is 1 and below the range, context result should be BadRequestResult")]
        [TestCase(0, 2, 110, TestName = "OnActionExecuting - number is 0 and below the range, context result should be BadRequestResult")]
        [TestCase(-1, 0, 50, TestName = "OnActionExecuting - number is -1 and below the range, context result should be BadRequestResult")]
        [TestCase(-256789, -20, -10, TestName = "OnActionExecuting - number is -256789 and below the range, context result should be BadRequestResult")]
        [TestCase(51, 0, 50, TestName = "OnActionExecuting - number is 51 and above the range, context result should be BadRequestResult")]
        [TestCase(-8, -10, -9, TestName = "OnActionExecuting - number is -8 and above the range, context result should be BadRequestResult")]
        [TestCase(0, -10, -1, TestName = "OnActionExecuting - number is 0 and above the range, context result should be BadRequestResult")]
        [TestCase(250, 0, 25, TestName = "OnActionExecuting - number is 250 and above the range, context result should be BadRequestResult")]
        public void OnActionExecuting_number_below_range_context_result_should_be_badrequest(int value, int min, int max)
        {
            var context = SetContext(value, min, max);
            var filter = new RangeValidationFilter();
            filter.OnActionExecuting(context);
            Assert.IsTrue(context.Result is BadRequestObjectResult);
            Assert.AreEqual((context.Result as BadRequestObjectResult).Value, $@"input out of range, it must be between {min} and {max}");
        }

        [TestCase(TestName = "OnActionExecuting - number is null, context result should be BadRequestResult")]
        public void OnActionExecuting_number_is_null_context_result_should_be_badrequest()
        {
            var context = SetContext(null, -10, 250);
            var filter = new RangeValidationFilter();
            filter.OnActionExecuting(context);
            Assert.IsTrue(context.Result is BadRequestObjectResult);
            Assert.AreEqual((context.Result as BadRequestObjectResult).Value, "Missing input");
        }

        private ActionExecutingContext SetContext(int? value, int min, int max)
        {
            IDictionary<string, object> actionArguments = new Dictionary<string, object>
            {
                ["value"] = value
            };
            mockOptions.SetupGet(mock => mock.Value).Returns(new Configuration
            {
                Range = new Range { Max = max, Min = min }
            });

            return new ActionExecutingContext(
                new ActionContext(
                    httpContext: mockHttpContext.Object,
                    routeData: new RouteData(),
                    actionDescriptor: new ActionDescriptor(),
                    modelState: new ModelStateDictionary()
                ),
                new List<IFilterMetadata>(),
                actionArguments,
                new Mock<ControllerBase>().Object);
        }
    }
}
