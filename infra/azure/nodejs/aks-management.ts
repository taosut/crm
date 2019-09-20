import { debug, Debugger } from 'debug';
import { ContainerServiceClient } from '@azure/arm-containerservice';
import { ManagedCluster } from '@azure/arm-containerservice/esm/models';

import { setupAzureCredential, azureSubscriptionId } from './utils';

export class AksManagement {
  private log: Debugger;
  private containerServiceClient: ContainerServiceClient;

  public constructor() {
    this.log = debug('crm-aks-management');
  }

  public async setupNewCluster(
    resouceGroupName: string,
    clusterName: string,
    settings: ManagedCluster
  ) {
    let cred = await setupAzureCredential();

    this.containerServiceClient = new ContainerServiceClient(cred, azureSubscriptionId);

    this.log(`Start to setup cluster ${clusterName} in resouce group ${resouceGroupName}`);

    var managedClusters = await this.containerServiceClient.managedClusters.listByResourceGroup(
      resouceGroupName
    );
    
    if (managedClusters != null && managedClusters.findIndex(x => x.name === clusterName) > -1) {
      this.log(`Cluster ${clusterName} was existed !!!`);
      this.log(managedClusters[0])
    } else {
      await this.containerServiceClient.managedClusters.createOrUpdate(
        resouceGroupName,
        clusterName,
        settings
      );
    }
  }
}
