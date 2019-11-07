using CRM.Protobuf.Contacts.V1;
using HotChocolate.Types;

namespace CRM.Graph.Gateway.Types.Contacts
{
    public class UploadPhotoInputType : InputObjectType<UploadPhotoRequest>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UploadPhotoRequest> descriptor)
        {
            descriptor.Name("UploadPhotoInput");
        }
    }
}
