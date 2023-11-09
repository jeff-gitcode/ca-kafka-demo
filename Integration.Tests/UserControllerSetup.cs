using AutoFixture;
using AutoFixture.Xunit2;

namespace Integration.Tests
{

    public class UserControllerSetup : AutoDataAttribute
    {
        public UserControllerSetup() : base(() => new Fixture()
            .Customize(new TestContainersSetup())
            .Customize(new KafkaConsumerSetup())
            .Customize(new TestServerSetup()))
        {
        }
    }
}