using AutoMapper;
using NSubstitute;
using OrderServiceDemo.Controllers;
using OrderServiceDemo.Exceptions;
using OrderServiceDemo.Models.Exceptions;
using OrderServiceDemo.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace OrderServiceDemo.Unit.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly IOrderService _orderSerivce;
        private readonly IMapper _mapper;

        public OrderControllerTests()
        {
            var autoMapperConfig = Mapping.AutoMapperConfig.Configure();
            _mapper = autoMapperConfig.CreateMapper();

            _orderSerivce = Substitute.For<IOrderService>();
        }

        [Fact]
        public async Task OrderController_WhenCreatingOrder_IfInvalidRequest_ShouldReturn_HttpBadReqest()
        {
            //Arrange
            _orderSerivce
                .CreateOrder(Arg.Any<Models.Order>())
                .Returns(Task.FromException<Models.Order>(new InvalidRequestException("The Message")));

            var controller = BuildController();

            //Act
            var result = await Assert.ThrowsAsync<StatusCodeException>(() => controller.CreateOrder(new Core.v1.RequestModels.CreateOrderRequest
            {
                OrderLineItems = new List<Core.v1.OrderLineItem>()
            }));

            //Assert
            Assert.Equal((int)System.Net.HttpStatusCode.BadRequest, result.StatusCode);
        }

        private OrderController BuildController() => new OrderController(
            _mapper, 
            _orderSerivce);
    }
}
