// CartControllerTest.cs
using Services;
using Moq;
using api.Controllers;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using api.test.Attributes;
using AutoFixture.Xunit2;
using FluentAssertions;

namespace test
{
    public class Tests
    {
        /*private CartController controller;
        private Mock<IPaymentService> paymentServiceMock;
        private Mock<ICartService> cartServiceMock;

        private Mock<IShipmentService> shipmentServiceMock;
        private Mock<ICard> cardMock;
        private Mock<IAddressInfo> addressInfoMock;
        private List<CartItem> items;*/

        /*public void Setup()
        { 
            cartServiceMock = new Mock<ICartService>();
            paymentServiceMock = new Mock<IPaymentService>();
            shipmentServiceMock = new Mock<IShipmentService>();

            cardMock = new Mock<ICard>();
            addressInfoMock = new Mock<IAddressInfo>();

            var cartItemMock = new Mock<CartItem>();
            cartItemMock.Setup(item => item.Price).Returns(10);

            items = new List<CartItem>()
            {
              cartItemMock.Object
            };

            cartServiceMock.Setup(c => c.Items()).Returns(items.AsEnumerable());

            controller = new CartController(cartServiceMock.Object, paymentServiceMock.Object, shipmentServiceMock.Object);
        }*/

        [Theory]
        [AutoMoqData]
        public void ShouldReturnCharged(
              [Frozen]Mock<ICartService> cartServiceMock
            , [Frozen]Mock<IPaymentService> paymentServiceMock
            , [Frozen]Mock<IShipmentService> shipmentServiceMock
            , Mock<ICard> cardMock
            , Mock<IAddressInfo> addressInfoMock
            , [Frozen] Mock<CartItem> cartItemMock
            , List<CartItem> items
            , CartController controller
          )
        {
            //arrange
            cartItemMock.Setup(item => item.Price).Returns(10);
            cartServiceMock.Setup(c => c.Items()).Returns(items.AsEnumerable());
            paymentServiceMock.Setup(p => p.Charge(It.IsAny<double>(), cardMock.Object)).Returns(true);

            // act
            var result = controller.CheckOut(cardMock.Object, addressInfoMock.Object);

            // assert
            shipmentServiceMock.Verify(s => s.Ship(addressInfoMock.Object, items.AsEnumerable()), Times.Once());
            result.Should().Be("charged");
        }

        [Theory]
        [AutoMoqData]
        public void ShouldReturnNotCharged(
              [Frozen] Mock<ICartService> cartServiceMock
            , [Frozen] Mock<IPaymentService> paymentServiceMock
            , [Frozen] Mock<IShipmentService> shipmentServiceMock
            , Mock<ICard> cardMock
            , Mock<IAddressInfo> addressInfoMock
            , [Frozen] Mock<CartItem> cartItemMock
            , List<CartItem> items
            , CartController controller
            ) 
        {
            //arrange
            cartItemMock.Setup(item => item.Price).Returns(10);
            cartServiceMock.Setup(c => c.Items()).Returns(items.AsEnumerable());
            paymentServiceMock.Setup(p => p.Charge(It.IsAny<double>(), cardMock.Object)).Returns(false);

            // act
            var result = controller.CheckOut(cardMock.Object, addressInfoMock.Object);

            // assert
            shipmentServiceMock.Verify(s => s.Ship(addressInfoMock.Object, items.AsEnumerable()), Times.Never());
            result.Should().Be("not charged");
        }
    }
}