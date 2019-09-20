import { ResourceManagementClient } from '@azure/arm-resources';
import { debug, Debugger } from 'debug';
import { ResourceGroup } from '@azure/arm-resources/esm/models';

import { setupAzureCredential, azureSubscriptionId } from './utils';

export class ResourceManagement {
  private resourceManagementClient: ResourceManagementClient;
  private log: Debugger;

  public constructor() {
    this.log = debug('crm-resource-management');
  }

  public async Run(resouceGroupName: string, setting: ResourceGroup) {
    let cred = await setupAzureCredential();
    this.resourceManagementClient = new ResourceManagementClient(cred as any, azureSubscriptionId);

    //create resource group 
    let existed = await this.resourceManagementClient.resourceGroups.checkExistence(resouceGroupName);
    
    if(!existed.body) {
        this.log(`Begin to create resouce group ${resouceGroupName}`);
        await this.resourceManagementClient.resourceGroups.createOrUpdate(resouceGroupName, setting);
    }
    else {
        this.log(`Resouce group ${resouceGroupName} existed !!!`)
    }
  }
}
