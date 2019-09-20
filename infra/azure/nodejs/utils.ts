import {
  ApplicationTokenCredentials,
  loginWithServicePrincipalSecret
} from '@azure/ms-rest-nodeauth';

export const azureSubscriptionId = process.env['AZURE_SUBSCRIPTION_ID'] || '';

export async function setupAzureCredential(): Promise<ApplicationTokenCredentials> {
  const clientId = process.env['AZURE_CLIENT_ID'] || '';
  const secret = process.env['AZURE_SECRET'] || '';
  const tenantId = process.env['AZURE_TENANT_ID'] || '';

  var cred = await loginWithServicePrincipalSecret(clientId, secret, tenantId);

  return cred;
}
