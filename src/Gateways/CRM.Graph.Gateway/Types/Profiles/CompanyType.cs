// using HotChocolate.Types;
// using CRM.Protobuf.Profile.V1;

// namespace CRM.Graph.Gateway.Types.Profiles
// {
//     public class CompanyType : ObjectType<Company>
//     {
//         protected override void Configure(IObjectTypeDescriptor<Company> descriptor)
//         {
//             descriptor.Field(t => t.CalculateSize()).Ignore();
//             descriptor.Field(t => t.Clone()).Ignore();
//             descriptor.Field(t => t.Equals(null)).Ignore();
//         }
//     }
// }
