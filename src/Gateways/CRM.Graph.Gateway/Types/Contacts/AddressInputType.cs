using CRM.Protobuf.Contacts.V1;
using HotChocolate.Types;

namespace CRM.Graph.Gateway.Types
{
    public class AddressInputType : InputObjectType<Address>
    {
        protected override void Configure(IInputObjectTypeDescriptor<Address> descriptor)
        {
        }
    }
}
