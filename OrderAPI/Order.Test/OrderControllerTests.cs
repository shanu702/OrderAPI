using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Order.Entities;
using OrderAPI.Controllers;
using OrderAPI.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OrderApi.Tests
{
    public class OrderControllerTests
    {
        private readonly Mock<OrderQueueService> _orderQueueServiceMock;
        private readonly Mock<OrderProcessingService> _orderProcessingServiceMock;
        private readonly Mock<ILogger<OrderController>> _loggerMock;
        private readonly Mock<IValidator<OrderRequest>> _validatorMock;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _orderQueueServiceMock = new Mock<OrderQueueService>();
            _orderProcessingServiceMock = new Mock<OrderProcessingService>();
            _loggerMock = new Mock<ILogger<OrderController>>();
            _validatorMock = new Mock<IValidator<OrderRequest>>();
            _controller = new OrderController(_orderQueueServiceMock.Object, _orderProcessingServiceMock.Object, _loggerMock.Object, _validatorMock.Object);
        }

        [Fact]
        public void PlaceOrder_ReturnsCreatedResult()
        {
            // Arrange
            var orderRequest = new OrderRequest { OrderId = 1, ProductName = "Product A", Quantity = 10 };
            _validatorMock.Setup(v => v.Validate(orderRequest)).Returns(new FluentValidation.Results.ValidationResult());

            // Act
            var result = _controller.PlaceOrder(orderRequest);

            // Assert
            var createdAtActionResult = Assert.IsAssignableFrom<CreatedAtActionResult>(result);
            Assert.Equal("GetOrder", createdAtActionResult.ActionName);
        }

        [Fact]
        public void GetOrders_ReturnsOkResult()
        {
            // Arrange
            List<Order.Entities.Order> orders = new List<Order.Entities.Order>
            {
                new Order.Entities.Order { OrderId = 1, ProductName = "Product A", Quantity = 10 },
                new Order.Entities.Order { OrderId = 2, ProductName = "Product B", Quantity = 20 }
            };
            _orderProcessingServiceMock.Setup(s => s.GetAllOrders(It.IsAny<int>(), It.IsAny<int>())).Returns(orders);

            // Act
            var result = _controller.GetOrders(1, 10);

            // Assert
            var okResult = Assert.IsAssignableFrom<OkObjectResult>(result);
            var returnedOrders = Assert.IsAssignableFrom<IEnumerable<Order.Entities.Order>>(okResult.Value);
            Assert.Equal(2, returnedOrders.Count());
        }

        [Fact]
        public void GetOrder_ReturnsOkResult_WhenOrderExists()
        {
            // Arrange
            var order = new Order.Entities.Order { OrderId = 1, ProductName = "Product A", Quantity = 10 };
            _orderProcessingServiceMock.Setup(s => s.GetOrderById(1)).Returns(order);

            // Act
            var result = _controller.GetOrder(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedOrder = Assert.IsType<Order.Entities.Order>(okResult.Value);
            Assert.Equal(order.OrderId, returnedOrder.OrderId);
        }

        [Fact]
        public void GetOrder_ReturnsNotFoundResult_WhenOrderDoesNotExist()
        {
            // Arrange
            _orderProcessingServiceMock.Setup(s => s.GetOrderById(1)).Returns((Order.Entities.Order)null);

            // Act
            var result = _controller.GetOrder(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}